using System.ComponentModel.DataAnnotations;

namespace ShtikLive.Shows.Data
{
    public class Slide
    {
        public int Id { get; set; }
        public int ShowId { get; set; }
        public int Index { get; set; }

        [MaxLength(256)]
        public string Title { get; set; }

        [MaxLength(256)]
        public string Layout { get; set; }
        public string Html { get; set; }
    }
}