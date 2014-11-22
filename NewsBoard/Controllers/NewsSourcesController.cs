using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using NewsBoard.Model;
using NewsBoard.Persistence;

namespace NewsBoard.Web.Controllers
{
    public class NewsSourcesController : Controller
    {
        private NewsDb db = new NewsDb();

        // GET: NewsSources
        public async Task<ActionResult> Index()
        {
            return View(await db.NewsSources.ToListAsync());
        }

        // GET: NewsSources/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsSource newsSource = await db.NewsSources.FindAsync(id);
            if (newsSource == null)
            {
                return HttpNotFound();
            }
            return View(newsSource);
        }

        // GET: NewsSources/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NewsSources/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            [Bind(Include = "Id,Name,RssUrl,FaviconUrl,SiteUrl")] NewsSource newsSource)
        {
            if (ModelState.IsValid)
            {
                db.NewsSources.Add(newsSource);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(newsSource);
        }

        // GET: NewsSources/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsSource newsSource = await db.NewsSources.FindAsync(id);
            if (newsSource == null)
            {
                return HttpNotFound();
            }
            return View(newsSource);
        }

        // POST: NewsSources/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,RssUrl,FaviconUrl,SiteUrl")] NewsSource newsSource)
        {
            if (ModelState.IsValid)
            {
                db.Entry(newsSource).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(newsSource);
        }

        // GET: NewsSources/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsSource newsSource = await db.NewsSources.FindAsync(id);
            if (newsSource == null)
            {
                return HttpNotFound();
            }
            return View(newsSource);
        }

        // POST: NewsSources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            NewsSource newsSource = await db.NewsSources.FindAsync(id);
            db.NewsSources.Remove(newsSource);
            await db.SaveChangesAsync();
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