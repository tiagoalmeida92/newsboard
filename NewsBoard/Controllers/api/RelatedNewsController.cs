using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using NewsBoard.Model;
using NewsBoard.Persistence;
using NewsBoard.Web.ViewModels;

namespace NewsBoard.Web.Controllers.api
{
    public class RelatedNewsController : ApiController
    {
        private NewsDb _db = new NewsDb();

        // GET: api/RelatedNews
        /// <summary>
        ///     Executes a relation search between news using Lucene index.
        /// </summary>
        /// <param name="link">The id of the newsitem to perform the relation search</param>
        /// <returns>A list of RelatedNewsItems which have the NewsItem and the percentage of relation between them</returns>
        //public IEnumerable<RelatedNewsItem> Get(string link)
        //{
        //    if (link == null) return Enumerable.Empty<RelatedNewsItem>();
        //    var decorator = new NewsRelationshipDecorator(new NewsIndexer());
        //    IDictionary<string,float> res = decorator.GetNewsRelated(link);
        //    if (res == null) return Enumerable.Empty<RelatedNewsItem>();
            
        //    var newsitems = _db.NewsItems.Where(ni => res.Keys.Contains(ni.Link)).ToList();

        //    return newsitems.Zip(
        //        res.Values, 
        //        (item, f) => new RelatedNewsItem{NewsItem = item, Percentage = (int)(f*100)});
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}