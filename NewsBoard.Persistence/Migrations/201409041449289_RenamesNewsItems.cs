using System.Data.Entity.Migrations;

namespace NewsBoard.Persistence.Migrations
{
    public partial class RenamesNewsItems : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NewsItems", "RssCategory", c => c.String());
            DropColumn("dbo.NewsItems", "ManualCategoryName");
        }

        public override void Down()
        {
            AddColumn("dbo.NewsItems", "ManualCategoryName", c => c.String());
            DropColumn("dbo.NewsItems", "RssCategory");
        }
    }
}