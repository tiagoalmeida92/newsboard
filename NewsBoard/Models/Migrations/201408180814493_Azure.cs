using System.Data.Entity.Migrations;

namespace NewsBoard.Web.Models.Migrations
{
    public partial class Azure : DbMigration
    {
        public override void Up()
        {
            //CreateIndex("dbo.AspNetUserClaims", "User_Id");
            //CreateIndex("dbo.AspNetUserLogins", "UserId");
            //CreateIndex("dbo.AspNetUserRoles", "UserId");
            //CreateIndex("dbo.AspNetUserRoles", "RoleId");
        }

        public override void Down()
        {
            //DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            //DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            //DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            //DropIndex("dbo.AspNetUserClaims", new[] { "User_Id" });
        }
    }
}