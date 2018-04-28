using JCTest.Models.Interfaces;
using System.Web.Mvc;

namespace JCTest.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Movies");
            }

            return View();
        }
    }
}