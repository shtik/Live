using Microsoft.AspNetCore.Mvc;

namespace ShtikLive.Web.Controllers
{
    public class LiveController : Controller
    {
        // GET
        [HttpGet("{session}")]
        public IActionResult Index()
        {
            return View();
        }
    }
}