using System.Data.Entity;
using System.Data.Entity.SqlServer;
using NewsBoard.Model;

namespace NewsBoard.Persistence
{
    public class NewsDb : DbContext
    {
        public NewsDb()
            : base("DefaultConnection")
        {
            // ROLA - This is a hack to ensure that Entity Framework SQL Provider is copied across to the output folder.
            // As it is installed in the GAC, Copy Local does not work. It is required for probing.
            // Fixed "Provider not loaded" error
            //Reference: http://robsneuron.blogspot.pt/2013/11/entity-framework-upgrade-to-6.html
            SqlProviderServices ensureDLLIsCopied = SqlProviderServices.Instance;
        }

        /// <summary>
        ///     Code First table to store News 
        /// </summary>
        public DbSet<NewsItem> NewsItems { get; set; }

        /// <summary>
        ///     Code First table to store News Sources
        /// </summary>
        public DbSet<NewsSource> NewsSources { get; set; }

        /// <summary>
        ///     Code First table to store News Categories. Can be changed or updated by Administrator.
        /// </summary>
        public DbSet<NewsCategory> NewsCategories { get; set; }

        /// <summary>
        /// Code first table to store requests made by users to add new News Sources. 
        /// </summary>
        public DbSet<NewsSourceRequest> NewsSourceRequests { get; set; }
    }
}