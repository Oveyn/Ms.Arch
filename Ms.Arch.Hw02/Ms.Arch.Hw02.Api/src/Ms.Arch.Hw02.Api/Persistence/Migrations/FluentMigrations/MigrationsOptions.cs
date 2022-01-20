using System.Reflection;

namespace Ms.Arch.Hw02.Api.Persistence.Migrations.FluentMigrations
{
    public sealed class MigrationsOptions
    {
        public bool Enabled { get; set; }

        public string ConnectionString { get; set; }

        public Assembly Assembly { get; set; }

        public string VersionTableSchemaName { get; set; }

        public string VersionTableName { get; set; }

        public string UniqueIndexName { get; set; }
    }
}
