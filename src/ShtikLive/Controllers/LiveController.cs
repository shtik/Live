using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ShtikLive.Controllers
{
    [Route("live")]
    public class LiveController : Controller
    {
        [HttpGet("{presenter}")]
        public Task<IActionResult> Presenter(string presenter)
        {
            throw new NotImplementedException();
        }

        // GET
        [HttpGet("{presenter}/{session}")]
        public IActionResult Index()
        {
            throw new NotImplementedException();
        }
    }
}