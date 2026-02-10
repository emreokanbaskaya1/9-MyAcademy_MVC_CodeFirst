using _9_MyAcademy_MVC_CodeFirst.Data.Context;
using _9_MyAcademy_MVC_CodeFirst.Data.Entities;
using System.Linq;
using System.Web.Mvc;

namespace _9_MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    public class AboutController : Controller
    {
        private readonly AppDbContext _context = new AppDbContext();

        // GET: Admin/About
        public ActionResult Index()
        {
            var abouts = _context.Abouts.Where(x => x.IsActive).ToList();
            return View(abouts);
        }

        // GET: Admin/About/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/About/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(About about)
        {
            if (ModelState.IsValid)
            {
                about.IsActive = true;
                _context.Abouts.Add(about);
                _context.SaveChanges();
                TempData["Success"] = "About section created successfully!";
                return RedirectToAction("Index");
            }
            return View(about);
        }

        // GET: Admin/About/Edit/5
        public ActionResult Edit(int id)
        {
            var about = _context.Abouts.Find(id);
            if (about == null)
            {
                return HttpNotFound();
            }
            return View(about);
        }

        // POST: Admin/About/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(About about)
        {
            if (ModelState.IsValid)
            {
                var existing = _context.Abouts.Find(about.Id);
                if (existing != null)
                {
                    existing.Title = about.Title;
                    existing.Subtitle = about.Subtitle;
                    existing.Description = about.Description;
                    existing.ImageUrl = about.ImageUrl;
                    existing.InsurancePolicies = about.InsurancePolicies;
                    existing.AwardsWon = about.AwardsWon;
                    existing.SkilledAgents = about.SkilledAgents;
                    existing.TeamMembers = about.TeamMembers;
                    existing.IsActive = about.IsActive;
                    _context.SaveChanges();
                    TempData["Success"] = "About section updated successfully!";
                    return RedirectToAction("Index");
                }
            }
            return View(about);
        }

        // GET: Admin/About/Delete/5
        public ActionResult Delete(int id)
        {
            var about = _context.Abouts.Find(id);
            if (about != null)
            {
                about.IsActive = false; // Soft delete
                _context.SaveChanges();
                TempData["Success"] = "About section deleted successfully!";
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
