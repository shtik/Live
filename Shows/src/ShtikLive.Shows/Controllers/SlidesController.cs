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

        [HttpGet("{presenter}/{slug}/{number}")]
        public async Task<IActionResult> GetShowSlideNumber(string presenter, string slug, int number)
        {
            var slide = await _context.Slides
                .SingleOrDefaultAsync(s => s.Show.Presenter == presenter && s.Show.Slug == slug && s.Number == number)
                .ConfigureAwait(false);
            return slide == null ? NotFound() : Ok(slide);
        }

        [HttpPut("show/{presenter}/{slug}/{number}")]
        public async Task<IActionResult> ShowSlideNumber(string presenter, string slug, int number)
        {
            var slide = await _context.Slides
                .SingleOrDefaultAsync(s => s.Show.Presenter == presenter && s.Show.Slug == slug && s.Number == number)
                .ConfigureAwait(false);

            if (slide == null) return NotFound();

            if (!slide.HasBeenShown)
            {
                slide.HasBeenShown = true;
                await _context.SaveChangesAsync();
            }

            return Accepted();
        }
    }
}