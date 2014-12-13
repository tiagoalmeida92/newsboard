using System.Data.Entity.Migrations;

namespace NewsBoard.Persistence.Migrations
{
    public partial class RenameCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NewsCategories", "Name", c => c.String());
            DropColumn("dbo.NewsCategories", "ManualName");
        }

        public override void Down()
        {
            AddColumn("dbo.NewsCategories", "ManualName", c => c.String());
            DropColumn("dbo.NewsCategories", "Name");
        }
    }
}