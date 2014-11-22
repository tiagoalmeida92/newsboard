using System.Data.Entity.Migrations;

namespace NewsBoard.Web.Models.Migrations
{
    public partial class AppUserUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.NewsSourceIds", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.NewsSourceIds", new[] {"ApplicationUser_Id"});
            AddColumn("dbo.AspNetUsers", "InternalData", c => c.String());
            DropTable("dbo.NewsSourceIds");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.NewsSourceIds",
                c => new
                {
                    Id = c.Int(false, true),
                    NSourceId = c.Int(false),
                    ApplicationUser_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.Id);

            DropColumn("dbo.AspNetUsers", "InternalData");
            CreateIndex("dbo.NewsSourceIds", "ApplicationUser_Id");
            AddForeignKey("dbo.NewsSourceIds", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}