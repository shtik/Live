using System;
using System.Collections.Generic;

namespace ShtikLive.Models.Live
{
    public class Show
    {
        public int Id { get; set; }
        public string Presenter { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public DateTimeOffset Time { get; set; }
        public string Place { get; set; }
        public List<Slide> Slides { get; set; }
    }
}