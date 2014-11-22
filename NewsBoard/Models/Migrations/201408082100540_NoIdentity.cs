using System.Data.Entity.Migrations;

namespace NewsBoard.Web.Models.Migrations
{
    public partial class NoIdentity : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.NewsSourceIds", "Id", c => c.Int(false));
        }

        public override void Down()
        {
            AlterColumn("dbo.NewsSourceIds", "Id", c => c.Int(false, true));
        }
    }
}