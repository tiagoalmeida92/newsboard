using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData;
using NewsBoard.Model;
using NewsBoard.Persistence;

namespace NewsBoard.Web.Controllers.api
{
    /// <summary>
    /// OData Endpoint
    /// For NewsItem consumption
    /// </summary>
    public class NewsItemsController : ODataController
    {
        private NewsDb db = new NewsDb();

        // GET: odata/NewsItems
        [Queryable]
        public IQueryable<NewsItem> GetNewsItems()
        {
            return db.NewsItems.Include(n => n.NewsSource);
        }

        // GET: odata/NewsItems(5)
        [Queryable]
        public SingleResult<NewsItem> GetNewsItem([FromODataUri] string key)
        {
            return SingleResult.Create(db.NewsItems.Where(newsItem => newsItem.Link == key));
        }

        // GET: odata/NewsItems(5)/NewsSource
        [Queryable]
        public SingleResult<NewsSource> GetNewsSource([FromODataUri] string key)
        {
            return SingleResult.Create(db.NewsItems.Where(m => m.Link == key).Select(m => m.NewsSource));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}