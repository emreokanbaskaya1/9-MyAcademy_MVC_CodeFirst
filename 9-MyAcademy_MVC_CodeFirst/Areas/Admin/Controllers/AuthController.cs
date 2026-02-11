using _9_MyAcademy_MVC_CodeFirst.Data.Context;
using _9_MyAcademy_MVC_CodeFirst.Data.Entities;
using _9_MyAcademy_MVC_CodeFirst.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace _9_MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context = new AppDbContext();

        [HttpGet]
        public ActionResult Login()
        {
            if (Session["AdminUserId"] != null)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            if (Session["AdminUserId"] != null)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(AdminRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (_context.AdminUsers.Any(u => u.Username == model.Username))
            {
                ModelState.AddModelError("Username", "This username is already taken.");
                return View(model);
            }

            if (_context.AdminUsers.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "This email is already registered.");
                return View(model);
            }

            var user = new AdminUser
            {
                Username = model.Username,
                Email = model.Email,
                FullName = model.FullName,
                PasswordHash = HashPassword(model.Password),
                IsActive = true
            };

            _context.AdminUsers.Add(user);
            _context.SaveChanges();

            TempData["RegisterSuccess"] = "Registration successful! Please login.";
            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AdminLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var passwordHash = HashPassword(model.Password);
            var user = _context.AdminUsers.FirstOrDefault(u =>
                u.Username == model.Username &&
                u.PasswordHash == passwordHash &&
                u.IsActive);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }

            Session["AdminUserId"] = user.Id;
            Session["AdminUsername"] = user.Username;
            Session["AdminFullName"] = user.FullName ?? user.Username;

            return RedirectToAction("Index", "Dashboard");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var sb = new StringBuilder();
                foreach (var b in bytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
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
