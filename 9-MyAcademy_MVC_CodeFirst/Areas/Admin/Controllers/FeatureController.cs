using _9_MyAcademy_MVC_CodeFirst.Data.Context;
using _9_MyAcademy_MVC_CodeFirst.Data.Entities;
using System.Linq;
using System.Web.Mvc;

namespace _9_MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    public class FeatureController : Controller
    {
        private readonly AppDbContext _context = new AppDbContext();

        // GET: Admin/Feature
        public ActionResult Index()
        {
            var features = _context.Features.Where(x => x.IsActive).ToList();
            return View(features);
        }

        // GET: Admin/Feature/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Feature/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Feature feature)
        {
            if (ModelState.IsValid)
            {
                feature.IsActive = true;
                _context.Features.Add(feature);
                _context.SaveChanges();
                TempData["Success"] = "Feature created successfully!";
                return RedirectToAction("Index");
            }
            return View(feature);
        }

        // GET: Admin/Feature/Edit/5
        public ActionResult Edit(int id)
        {
            var feature = _context.Features.Find(id);
            if (feature == null)
            {
                return HttpNotFound();
            }
            return View(feature);
        }

        // POST: Admin/Feature/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Feature feature)
        {
            if (ModelState.IsValid)
            {
                var existingFeature = _context.Features.Find(feature.Id);
                if (existingFeature != null)
                {
                    existingFeature.Icon = feature.Icon;
                    existingFeature.Title = feature.Title;
                    existingFeature.Description = feature.Description;
                    existingFeature.IsActive = feature.IsActive;
                    _context.SaveChanges();
                    TempData["Success"] = "Feature updated successfully!";
                    return RedirectToAction("Index");
                }
            }
            return View(feature);
        }

        // GET: Admin/Feature/Delete/5
        public ActionResult Delete(int id)
        {
            var feature = _context.Features.Find(id);
            if (feature != null)
            {
                feature.IsActive = false; // Soft delete
                _context.SaveChanges();
                TempData["Success"] = "Feature deleted successfully!";
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
