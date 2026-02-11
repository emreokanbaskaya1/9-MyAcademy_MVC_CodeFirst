using _9_MyAcademy_MVC_CodeFirst.Data.Context;
using _9_MyAcademy_MVC_CodeFirst.Data.Entities;
using _9_MyAcademy_MVC_CodeFirst.Filters;
using System;
using System.Linq;
using System.Web.Mvc;

namespace _9_MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    [AdminAuthFilter]
    public class BlogController : Controller
    {
        private readonly AppDbContext _context = new AppDbContext();

        // GET: Admin/Blog
        public ActionResult Index()
        {
            var blogs = _context.Blogs
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.PublishDate)
                .ToList();
            return View(blogs);
        }

        // GET: Admin/Blog/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Blog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Blog blog)
        {
            if (ModelState.IsValid)
            {
                blog.IsActive = true;
                blog.PublishDate = DateTime.Now;
                blog.CommentCount = 0;
                _context.Blogs.Add(blog);
                _context.SaveChanges();
                TempData["Success"] = "Blog post created successfully!";
                return RedirectToAction("Index");
            }
            return View(blog);
        }

        // GET: Admin/Blog/Edit/5
        public ActionResult Edit(int id)
        {
            var blog = _context.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: Admin/Blog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Blog blog)
        {
            if (ModelState.IsValid)
            {
                var existing = _context.Blogs.Find(blog.Id);
                if (existing != null)
                {
                    existing.Title = blog.Title;
                    existing.Description = blog.Description;
                    existing.Content = blog.Content;
                    existing.ImageUrl = blog.ImageUrl;
                    existing.Author = blog.Author;
                    existing.CategoryName = blog.CategoryName;
                    existing.IsActive = blog.IsActive;
                    _context.SaveChanges();
                    TempData["Success"] = "Blog post updated successfully!";
                    return RedirectToAction("Index");
                }
            }
            return View(blog);
        }

        // GET: Admin/Blog/Delete/5
        public ActionResult Delete(int id)
        {
            var blog = _context.Blogs.Find(id);
            if (blog != null)
            {
                blog.IsActive = false; // Soft delete
                _context.SaveChanges();
                TempData["Success"] = "Blog post deleted successfully!";
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
