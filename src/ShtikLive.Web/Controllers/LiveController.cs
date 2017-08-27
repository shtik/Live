using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShtikLive.Web.Models;

namespace ShtikLive.Web.Controllers
{
    public class LiveController : Controller
    {
        [HttpGet("{presenter}")]
        public async Task<IActionResult> Presenter(string presenter)
        {
            
        }

        // GET
        [HttpGet("{session}")]
        public IActionResult Index()
        {
            return View();
        }
    }

    [Route("presenter")]
    public class PresenterController : Controller
    {
        [HttpPost("start")]
        public async Task<IActionResult> Start([FromBody] Show show)
        {
            
        }
    }
}