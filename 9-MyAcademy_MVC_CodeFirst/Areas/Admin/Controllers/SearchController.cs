using _9_MyAcademy_MVC_CodeFirst.Services;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace _9_MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    public class SearchController : Controller
    {
        private readonly TavilyService _tavilyService = new TavilyService();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                TempData["Error"] = "Please enter a search query.";
                return RedirectToAction("Index");
            }

            var result = await _tavilyService.SearchWeb(query);
            ViewBag.Query = query;
            ViewBag.Result = result;

            return View("Index");
        }
    }
}
