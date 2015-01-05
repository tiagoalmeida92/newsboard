using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using NewsBoard.Model;
using NewsBoard.Persistence;
using NewsBoard.Web.ViewModels;

namespace NewsBoard.Web.Controllers
{
    public class SearchController : Controller
    {
        public NewsDb _db = new NewsDb();

        //
        // GET: /Search/
        public ActionResult Index(string q, int? ns)
        {
            var viewModel = new SearchViewModel
            {
                Results = Enumerable.Empty<NewsItem>(),
                Query = q,
                NewsSources = _db.NewsSources
            };
            if (q.IsNullOrWhiteSpace())
            {
                return View(viewModel);
            }
            //var search = new SearchDecorator(new NewsIndexer());
            //List<string> searchResult = search.Search(q).ToList();
            //IQueryable<NewsItem> query = _db.NewsItems.Where(n => searchResult.Contains(n.Link));
            //if (ns.HasValue) query = query.Where(n => n.NewsSource.Id == ns.Value);
            //viewModel.Results = query.OrderByDescending(ni => ni.PubDate);
            return View(viewModel);
        }

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