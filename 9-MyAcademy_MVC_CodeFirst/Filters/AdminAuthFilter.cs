using System.Web;
using System.Web.Mvc;

namespace _9_MyAcademy_MVC_CodeFirst.Filters
{
    public class AdminAuthFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;
            if (session == null || session["AdminUserId"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary
                    {
                        { "controller", "Auth" },
                        { "action", "Login" },
                        { "area", "Admin" }
                    });
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
