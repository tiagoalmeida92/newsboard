using System.Data.Entity.Migrations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace NewsBoard.Web.Models.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            MigrationsDirectory = @"Models\Migrations";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            const string name = "admin";
            const string role = "admin";
            const string password = "123456";

            if (!roleManager.RoleExists(role))
            {
                roleManager.Create(new IdentityRole(name));
            }

            var user = new ApplicationUser {UserName = name};
            IdentityResult adminresult = userManager.Create(user, password);

            if (adminresult.Succeeded)
            {
                userManager.AddToRole(user.Id, name);
            }
            base.Seed(context);
        }
    }
}