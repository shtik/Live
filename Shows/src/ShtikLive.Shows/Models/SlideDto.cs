using System.ComponentModel.DataAnnotations;
using ShtikLive.Shows.Data;

namespace ShtikLive.Shows.Models
{
    public class SlideDto
    {
        public string Presenter { get; set; }

        public string Slug { get; set; }

        public int Number { get; set; }

        [MaxLength(256)]
        public string Title { get; set; }

        [MaxLength(256)]
        public string Layout { get; set; }

        public string Html { get; set; }

        public static SlideDto FromSlide(string presenter, string slug, Slide slide)
        {
            return new SlideDto
            {
                Presenter = presenter,
                Slug = slug,
                Number = slide.Number,
                Title = slide.Title,
                Layout = slide.Layout,
                Html = slide.Html
            };
        }
    }
}