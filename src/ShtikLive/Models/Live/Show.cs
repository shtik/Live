using System;

namespace ShtikLive.Web.Models
{
    public class Show
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public DateTimeOffset Time { get; set; }
        public string Place { get; set; }
    }
}