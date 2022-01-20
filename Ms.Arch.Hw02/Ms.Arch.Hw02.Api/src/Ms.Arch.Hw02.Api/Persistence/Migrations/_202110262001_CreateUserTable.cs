using FluentMigrator;

namespace Ms.Arch.Hw02.Api.Persistence.Migrations
{
    [Migration(202110262001, "CreateTableUserAccount")]
    public class _202110262001_CreateUserTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("account").InSchema("account")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("firstname").AsString(256).NotNullable()
                .WithColumn("lastname").AsString(256)
                .WithColumn("phone").AsString(32)
                .WithColumn("email").AsString(64);
        }
    }
}