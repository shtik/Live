using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using ShtikLive.Shows.Models;

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
    }
}