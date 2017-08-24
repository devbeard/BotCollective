using System.Web.Mvc;

namespace BirthdayBot.Api.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "BirthdayWeb");
        }
    }
}
