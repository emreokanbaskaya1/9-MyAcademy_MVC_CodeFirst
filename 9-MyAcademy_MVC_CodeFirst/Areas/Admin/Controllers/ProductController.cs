using _9_MyAcademy_MVC_CodeFirst.Data.Context;
using _9_MyAcademy_MVC_CodeFirst.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _9_MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{

    public class ProductController : Controller
    {
        private readonly AppDbContext context = new AppDbContext();

        private void GetCategories()
        {
            var categories = context.Categories.ToList();
            ViewBag.categories = (from category in categories
                                  select new SelectListItem
                                  {
                                      Text = category.Name,
                                      Value= category.Id.ToString()
                                  }).ToList();
        }


        public ActionResult Index()
        {
            var products = context.Products.Where(x => x.IsActive).ToList();
            return View(products);
        }

        public ActionResult CreateProduct()
        {
            GetCategories();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                product.IsActive = true;
                context.Products.Add(product);
                context.SaveChanges();
                TempData["Success"] = "Product created successfully!";
                return RedirectToAction(nameof(Index));
            }
            GetCategories();
            return View(product);
        }

        public ActionResult UpdateProduct(int id)
        {
            GetCategories();
            var product = context.Products.Find(id);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProduct(Product model)
        {
            if (ModelState.IsValid)
            {
                var product = context.Products.Find(model.Id);
                product.Name = model.Name;
                product.Description = model.Description;
                product.ImageUrl = model.ImageUrl;
                product.Price = model.Price;
                product.CategoryId = model.CategoryId;
                product.IsActive = model.IsActive;
                context.SaveChanges();
                TempData["Success"] = "Product updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            GetCategories();
            return View(model);
        }

        public ActionResult DeleteProduct(int id)
        {
            var product = context.Products.Find(id);
            product.IsActive = false;
            context.SaveChanges();
            TempData["Success"] = "Product deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}