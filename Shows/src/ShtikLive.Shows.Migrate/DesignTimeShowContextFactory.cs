using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ShtikLive.Shows.Data;

namespace ShtikLive.Shows.Migrate
{
    public class DesignTimeShowContextFactory : IDesignTimeDbContextFactory<ShowContext>
    {
        public const string LocalPostgres = "Host=localhost;Database=shows;Username=shows;Password=secretsquirrel";

        public static readonly string MigrationAssemblyName =
            typeof(DesignTimeShowContextFactory).Assembly.GetName().Name;

        public ShowContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder()
                .UseNpgsql(LocalPostgres, b => b.MigrationsAssembly(MigrationAssemblyName));
            return new ShowContext(builder.Options);
        }
    }
}