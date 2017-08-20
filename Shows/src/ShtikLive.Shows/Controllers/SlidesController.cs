using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShtikLive.Shows.Data;

namespace ShtikLive.Shows.Controllers
{
    using static ResultMethods;

    [Route("slides")]
    public class SlidesController
    {
        private readonly ShowContext _context;

        public SlidesController(ShowContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var slide = await _context.Slides
                .SingleOrDefaultAsync(s => s.Id == id)
                .ConfigureAwait(false);
            return slide == null ? NotFound() : Ok(slide);
        }

        [HttpGet("show/{showId}/{number}")]
        public async Task<IActionResult> GetShowSlideNumber(int showId, int number)
        {
            var slide = await _context.Slides
                .SingleOrDefaultAsync(s => s.ShowId == showId && s.Number == number)
                .ConfigureAwait(false);
            return slide == null ? NotFound() : Ok(slide);
        }

        [HttpPut("show/{showId}/{number}")]
        public async Task<IActionResult> ShowSlideNumber(int showId, int number)
        {
            var slide = await _context.Slides
                .SingleOrDefaultAsync(s => s.ShowId == showId && s.Number == number)
                .ConfigureAwait(false);

            if (slide == null) return NotFound();

            if (!slide.Shown)
            {
                slide.Shown = true;
                await _context.SaveChangesAsync();
            }

            return Accepted();
        }
    }
}