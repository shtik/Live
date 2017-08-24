using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace ShtikLive.Shows.Data
{
    public class ShowContext : DbContext
    {
        public ShowContext([NotNull] DbContextOptions options) : base(options)
        {
            if (options == null)
            {
                throw new System.ArgumentNullException(nameof(options));
            }
        }

        public DbSet<Show> Shows { get; set; }
        public DbSet<Slide> Slides { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Show>()
                .HasIndex(s => new { s.Presenter, s.Slug })
                .IsUnique();
        }
    }
}