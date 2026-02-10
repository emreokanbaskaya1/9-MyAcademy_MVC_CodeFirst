using _9_MyAcademy_MVC_CodeFirst.Data.Context;
using _9_MyAcademy_MVC_CodeFirst.Data.Entities;
using System.Linq;
using System.Web.Mvc;

namespace _9_MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    public class TeamMemberController : Controller
    {
        private readonly AppDbContext _context = new AppDbContext();

        // GET: Admin/TeamMember
        public ActionResult Index()
        {
            var teamMembers = _context.TeamMembers.Where(x => x.IsActive).ToList();
            return View(teamMembers);
        }

        // GET: Admin/TeamMember/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/TeamMember/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TeamMember teamMember)
        {
            if (ModelState.IsValid)
            {
                teamMember.IsActive = true;
                _context.TeamMembers.Add(teamMember);
                _context.SaveChanges();
                TempData["Success"] = "Team member created successfully!";
                return RedirectToAction("Index");
            }
            return View(teamMember);
        }

        // GET: Admin/TeamMember/Edit/5
        public ActionResult Edit(int id)
        {
            var teamMember = _context.TeamMembers.Find(id);
            if (teamMember == null)
            {
                return HttpNotFound();
            }
            return View(teamMember);
        }

        // POST: Admin/TeamMember/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TeamMember teamMember)
        {
            if (ModelState.IsValid)
            {
                var existing = _context.TeamMembers.Find(teamMember.Id);
                if (existing != null)
                {
                    existing.Name = teamMember.Name;
                    existing.Position = teamMember.Position;
                    existing.ImageUrl = teamMember.ImageUrl;
                    existing.FacebookUrl = teamMember.FacebookUrl;
                    existing.TwitterUrl = teamMember.TwitterUrl;
                    existing.LinkedInUrl = teamMember.LinkedInUrl;
                    existing.InstagramUrl = teamMember.InstagramUrl;
                    existing.IsActive = teamMember.IsActive;
                    _context.SaveChanges();
                    TempData["Success"] = "Team member updated successfully!";
                    return RedirectToAction("Index");
                }
            }
            return View(teamMember);
        }

        // GET: Admin/TeamMember/Delete/5
        public ActionResult Delete(int id)
        {
            var teamMember = _context.TeamMembers.Find(id);
            if (teamMember != null)
            {
                teamMember.IsActive = false; // Soft delete
                _context.SaveChanges();
                TempData["Success"] = "Team member deleted successfully!";
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
