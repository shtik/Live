using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShtikLive.Clients;
using ShtikLive.Identity;
using ShtikLive.Models.Notes;
using ShtikLive.Models.Questions;

namespace ShtikLive.Controllers
{
    [Route("api/questions")]
    [Authorize]
    public class QuestionsController : Controller
    {
        private readonly IQuestionsClient _questions;
        private readonly ILogger<QuestionsController> _logger;
        public QuestionsController(IQuestionsClient questions, ILogger<QuestionsController> logger)
        {
            _questions = questions;
            _logger = logger;
        }

        [HttpGet("{presenter}/{slug}")]
        public async Task<IActionResult> Get(string presenter, string slug, CancellationToken ct)
        {
            var questions = await _questions.GetQuestionsForShow(presenter, slug, ct).ConfigureAwait(false);
            return Ok(questions ?? new List<Question>());
        }

        [HttpGet("{presenter}/{slug}/{slideNumber}")]
        public async Task<IActionResult> Get(string presenter, string slug, int slideNumber, CancellationToken ct)
        {
            var questions = await _questions.GetQuestionsForSlide(presenter, slug, slideNumber, ct).ConfigureAwait(false);
            return Ok(questions ?? new List<Question>());
        }

        [HttpPost("{presenter}/{slug}/{slideNumber}")]
        public async Task<IActionResult> Ask(string presenter, string slug, int slideNumber, [FromBody] Question question, CancellationToken ct)
        {
            var user = User.FindFirst(ShtikClaimTypes.Handle)?.Value;
            if (string.IsNullOrWhiteSpace(user))
            {
                _logger.LogWarning(EventIds.AnonymousAccess, "Anonymous access for {presenter}/{slug}/{slide}", presenter, slug, slideNumber);
                return BadRequest();
            }
            question.User = user;
            question.Time = DateTimeOffset.UtcNow;
            await _questions.AskQuestion(presenter, slug, slideNumber, question, ct).ConfigureAwait(false);
            return Ok();
        }
    }
}