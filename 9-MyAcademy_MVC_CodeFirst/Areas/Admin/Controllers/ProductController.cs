using _9_MyAcademy_MVC_CodeFirst.Data.Context;
using _9_MyAcademy_MVC_CodeFirst.Data.Entities;
using System.Linq;
using System.Web.Mvc;

namespace _9_MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context = new AppDbContext();

        private void GetCategories()
        {
            var categories = _context.Categories.ToList();
            ViewBag.categories = (from category in categories
                                  select new SelectListItem
                                  {
                                      Text = category.Name,
                                      Value = category.Id.ToString()
                                  }).ToList();
        }

        public ActionResult Index()
        {
            var products = _context.Products.Where(x => x.IsActive).ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            GetCategories();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                product.IsActive = true;
                _context.Products.Add(product);
                _context.SaveChanges();
                TempData["Success"] = "Product created successfully!";
                return RedirectToAction("Index");
            }
            GetCategories();
            return View(product);
        }

        public ActionResult Edit(int id)
        {
            GetCategories();
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product model)
        {
            if (ModelState.IsValid)
            {
                var product = _context.Products.Find(model.Id);
                if (product != null)
                {
                    product.Name = model.Name;
                    product.Description = model.Description;
                    product.ImageUrl = model.ImageUrl;
                    product.Price = model.Price;
                    product.CategoryId = model.CategoryId;
                    product.IsActive = model.IsActive;
                    _context.SaveChanges();
                    TempData["Success"] = "Product updated successfully!";
                    return RedirectToAction("Index");
                }
            }
            GetCategories();
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                product.IsActive = false;
                _context.SaveChanges();
                TempData["Success"] = "Product deleted successfully!";
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