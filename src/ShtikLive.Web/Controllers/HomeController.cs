using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ShtikLive.Web.Models;

namespace ShtikLive.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
