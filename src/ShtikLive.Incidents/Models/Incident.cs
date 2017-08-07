using System;

namespace ShtikLive.Incidents.Models
{
    public class Incident
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public DateTimeOffset StartTime { get; set; }
    }
}