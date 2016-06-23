using System.Web.Mvc;

namespace RaspPi3.WebApi.Controllers
{
    [Authorize]
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
