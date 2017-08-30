using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShtikLive.Clients;
using ShtikLive.Identity;
using ShtikLive.Models.Notes;

namespace ShtikLive.Controllers
{
    [Route("api/notes")]
    [Authorize]
    public class NotesController : Controller
    {
        private readonly INotesClient _notes;
        private readonly ILogger<NotesController> _logger;
        public NotesController(INotesClient notes, ILogger<NotesController> logger)
        {
            _notes = notes;
            _logger = logger;
        }

        [HttpGet("{presenter}/{slug}/{slideNumber}")]
        public async Task<IActionResult> Get(string presenter, string slug, int slideNumber, CancellationToken ct)
        {
            var user = User.FindFirst(ShtikClaimTypes.Handle)?.Value;
            if (string.IsNullOrWhiteSpace(user)) return BadRequest();
            var note = await _notes.GetNotes(user, presenter, slug, slideNumber, ct).ConfigureAwait(false);
            return Ok(note ?? new Note());
        }

        [HttpPut("{presenter}/{slug}/{slideNumber}")]
        public async Task<IActionResult> Put(string presenter, string slug, int slideNumber, [FromBody] Note note, CancellationToken ct)
        {
            var user = User.FindFirst(ShtikClaimTypes.Handle)?.Value;
            if (string.IsNullOrWhiteSpace(user)) return BadRequest();
            await _notes.SaveNotes(user, presenter, slug, slideNumber, note, ct).ConfigureAwait(false);
            return Ok();
        }
    }
}