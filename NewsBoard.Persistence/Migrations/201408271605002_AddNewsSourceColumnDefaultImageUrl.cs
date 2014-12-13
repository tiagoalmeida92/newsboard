using System.Data.Entity.Migrations;

namespace NewsBoard.Persistence.Migrations
{
    public partial class AddNewsSourceColumnDefaultImageUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NewsSources", "DefaultImageUrl", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.NewsSources", "DefaultImageUrl");
        }
    }
}