using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShtikLive.Shows.Data;

namespace ShtikLive.Shows.Controllers
{
    using static ResultMethods;

    [Route("shows")]
    public class ShowsController
    {
        private readonly ShowContext _context;

        public ShowsController(ShowContext context)
        {
            _context = context;
        }

        [HttpPost("start")]
        public async Task<IActionResult> Start([FromBody] Show show)
        {
            _context.Shows.Add(show);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return CreatedAtAction("Get", "Shows", new {id = show.Id}, show);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var show = await _context.Shows.SingleOrDefaultAsync(s => s.Id == id).ConfigureAwait(false);
            return show == null ? NotFound() : Ok(show);
        }

        [HttpGet("by/{handle}")]
        public async Task<IActionResult> ListByPresenter(string handle)
        {
            var shows = await _context.Shows
                .Where(s => s.Presenter == handle)
                .OrderByDescending(s => s.Time)
                .ToListAsync()
                .ConfigureAwait(false);
            return Ok(shows);
        }

        [HttpGet("by/{handle}/latest")]
        public async Task<IActionResult> LatestByPresenter(string handle)
        {
            var show = await _context.Shows
                .Where(s => s.Presenter == handle)
                .OrderByDescending(s => s.Time)
                .Take(1)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);
            return show == null ? NotFound() : Ok(show);
        }
    }
}
