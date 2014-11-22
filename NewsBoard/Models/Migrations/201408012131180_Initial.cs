using System.Data.Entity.Migrations;

namespace NewsBoard.Web.Models.Migrations
{
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                {
                    Id = c.String(false, 128),
                    Name = c.String(false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.AspNetUsers",
                c => new
                {
                    Id = c.String(false, 128),
                    UserName = c.String(),
                    PasswordHash = c.String(),
                    SecurityStamp = c.String(),
                    Discriminator = c.String(false, 128),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                {
                    Id = c.Int(false, true),
                    ClaimType = c.String(),
                    ClaimValue = c.String(),
                    User_Id = c.String(false, 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, true)
                .Index(t => t.User_Id);

            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                {
                    UserId = c.String(false, 128),
                    LoginProvider = c.String(false, 128),
                    ProviderKey = c.String(false, 128),
                })
                .PrimaryKey(t => new {t.UserId, t.LoginProvider, t.ProviderKey})
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                {
                    UserId = c.String(false, 128),
                    RoleId = c.String(false, 128),
                })
                .PrimaryKey(t => new {t.UserId, t.RoleId})
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.NewsSourceIds",
                c => new
                {
                    Id = c.Int(false, true),
                    ApplicationUser_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
        }

        public override void Down()
        {
            DropForeignKey("dbo.NewsSourceIds", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.NewsSourceIds", new[] {"ApplicationUser_Id"});
            DropIndex("dbo.AspNetUserClaims", new[] {"User_Id"});
            DropIndex("dbo.AspNetUserRoles", new[] {"UserId"});
            DropIndex("dbo.AspNetUserRoles", new[] {"RoleId"});
            DropIndex("dbo.AspNetUserLogins", new[] {"UserId"});
            DropTable("dbo.NewsSourceIds");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetRoles");
        }
    }
}