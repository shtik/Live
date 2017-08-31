using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Semantics;
using Microsoft.Extensions.Logging;
using ShtikLive.Clients;
using ShtikLive.Identity;
using ShtikLive.Models.Present;

namespace ShtikLive.Controllers
{
    [Route("present")]
    public class PresentController : Controller
    {
        private readonly IShowsClient _shows;
        private readonly IQuestionsClient _questions;
        private readonly ILogger<PresentController> _logger;
        private readonly IApiKeyProvider _apiKeyProvider;

        public PresentController(IApiKeyProvider apiKeyProvider, IShowsClient shows, ILogger<PresentController> logger, IQuestionsClient questions)
        {
            _apiKeyProvider = apiKeyProvider;
            _shows = shows;
            _logger = logger;
            _questions = questions;
        }

        [HttpGet("{handle}/{slug}")]
        public async Task<IActionResult> PresenterView(string handle, string slug, CancellationToken ct)
        {
            var user = User.FindFirst(ShtikClaimTypes.Handle)?.Value;
            if (!handle.Equals(user, StringComparison.OrdinalIgnoreCase))
            {
                return NotFound();
            }
            var (show, questions) = await MultiTask.WhenAll(_shows.Get(user, slug, ct), _questions.GetQuestionsForShow(user, slug, ct));
            if (show == null)
            {
                return NotFound();
            }
            var viewModel = new PresenterViewModel
            {
                Show = show,
                Questions = questions.OrderByDescending(q => q.Time).ToList()
            };
            return View(viewModel);
        }

        [HttpPost("{handle}/start")]
        public async Task<IActionResult> Start(string handle, [FromBody] StartShow startShow, CancellationToken ct)
        {
            var apiKey = Request.Headers["API-Key"];
            if (!_apiKeyProvider.CheckBase64(handle, apiKey))
            {
                _logger.LogWarning(EventIds.PresenterInvalidApiKey, "Invalid API key.");
                return Forbid();
            }
            startShow.Presenter = handle;
            var show = await _shows.Start(startShow, ct).ConfigureAwait(false);
            return CreatedAtAction("Show", "Live", new {presenter = startShow.Presenter, slug = startShow.Slug}, show);
        }

        [HttpPut("{handle}/{slug}/{number}")]
        public async Task<IActionResult> ShowSlide(string handle, string slug, int number, CancellationToken ct)
        {
            var ok = await _shows.ShowSlide(handle, slug, number, ct);
            if (!ok) return NotFound();
            return Accepted();
        }
    }
}