using _9_MyAcademy_MVC_CodeFirst.Data.Context;
using _9_MyAcademy_MVC_CodeFirst.Data.Entities;
using System.Linq;
using System.Web.Mvc;

namespace _9_MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context = new AppDbContext();

        // GET: Admin/Contact
        public ActionResult Index()
        {
            var contacts = _context.Contacts.Where(x => x.IsActive).ToList();
            return View(contacts);
        }

        // GET: Admin/Contact/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Contact/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.IsActive = true;
                _context.Contacts.Add(contact);
                _context.SaveChanges();
                TempData["Success"] = "Contact information created successfully!";
                return RedirectToAction("Index");
            }
            return View(contact);
        }

        // GET: Admin/Contact/Edit/5
        public ActionResult Edit(int id)
        {
            var contact = _context.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Admin/Contact/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Contact contact)
        {
            if (ModelState.IsValid)
            {
                var existing = _context.Contacts.Find(contact.Id);
                if (existing != null)
                {
                    existing.Address = contact.Address;
                    existing.Email = contact.Email;
                    existing.Phone = contact.Phone;
                    existing.FacebookUrl = contact.FacebookUrl;
                    existing.TwitterUrl = contact.TwitterUrl;
                    existing.InstagramUrl = contact.InstagramUrl;
                    existing.LinkedInUrl = contact.LinkedInUrl;
                    existing.IsActive = contact.IsActive;
                    _context.SaveChanges();
                    TempData["Success"] = "Contact information updated successfully!";
                    return RedirectToAction("Index");
                }
            }
            return View(contact);
        }

        // GET: Admin/Contact/Delete/5
        public ActionResult Delete(int id)
        {
            var contact = _context.Contacts.Find(id);
            if (contact != null)
            {
                contact.IsActive = false; // Soft delete
                _context.SaveChanges();
                TempData["Success"] = "Contact information deleted successfully!";
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
