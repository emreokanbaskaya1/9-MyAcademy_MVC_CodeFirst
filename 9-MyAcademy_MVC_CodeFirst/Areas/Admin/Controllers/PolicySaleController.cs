using _9_MyAcademy_MVC_CodeFirst.Data.Context;
using _9_MyAcademy_MVC_CodeFirst.Data.Entities;
using _9_MyAcademy_MVC_CodeFirst.Filters;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace _9_MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    [AdminAuthFilter]
    public class PolicySaleController : Controller
    {
        private readonly AppDbContext _context = new AppDbContext();

        // GET: Admin/PolicySale
        public ActionResult Index()
        {
            var policySales = _context.PolicySales
                .Include(p => p.Product)
                .OrderByDescending(x => x.CreatedDate)
                .ToList();
            return View(policySales);
        }

        // GET: Admin/PolicySale/Create
        public ActionResult Create()
        {
            LoadProducts();
            return View();
        }

        // POST: Admin/PolicySale/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PolicySale policySale, string manualStatus)
        {
            if (ModelState.IsValid)
            {
                // Get product price if sale amount not set
                if (policySale.SaleAmount == 0)
                {
                    var product = _context.Products.Find(policySale.ProductId);
                    if (product != null)
                    {
                        policySale.SaleAmount = product.Price;
                    }
                }

                policySale.IsActive = true;
                policySale.CreatedDate = DateTime.Now;
                policySale.CreatedBy = User.Identity.Name ?? "Admin";
                
                // Handle manual status
                if (!string.IsNullOrEmpty(manualStatus))
                {
                    PolicyStatus status;
                    if (Enum.TryParse(manualStatus, out status))
                    {
                        policySale.SetManualStatus(status);
                    }
                }
                
                _context.PolicySales.Add(policySale);
                _context.SaveChanges();
                
                TempData["Success"] = $"Policy sale for {policySale.FullName} created successfully!";
                return RedirectToAction("Index");
            }
            
            LoadProducts();
            return View(policySale);
        }

        // GET: Admin/PolicySale/Edit/5
        public ActionResult Edit(int id)
        {
            var policySale = _context.PolicySales
                .Include(p => p.Product)
                .FirstOrDefault(p => p.Id == id);
                
            if (policySale == null)
            {
                return HttpNotFound();
            }
            
            LoadProducts();
            return View(policySale);
        }

        // POST: Admin/PolicySale/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PolicySale policySale, string manualStatus)
        {
            if (ModelState.IsValid)
            {
                var existing = _context.PolicySales.Find(policySale.Id);
                if (existing != null)
                {
                    existing.FirstName = policySale.FirstName;
                    existing.LastName = policySale.LastName;
                    existing.Email = policySale.Email;
                    existing.PhoneNumber = policySale.PhoneNumber;
                    existing.TCIdentityNumber = policySale.TCIdentityNumber;
                    existing.Address = policySale.Address;
                    existing.ProductId = policySale.ProductId;
                    existing.SaleAmount = policySale.SaleAmount;
                    existing.PolicyStartDate = policySale.PolicyStartDate;
                    existing.PolicyEndDate = policySale.PolicyEndDate;
                    existing.PaymentStatus = policySale.PaymentStatus;
                    existing.Notes = policySale.Notes;
                    existing.UpdatedDate = DateTime.Now;
                    existing.IsActive = policySale.IsActive;
                    
                    // Handle manual status
                    if (!string.IsNullOrEmpty(manualStatus))
                    {
                        PolicyStatus status;
                        if (Enum.TryParse(manualStatus, out status))
                        {
                            existing.SetManualStatus(status);
                        }
                    }
                    else
                    {
                        existing.ManualPolicyStatus = null;
                    }
                    
                    _context.SaveChanges();
                    TempData["Success"] = $"Policy sale for {existing.FullName} updated successfully!";
                    return RedirectToAction("Index");
                }
            }
            
            LoadProducts();
            return View(policySale);
        }

        // GET: Admin/PolicySale/Details/5
        public ActionResult Details(int id)
        {
            var policySale = _context.PolicySales
                .Include(p => p.Product)
                .Include(p => p.Product.Category)
                .FirstOrDefault(p => p.Id == id);
                
            if (policySale == null)
            {
                return HttpNotFound();
            }
            
            return View(policySale);
        }

        // GET: Admin/PolicySale/Delete/5
        public ActionResult Delete(int id)
        {
            var policySale = _context.PolicySales.Find(id);
            if (policySale != null)
            {
                policySale.IsActive = false; // Soft delete
                policySale.UpdatedDate = DateTime.Now;
                _context.SaveChanges();
                TempData["Success"] = "Policy sale deleted successfully!";
            }
            return RedirectToAction("Index");
        }

        // AJAX: Get product price
        [HttpGet]
        public JsonResult GetProductPrice(int productId)
        {
            var product = _context.Products.Find(productId);
            if (product != null)
            {
                return Json(new { success = true, price = product.Price, name = product.Name }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        private void LoadProducts()
        {
            var products = _context.Products
                .Where(p => p.IsActive)
                .Include(p => p.Category)
                .OrderBy(p => p.Name)
                .ToList();
                
            ViewBag.Products = products.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = $"{p.Name} - {p.Price:C}"
            }).ToList();
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
