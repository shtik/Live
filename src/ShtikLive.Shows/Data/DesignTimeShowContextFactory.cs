using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ShtikLive.Shows.Data
{
    public class DesignTimeShowContextFactory : IDesignTimeDbContextFactory<ShowContext>
    {
        private const string LocalPostgres = "Host=localhost;Database=shows;Username=shows;Password=secretsquirrel";

        public ShowContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder()
                .UseNpgsql(LocalPostgres);
            return new ShowContext(builder.Options);
        }
    }
}