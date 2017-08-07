using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace ShtikLive.Incidents.Data
{
    public class IncidentContext : DbContext
    {
        public IncidentContext([NotNull] DbContextOptions options) : base(options)
        {
            if (options == null)
            {
                throw new System.ArgumentNullException(nameof(options));
            }
        }
    }
}