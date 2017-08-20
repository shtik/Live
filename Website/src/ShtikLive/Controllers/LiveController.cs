using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShtikLive.Clients;

namespace ShtikLive.Controllers
{
    [Route("live")]
    public class LiveController : Controller
    {
        private readonly IShowsClient _shows;
        private readonly ILogger _logger;

        public LiveController(IShowsClient shows, ILogger<LiveController> logger)
        {
            _shows = shows;
            _logger = logger;
        }

        [HttpGet("{presenter}")]
        public async Task<IActionResult> Presenter(string presenter)
        {
            var show = await _shows.GetLatest(presenter);
            if (show == null)
            {
                _logger.LogWarning(EventIds.PresenterShowNotFound, "Show not found for presenter '{presenter}'", presenter);
                return NotFound();
            }
            return RedirectToAction("Show", new {presenter, slug = show.Slug});
        }

        // GET
        [HttpGet("{presenter}/{slug}")]
        public IActionResult Show(string presenter, string slug)
        {
            throw new NotImplementedException();
        }
    }
}