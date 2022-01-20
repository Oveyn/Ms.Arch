using System;
using Microsoft.Extensions.DependencyInjection;

namespace Ms.Arch.Hw02.Api.Persistence.Migrations.FluentMigrations
{
    public static class MigratorExtensions
    {
        public static void Migrate(this IServiceProvider serviceProvider)
        {
            var migrator = serviceProvider.GetRequiredService<MigrationsService>();
            migrator.Migrate();
        }
    }
}