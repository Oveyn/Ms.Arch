using FluentMigrator;

namespace Ms.Arch.Hw02.Api.Persistence.Migrations
{
    [Migration(202110262000, "CreateSchemaUserAccount")]
    public class _202110262000_CreateSchema: AutoReversingMigration
    {
        public override void Up()
        {
            Create.Schema("account");
        }
    }
}
