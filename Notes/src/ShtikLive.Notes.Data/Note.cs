using System;
using System.ComponentModel.DataAnnotations;

namespace ShtikLive.Notes.Data
{
    public class Note
    {
        public int Id { get; set; }

        public int ShowId { get; set; }

        public int SlideNumber { get; set; }

        [MaxLength(16)]
        public string UserHandle { get; set; }

        public string NoteText { get; set; }

        public bool Public { get; set; }

        public DateTimeOffset Timestamp { get; set; }
    }
}