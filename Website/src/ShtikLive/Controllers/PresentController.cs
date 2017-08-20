using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShtikLive.Identity;
using ShtikLive.Models.Live;

namespace ShtikLive.Controllers
{
    [Route("present")]
    public class PresentController : Controller
    {
        private readonly IApiKeyProvider _apiKeyProvider;

        public PresentController(IApiKeyProvider apiKeyProvider)
        {
            _apiKeyProvider = apiKeyProvider;
        }

        [HttpPost("{handle}/start")]
        public async Task<IActionResult> Start(string handle, [FromBody] Show show)
        {
            var apiKey = Request.Headers["API-Key"];
            if (!_apiKeyProvider.CheckBase64(handle, apiKey))
            {
                return Forbid();
            }
            throw new NotImplementedException();
        }
    }
}