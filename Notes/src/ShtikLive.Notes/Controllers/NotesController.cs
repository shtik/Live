using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShtikLive.Notes.Data;

namespace ShtikLive.Notes.Controllers
{
    using static ResultMethods;

    [Route("notes")]
    public class NotesController
    {
        private readonly NoteContext _context;

        public NotesController(NoteContext context)
        {
            _context = context;
        }

        [HttpGet("{userHandle}/{showId}/{slideNumber}")]
        public async Task<IActionResult> GetForSlide(string userHandle, int showId, int slideNumber)
        {
            var existingNote = await _context.Notes
                .SingleOrDefaultAsync(n =>
                    n.ShowId == showId && n.SlideNumber == slideNumber && n.UserHandle == userHandle)
                .ConfigureAwait(false);
            return existingNote == null ? NotFound() : Ok(existingNote);
        }

        [HttpGet("{userHandle}/{showId}")]
        public async Task<IActionResult> GetForShow(string userHandle, int showId)
        {
            var existingNotes = await _context.Notes
                .Where(n =>
                    n.ShowId == showId && n.UserHandle == userHandle)
                .OrderBy(n => n.SlideNumber)
                .ToListAsync()
                .ConfigureAwait(false);
            return Ok(existingNotes);
        }

        [HttpPut("{userHandle}/{showId}/{slideNumber}")]
        public async Task<IActionResult> Write(string userHandle, int showId, int slideNumber, [FromBody] Note note)
        {
            var existingNote = await _context.Notes
                .SingleOrDefaultAsync(n =>
                    n.ShowId == showId && n.SlideNumber == slideNumber && n.UserHandle == userHandle)
                .ConfigureAwait(false);
            if (existingNote == null)
            {
                note.ShowId = showId;
                note.SlideNumber = slideNumber;
                note.UserHandle = userHandle;
                note.Timestamp = DateTimeOffset.UtcNow;
                _context.Notes.Add(note);
            }
            else
            {
                existingNote.NoteText = note.NoteText;
                existingNote.Timestamp = DateTimeOffset.UtcNow;
            }
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return Accepted();
        }
    }
}
