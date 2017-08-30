using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
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
            var latestSlide = await _shows.GetLatestSlide(presenter, slug, ct).ConfigureAwait(false);
            if (latestSlide == null)
            {
                _logger.LogWarning(EventIds.PresenterShowNotFound, "Latest slide not found for show '{presenter}/{slug}'", presenter, slug);
                return NotFound();
            }
            return RedirectToAction("ShowSlide", new { presenter, slug, number = latestSlide.Number });
        }

        [HttpGet("{presenter}/{slug}/{number}")]
        public async Task<IActionResult> ShowSlide(string presenter, string slug, int number)
        {
            var (show, slide) =
                await MultiTask.Wait(_shows.Get(presenter, slug), _shows.GetSlide(presenter, slug, number));
            if (show == null || (slide == null || !slide.HasBeenShown)) return NotFound();

            slide.Html = ProcessSlideHtml(slide.Html);

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

        private string ProcessSlideHtml(string html)
        {
            var shtikUrl = Regex.Replace(Request.GetDisplayUrl(), @"\/[0-9]+$", "");
            return html.Replace("{{shtik}}", shtikUrl);
        }
    }
}