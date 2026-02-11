using _9_MyAcademy_MVC_CodeFirst.Data.Context;
using _9_MyAcademy_MVC_CodeFirst.Data.Entities;
using _9_MyAcademy_MVC_CodeFirst.Filters;
using System;
using System.Linq;
using System.Web.Mvc;

namespace _9_MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    [AdminAuthFilter]
    public class TestimonialController : Controller
    {
        private readonly AppDbContext _context = new AppDbContext();

        // GET: Admin/Testimonial
        public ActionResult Index()
        {
            var testimonials = _context.Testimonials
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.CreatedDate)
                .ToList();
            return View(testimonials);
        }

        // GET: Admin/Testimonial/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Testimonial/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Testimonial testimonial)
        {
            if (ModelState.IsValid)
            {
                testimonial.IsActive = true;
                testimonial.CreatedDate = DateTime.Now;
                _context.Testimonials.Add(testimonial);
                _context.SaveChanges();
                TempData["Success"] = "Testimonial created successfully!";
                return RedirectToAction("Index");
            }
            return View(testimonial);
        }

        // GET: Admin/Testimonial/Edit/5
        public ActionResult Edit(int id)
        {
            var testimonial = _context.Testimonials.Find(id);
            if (testimonial == null)
            {
                return HttpNotFound();
            }
            return View(testimonial);
        }

        // POST: Admin/Testimonial/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Testimonial testimonial)
        {
            if (ModelState.IsValid)
            {
                var existing = _context.Testimonials.Find(testimonial.Id);
                if (existing != null)
                {
                    existing.ClientName = testimonial.ClientName;
                    existing.Position = testimonial.Position;
                    existing.Comment = testimonial.Comment;
                    existing.ImageUrl = testimonial.ImageUrl;
                    existing.Rating = testimonial.Rating;
                    existing.IsActive = testimonial.IsActive;
                    _context.SaveChanges();
                    TempData["Success"] = "Testimonial updated successfully!";
                    return RedirectToAction("Index");
                }
            }
            return View(testimonial);
        }

        // GET: Admin/Testimonial/Delete/5
        public ActionResult Delete(int id)
        {
            var testimonial = _context.Testimonials.Find(id);
            if (testimonial != null)
            {
                testimonial.IsActive = false; // Soft delete
                _context.SaveChanges();
                TempData["Success"] = "Testimonial deleted successfully!";
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
