using System.Data.Entity.Migrations;

namespace NewsBoard.Web.Models.Migrations
{
    public partial class NewColumnId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NewsSourceIds", "NSourceId", c => c.Int(false));
            AlterColumn("dbo.NewsSourceIds", "Id", c => c.Int(false, true));
        }

        public override void Down()
        {
            AlterColumn("dbo.NewsSourceIds", "Id", c => c.Int(false));
            DropColumn("dbo.NewsSourceIds", "NSourceId");
        }
    }
}