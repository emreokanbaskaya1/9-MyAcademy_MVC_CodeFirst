using _9_MyAcademy_MVC_CodeFirst.Data.Context;
using _9_MyAcademy_MVC_CodeFirst.Data.Entities;
using System.Linq;
using System.Web.Mvc;

namespace _9_MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    public class SliderController : Controller
    {
        private readonly AppDbContext _context = new AppDbContext();

        // GET: Admin/Slider
        public ActionResult Index()
        {
            var sliders = _context.Sliders
                .Where(x => x.IsActive)
                .OrderBy(x => x.OrderNumber)
                .ToList();
            return View(sliders);
        }

        // GET: Admin/Slider/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Slider/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Slider slider)
        {
            if (ModelState.IsValid)
            {
                slider.IsActive = true;
                _context.Sliders.Add(slider);
                _context.SaveChanges();
                TempData["Success"] = "Slider created successfully!";
                return RedirectToAction("Index");
            }
            return View(slider);
        }

        // GET: Admin/Slider/Edit/5
        public ActionResult Edit(int id)
        {
            var slider = _context.Sliders.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: Admin/Slider/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Slider slider)
        {
            if (ModelState.IsValid)
            {
                var existing = _context.Sliders.Find(slider.Id);
                if (existing != null)
                {
                    existing.Title = slider.Title;
                    existing.Subtitle = slider.Subtitle;
                    existing.Description = slider.Description;
                    existing.ImageUrl = slider.ImageUrl;
                    existing.ButtonText = slider.ButtonText;
                    existing.ButtonUrl = slider.ButtonUrl;
                    existing.OrderNumber = slider.OrderNumber;
                    existing.IsActive = slider.IsActive;
                    _context.SaveChanges();
                    TempData["Success"] = "Slider updated successfully!";
                    return RedirectToAction("Index");
                }
            }
            return View(slider);
        }

        // GET: Admin/Slider/Delete/5
        public ActionResult Delete(int id)
        {
            var slider = _context.Sliders.Find(id);
            if (slider != null)
            {
                slider.IsActive = false; // Soft delete
                _context.SaveChanges();
                TempData["Success"] = "Slider deleted successfully!";
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
