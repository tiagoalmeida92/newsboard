using System.Data.Entity.Migrations;

namespace NewsBoard.Persistence.Migrations
{
    public partial class AddRssUrlNewsSourceRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NewsSourceRequests", "Name", c => c.String());
            AddColumn("dbo.NewsSourceRequests", "Url", c => c.String());
            AddColumn("dbo.NewsSourceRequests", "RssUrl", c => c.String());
            DropColumn("dbo.NewsSourceRequests", "SourceName");
            DropColumn("dbo.NewsSourceRequests", "SourceUrl");
            DropColumn("dbo.NewsSourceRequests", "Message");
        }

        public override void Down()
        {
            AddColumn("dbo.NewsSourceRequests", "Message", c => c.String());
            AddColumn("dbo.NewsSourceRequests", "SourceUrl", c => c.String());
            AddColumn("dbo.NewsSourceRequests", "SourceName", c => c.String());
            DropColumn("dbo.NewsSourceRequests", "RssUrl");
            DropColumn("dbo.NewsSourceRequests", "Url");
            DropColumn("dbo.NewsSourceRequests", "Name");
        }
    }
}