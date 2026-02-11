using _9_MyAcademy_MVC_CodeFirst.Data.Context;
using _9_MyAcademy_MVC_CodeFirst.Data.Entities;
using _9_MyAcademy_MVC_CodeFirst.Filters;
using System.Linq;
using System.Web.Mvc;

namespace _9_MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    [AdminAuthFilter]
    public class FAQController : Controller
    {
        private readonly AppDbContext _context = new AppDbContext();

        // GET: Admin/FAQ
        public ActionResult Index()
        {
            var faqs = _context.FAQs
                .Where(x => x.IsActive)
                .OrderBy(x => x.OrderNumber)
                .ToList();
            return View(faqs);
        }

        // GET: Admin/FAQ/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/FAQ/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FAQ faq)
        {
            if (ModelState.IsValid)
            {
                faq.IsActive = true;
                _context.FAQs.Add(faq);
                _context.SaveChanges();
                TempData["Success"] = "FAQ created successfully!";
                return RedirectToAction("Index");
            }
            return View(faq);
        }

        // GET: Admin/FAQ/Edit/5
        public ActionResult Edit(int id)
        {
            var faq = _context.FAQs.Find(id);
            if (faq == null)
            {
                return HttpNotFound();
            }
            return View(faq);
        }

        // POST: Admin/FAQ/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FAQ faq)
        {
            if (ModelState.IsValid)
            {
                var existing = _context.FAQs.Find(faq.Id);
                if (existing != null)
                {
                    existing.Question = faq.Question;
                    existing.Answer = faq.Answer;
                    existing.OrderNumber = faq.OrderNumber;
                    existing.IsActive = faq.IsActive;
                    _context.SaveChanges();
                    TempData["Success"] = "FAQ updated successfully!";
                    return RedirectToAction("Index");
                }
            }
            return View(faq);
        }

        // GET: Admin/FAQ/Delete/5
        public ActionResult Delete(int id)
        {
            var faq = _context.FAQs.Find(id);
            if (faq != null)
            {
                faq.IsActive = false; // Soft delete
                _context.SaveChanges();
                TempData["Success"] = "FAQ deleted successfully!";
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
