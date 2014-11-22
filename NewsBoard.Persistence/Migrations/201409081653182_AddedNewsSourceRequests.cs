using System.Data.Entity.Migrations;

namespace NewsBoard.Persistence.Migrations
{
    public partial class AddedNewsSourceRequests : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NewsSourceRequests",
                c => new
                {
                    Id = c.Int(false, true),
                    RequesterNickName = c.String(),
                    SourceName = c.String(),
                    SourceUrl = c.String(),
                    Message = c.String(),
                })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropTable("dbo.NewsSourceRequests");
        }
    }
}