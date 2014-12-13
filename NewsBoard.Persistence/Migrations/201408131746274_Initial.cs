using System.Data.Entity.Migrations;

namespace NewsBoard.Persistence.Migrations
{
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NewsCategories",
                c => new
                {
                    Id = c.Int(false, true),
                    ManualName = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.CategoryDatas",
                c => new
                {
                    Id = c.Int(false, true),
                    Name = c.String(),
                    NewsCategory_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NewsCategories", t => t.NewsCategory_Id)
                .Index(t => t.NewsCategory_Id);

            CreateTable(
                "dbo.NewsItems",
                c => new
                {
                    Link = c.String(false, 2000),
                    Title = c.String(),
                    Description = c.String(),
                    PubDate = c.DateTime(false),
                    ImageLink = c.String(),
                    CategoryName = c.String(),
                    ManualCategoryName = c.String(),
                    NewsSource_Id = c.Int(),
                })
                .PrimaryKey(t => t.Link)
                .ForeignKey("dbo.NewsSources", t => t.NewsSource_Id)
                .Index(t => t.NewsSource_Id);

            CreateTable(
                "dbo.NewsSources",
                c => new
                {
                    Id = c.Int(false, true),
                    Name = c.String(),
                    RssUrl = c.String(),
                    FaviconUrl = c.String(),
                    SiteUrl = c.String(),
                })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropForeignKey("dbo.NewsItems", "NewsSource_Id", "dbo.NewsSources");
            DropForeignKey("dbo.CategoryDatas", "NewsCategory_Id", "dbo.NewsCategories");
            DropIndex("dbo.NewsItems", new[] {"NewsSource_Id"});
            DropIndex("dbo.CategoryDatas", new[] {"NewsCategory_Id"});
            DropTable("dbo.NewsSources");
            DropTable("dbo.NewsItems");
            DropTable("dbo.CategoryDatas");
            DropTable("dbo.NewsCategories");
        }
    }
}