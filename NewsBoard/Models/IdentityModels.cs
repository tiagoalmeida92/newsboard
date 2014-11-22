using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace NewsBoard.Web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [NotMapped]
        public IEnumerable<int> IgnoredNewsSourcesIds
        {
            get
            {
                if (InternalData.IsNullOrWhiteSpace())
                {
                    return Enumerable.Empty<int>();
                }
                string[] tab = InternalData.Split(',');
                return tab.Select(int.Parse);
            }
            set { InternalData = string.Join(",", value); }
        }

        public string InternalData { get; set; }
    }


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("IdentityConnection")
        {
        }
    }
}