using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public async Task<IActionResult> Presenter(string presenter, CancellationToken ct)
        {
            var show = await _shows.GetLatest(presenter, ct).ConfigureAwait(false);
            if (show == null)
            {
                _logger.LogWarning(EventIds.PresenterShowNotFound, "Show not found for presenter '{presenter}'", presenter);
                return NotFound();
            }
            return RedirectToAction("Show", new { presenter, slug = show.Slug, });
        }

        // GET
        [HttpGet("{presenter}/{slug}")]
        public async Task<IActionResult> Show(string presenter, string slug, CancellationToken ct)
        {
            var show = await _shows.Get(presenter, slug, ct);
            if (show == null)
            {
                _logger.LogWarning(EventIds.PresenterShowNotFound, "Show '{slug}' not found for presenter '{presenter}'", slug, presenter);
                return NotFound();
            }
            return RedirectToAction("ShowSlide", new { presenter, slug, number = show.HighestSlideShown });
        }

        [HttpGet("{presenter}/{slug}/{number:int}")]
        public async Task<IActionResult> ShowSlide(string presenter, string slug, int number)
        {
            var show = await _shows.Get(presenter, slug);
            if (show == null) return NotFound();
            if (show.HighestSlideShown < number)
                return RedirectToAction("ShowSlide", new {presenter, slug, number = show.HighestSlideShown});

            var viewModel = new ShowSlideViewModel
            {
                Presenter = presenter,
                Slug = slug,
                Title = show.Title,
                Time = show.Time,
                Place = show.Place,
                SlideNumber = number
            };
            return View(viewModel);
        }

        [HttpGet("{presenter}/{slug}/{number:int}/partial")]
        public async Task<IActionResult> GetSlidePartial(string presenter, string slug, int number)
        {
            var show = await _shows.Get(presenter, slug);
            if (show == null || show.HighestSlideShown < number) return NotFound();

            var slidePartial = new SlidePartial
            {
                SlideImageUrl = $"/slides/{presenter}/{slug}/{number}"
            };

            return Ok(slidePartial);
        }

        private string ProcessSlideHtml(string html)
        {
            var shtikUrl = Regex.Replace(Request.GetDisplayUrl(), @"\/[0-9]+$", "");
            return html.Replace("{{shtik}}", shtikUrl);
        }
    }
}