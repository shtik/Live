using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShtikLive.Clients;
using ShtikLive.Models.Live;

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
            return RedirectToAction("Show", new {presenter, slug = show.Slug, });
        }

        // GET
        [HttpGet("{presenter}/{slug}")]
        public IActionResult Show(string presenter, string slug)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{presenter}/{slug}/{number}")]
        public async Task<IActionResult> ShowSlide(string presenter, string slug, int number)
        {
            var (show, slide) =
                await MultiTask.Wait(_shows.Get(presenter, slug), _shows.GetSlide(presenter, slug, number));
            if (show == null || slide == null) return NotFound();
            var viewModel = new ShowSlideViewModel
            {
                Presenter = presenter,
                Slug = slug,
                Title = show.Title,
                Time = show.Time,
                Place = show.Place,
                Slide = slide
            };
            return View(viewModel);
        }
    }
}