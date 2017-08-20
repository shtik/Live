using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace ShtikLive.Notes.Data
{
    public class NoteContext : DbContext
    {
        public NoteContext([NotNull] DbContextOptions options) : base(options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
        }

        public DbSet<Note> Notes { get; set; }
    }
}
