using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShtikLive.Shows.Data
{
    public class Show
    {
        public int Id { get; set; }

        [MaxLength(16)]
        public string Presenter { get; set; }

        [MaxLength(256)]
        public string Slug { get; set; }

        [MaxLength(256)]
        public string Title { get; set; }

        public DateTimeOffset Time { get; set; }

        [MaxLength(256)]
        public string Place { get; set; }

        public List<Slide> Slides { get; set; }
    }
}