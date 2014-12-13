using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using NewsBoard.Indexer;
using NewsBoard.Model;
using NewsBoard.Model.Categories;
using NewsBoard.Persistence;
using NewsBoard.Web.ViewModels;

namespace NewsBoard.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private NewsDb _db = new NewsDb();

        //
        // GET: /Admin/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult News(string category)
        {
            if (category.IsNullOrWhiteSpace())
            {
                category = Constants.Constants.DEFAULTMANUALCATEGORY;
            }
            var vm = new AdminNewsViewModel
            {
                Category = category,
                NewsCategories = _db.NewsCategories.Select(nc => nc.Name),
                News = _db.NewsItems.Where(n => n.CategoryName == category),
            };

            return View(vm);
        }

        [HttpPost]
        public ActionResult Categorize(string newsitemId, string category, string topWords)
        {
            NewsItem newsItem = _db.NewsItems.Find(newsitemId);
            newsItem.CategoryName = category;
            var wordsdecorator =
                new WordsDecorator<UndifferentiatedCategory>(new ManualWordsIndexer());
            wordsdecorator.Index(
                new UndifferentiatedCategory
                {
                    Title = newsItem.Title,
                    Category = category,
                    Link = newsitemId,
                    TopSubjectsWords = topWords
                }, false);
            _db.SaveChanges();
            return new EmptyResult();
        }

        //AJAX
        [HttpGet]
        public ActionResult GetTopWords(string newsitemId)
        {
            var wordsdecorator =
                new WordsDecorator<UndifferentiatedCategory>(new ManualWordsIndexer());
            int res;
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data =
                    wordsdecorator.GetTopWordsFilter(Constants.Constants.ARTICLE_FIELD, Constants.Constants.LINK_FIELD,
                        newsitemId, out res)
            };
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