using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShtikLive.Clients;
using ShtikLive.Identity;
using ShtikLive.Models.Live;

namespace ShtikLive.Controllers
{
    [Route("present")]
    public class PresentController : Controller
    {
        private readonly IShowsClient _shows;
        private readonly ILogger<PresentController> _logger;
        private readonly IApiKeyProvider _apiKeyProvider;

        public PresentController(IApiKeyProvider apiKeyProvider, IShowsClient shows, ILogger<PresentController> logger)
        {
            _apiKeyProvider = apiKeyProvider;
            _shows = shows;
            _logger = logger;
        }

        [HttpPost("{handle}/start")]
        public async Task<IActionResult> Start(string handle, [FromBody] Show show)
        {
            var apiKey = Request.Headers["API-Key"];
            if (!_apiKeyProvider.CheckBase64(handle, apiKey))
            {
                _logger.LogWarning(EventIds.PresenterInvalidApiKey, "Invalid API key.");
                return Forbid();
            }
            show.Presenter = handle;
            show = await _shows.Start(show).ConfigureAwait(false);
            return CreatedAtAction("ShowSlide", new {id = show.Id}, show);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ShowSlide(int id, [FromBody]ShowingSlide showingSlide)
        {
            var ok = await _shows.ShowingSlide(id, showingSlide.Number);
            if (!ok) return NotFound();
            return Accepted();
        }
    }
}