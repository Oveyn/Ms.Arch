using FluentMigrator.Runner.VersionTableInfo;

namespace Ms.Arch.Hw02.Api.Persistence.Migrations.FluentMigrations
{
    internal sealed class VersionTable : IVersionTableMetaData
    {
        public object ApplicationContext { get; set; }

        public bool OwnsSchema { get; set; }

        public string SchemaName { get; set; }

        public string TableName { get; set; }

        public string ColumnName { get; set; }

        public string DescriptionColumnName { get; set; }

        public string UniqueIndexName { get; set; }

        public string AppliedOnColumnName { get; set; }
    }
}
