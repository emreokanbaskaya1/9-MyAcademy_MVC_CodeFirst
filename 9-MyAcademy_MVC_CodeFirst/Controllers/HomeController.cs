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
using System.Diagnostics;

namespace _9_MyAcademy_MVC_CodeFirst.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context = new AppDbContext();
        private readonly GeminiService _geminiService = new GeminiService();
        private readonly EmailService _emailService = new EmailService();
        private readonly HuggingFaceService _huggingFaceService = new HuggingFaceService();

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
        public async Task<ActionResult> Contact(ContactViewModel model)
        {
            if (ModelState.IsValid && model.Message != null)
            {
                try
                {
                    model.Message.CreatedDate = DateTime.Now;
                    model.Message.IsRead = false;

                    // Classify message using Hugging Face AI
                    string aiCategory = "General";
                    double aiConfidence = 0;
                    bool aiIsUrgent = false;

                    try
                    {
                        var classification = await _huggingFaceService.ClassifyMessage(model.Message.Message);
                        aiCategory = classification.Category;
                        aiConfidence = classification.Confidence;
                        aiIsUrgent = classification.IsUrgent;

                        model.Message.AICategory = aiCategory;
                        model.Message.AIConfidence = aiConfidence;
                        model.Message.AIIsUrgent = aiIsUrgent;

                        Debug.WriteLine($"Message classified as: {aiCategory} ({aiConfidence}%)");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Failed to classify message: {ex.Message}");
                        model.Message.AICategory = aiCategory;
                        model.Message.AIConfidence = aiConfidence;
                        model.Message.AIIsUrgent = aiIsUrgent;
                    }

                    // Save message to database
                    _context.ContactMessages.Add(model.Message);
                    _context.SaveChanges();

                    // Send notification email to admin with AI category
                    try
                    {
                        await _emailService.SendContactNotificationToAdmin(
                            model.Message.Name,
                            model.Message.Email,
                            model.Message.Phone,
                            model.Message.Subject,
                            model.Message.Message,
                            model.Message.InsuranceType,
                            aiCategory,
                            aiConfidence,
                            aiIsUrgent
                        );
                        Debug.WriteLine("Admin notification email sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Failed to send admin notification: {ex.Message}");
                    }

                    // Generate AI response and send auto-reply to customer with category
                    try
                    {
                        var aiResponse = await _geminiService.GenerateContactAutoReply(
                            model.Message.Name,
                            model.Message.Subject,
                            model.Message.Message
                        );

                        await _emailService.SendAutoReplyToCustomer(
                            model.Message.Email,
                            model.Message.Name,
                            model.Message.Subject,
                            aiResponse,
                            aiCategory
                        );
                        Debug.WriteLine("Auto-reply email sent successfully to customer.");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Failed to send auto-reply: {ex.Message}");
                    }

                    TempData["Success"] = "Your message has been sent successfully! We will get back to you soon.";
                    return RedirectToAction("Contact");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error processing contact form: {ex.Message}");
                    TempData["Error"] = "An error occurred while sending your message. Please try again.";
                }
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