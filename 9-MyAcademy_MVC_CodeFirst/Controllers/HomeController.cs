using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using _9_MyAcademy_MVC_CodeFirst.Data.Context;
using _9_MyAcademy_MVC_CodeFirst.Data.Entities;
using _9_MyAcademy_MVC_CodeFirst.Models;
using _9_MyAcademy_MVC_CodeFirst.Services;

namespace _9_MyAcademy_MVC_CodeFirst.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context = new AppDbContext();
        private readonly GeminiService _geminiService = new GeminiService();

        public ActionResult Index()
        {
            var viewModel = new HomeViewModel
            {
                Sliders = _context.Sliders.Where(x => x.IsActive).OrderBy(x => x.OrderNumber).ToList(),
                Features = _context.Features.Where(x => x.IsActive).Take(4).ToList(),
                About = _context.Abouts.FirstOrDefault(x => x.IsActive),
                Services = _context.Products.Where(x => x.IsActive).Take(4).ToList(),
                FAQs = _context.FAQs.Where(x => x.IsActive).OrderBy(x => x.OrderNumber).Take(3).ToList(),
                Blogs = _context.Blogs.Where(x => x.IsActive).OrderByDescending(x => x.PublishDate).Take(3).ToList(),
                TeamMembers = _context.TeamMembers.Where(x => x.IsActive).Take(4).ToList(),
                Testimonials = _context.Testimonials.Where(x => x.IsActive).OrderByDescending(x => x.CreatedDate).Take(3).ToList(),
                ContactInfo = _context.Contacts.FirstOrDefault(x => x.IsActive)
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> GetAIAdvice(string age, string job, string income, string family)
        {
            var advice = await _geminiService.GetInsuranceAdvice(age, job, income, family);
            TempData["AIAdvice"] = advice;
            return RedirectToAction("Index", null, null) ;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            var viewModel = new ContactViewModel
            {
                ContactInfo = _context.Contacts.FirstOrDefault(x => x.IsActive),
                Message = new ContactMessage()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid && model.Message != null)
            {
                model.Message.CreatedDate = DateTime.Now;
                model.Message.IsRead = false;
                _context.ContactMessages.Add(model.Message);
                _context.SaveChanges();
                TempData["Success"] = "Your message has been sent successfully!";
                return RedirectToAction("Contact");
            }

            model.ContactInfo = _context.Contacts.FirstOrDefault(x => x.IsActive);
            return View(model);
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