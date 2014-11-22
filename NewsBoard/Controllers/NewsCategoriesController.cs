using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using NewsBoard.Model;
using NewsBoard.Persistence;

namespace NewsBoard.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class NewsCategoriesController : Controller
    {
        private NewsDb db = new NewsDb();

        // GET: /NewsCategories/
        public ActionResult Index()
        {
            return View(db.NewsCategories.ToList());
        }

        // GET: /NewsCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsCategory newscategory = db.NewsCategories.Find(id);
            if (newscategory == null)
            {
                return HttpNotFound();
            }
            return View(newscategory);
        }

        // GET: /NewsCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /NewsCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] NewsCategory newscategory)
        {
            if (ModelState.IsValid)
            {
                db.NewsCategories.Add(newscategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(newscategory);
        }

        // GET: /NewsCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsCategory newscategory = db.NewsCategories.Find(id);
            if (newscategory == null)
            {
                return HttpNotFound();
            }
            return View(newscategory);
        }

        // POST: /NewsCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] NewsCategory newscategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(newscategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(newscategory);
        }

        // GET: /NewsCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsCategory newscategory = db.NewsCategories.Find(id);
            if (newscategory == null)
            {
                return HttpNotFound();
            }
            return View(newscategory);
        }

        // POST: /NewsCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NewsCategory newscategory = db.NewsCategories.Find(id);
            db.NewsCategories.Remove(newscategory);
            db.SaveChanges();
            return RedirectToAction("Index");
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