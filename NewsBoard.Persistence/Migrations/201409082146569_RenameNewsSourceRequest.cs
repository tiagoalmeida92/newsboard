using System.Data.Entity.Migrations;

namespace NewsBoard.Persistence.Migrations
{
    public partial class RenameNewsSourceRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NewsSourceRequests", "Requester", c => c.String());
            DropColumn("dbo.NewsSourceRequests", "RequesterNickName");
        }

        public override void Down()
        {
            AddColumn("dbo.NewsSourceRequests", "RequesterNickName", c => c.String());
            DropColumn("dbo.NewsSourceRequests", "Requester");
        }
    }
}