﻿using System;
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
        private readonly ILogger<PresentController> _logger;
        private readonly IApiKeyProvider _apiKeyProvider;

        public PresentController(IApiKeyProvider apiKeyProvider, IShowsClient shows, ILogger<PresentController> logger)
        {
            _apiKeyProvider = apiKeyProvider;
            _shows = shows;
            _logger = logger;
        }

        [HttpPost("{handle}/start")]
        public async Task<IActionResult> Start(string handle, [FromBody] StartShow startShow)
        {
            var apiKey = Request.Headers["API-Key"];
            if (!_apiKeyProvider.CheckBase64(handle, apiKey))
            {
                _logger.LogWarning(EventIds.PresenterInvalidApiKey, "Invalid API key.");
                return Forbid();
            }
            startShow.Presenter = handle;
            var show = await _shows.Start(startShow).ConfigureAwait(false);
            return CreatedAtAction("Show", "Live", new {presenter = startShow.Presenter, slug = startShow.Slug}, show);
        }

        [HttpPut("{handle}/{slug}/{number}")]
        public async Task<IActionResult> ShowSlide(string handle, string slug, int number)
        {
            var ok = await _shows.ShowSlide(handle, slug, number);
            if (!ok) return NotFound();
            return Accepted();
        }
    }
}