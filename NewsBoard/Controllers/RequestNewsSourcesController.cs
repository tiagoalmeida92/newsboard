using System.Linq;
using System.Web.Mvc;
using NewsBoard.Model;
using NewsBoard.Persistence;

namespace NewsBoard.Web.Controllers
{
    [Authorize]
    public class RequestNewsSourcesController : Controller
    {
        private NewsDb _db = new NewsDb();


        // GET: NewsSourcesRequest
        [Authorize(Roles = "admin")]
        public ActionResult Admin()
        {
            return View(_db.NewsSourceRequests);
        }

        public ActionResult Index(string q)
        {
            return View(model: q);
        }

        public ActionResult Request(NewsSourceRequest request)
        {
            string username = User.Identity.Name;
            bool find =
                _db.NewsSourceRequests.Any(
                    nsr => nsr.Requester == username && nsr.Name == request.Name);
            if (!find)
            {
                request.Requester = username;
                _db.NewsSourceRequests.Add(request);
                _db.SaveChanges();
            }
            return new EmptyResult();
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