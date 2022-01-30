using FluentMigrator;
using JetBrains.Annotations;

namespace Ms.Arch.Hw02.Api.Persistence.Migrations
{
    [Migration(202110262000, "CreateSchemaUserAccount")]
    [UsedImplicitly]
    public class _202110262000_CreateSchema: AutoReversingMigration
    {
        public override void Up()
        {
            Create.Schema("account");
        }
    }
}
