using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShtikLive.Web.Models;

namespace ShtikLive.Controllers
{
    [Route("presenter")]
    public class PresenterController : Controller
    {
        [HttpPost("start")]
        public Task<IActionResult> Start([FromBody] Show show)
        {
            throw new NotImplementedException();
        }
    }
}