using System;
using System.Reflection;
using FluentMigrator.Runner;
using FluentMigrator.Runner.VersionTableInfo;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Ms.Arch.Hw02.Api.Persistence.Migrations.FluentMigrations
{
    public class MigrationsService
    {
        private readonly MigrationsOptions _options;

        public MigrationsService(
            IOptions<MigrationsOptions> options)
        {
            _options = options.Value;
        }

        public void Migrate()
        {
            if (!_options.Enabled)
                return;

            var connectionString = _options.ConnectionString;
            if (String.IsNullOrEmpty(connectionString))
                return;

            if (_options.Assembly == null)
                return;

            var tableMetadata = CreateVersionTable(
                _options.VersionTableName,
                _options.VersionTableSchemaName,
                _options.UniqueIndexName);

            using var serviceProvider = CreateServiceProvider(connectionString, tableMetadata, _options.Assembly);
            using var scope = serviceProvider.CreateScope();

            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            if (runner.HasMigrationsToApplyUp())
            {
                runner.MigrateUp();
            }
            else
            {
                Console.WriteLine("No migrations to apply...");
                Console.WriteLine();
            }
        }


        private static ServiceProvider CreateServiceProvider(
            string connectionString,
            IVersionTableMetaData versionTable,
            Assembly assembly)
        {
            var services = new ServiceCollection();

            services.Configure<FluentMigratorLoggerOptions>(options =>
            {
                options.ShowSql = true;
                options.ShowElapsedTime = true;
            });

            services
                .AddFluentMigratorCore()
                .ConfigureRunner(builder => builder
                    .AddPostgres()
                    .WithVersionTable(versionTable)
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(assembly).For.All())
                .AddLogging(builder => builder
                    .AddFluentMigratorConsole());

            return services.BuildServiceProvider();
        }

        private static IVersionTableMetaData CreateVersionTable(
            string tableName,
            string schemaName,
            string uniqueIndexName)
        {
            var result = new VersionTable
            {
                SchemaName = schemaName ?? "public",
                TableName = tableName ?? "account_migration",
                ColumnName = "version",
                UniqueIndexName = uniqueIndexName ?? "idx_migration_version",
                AppliedOnColumnName = "date",
                DescriptionColumnName = "name",
                OwnsSchema = true,
            };

            return result;
        }
    }
}
