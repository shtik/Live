using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ShtikLive.Identity.Migrate
{
    public class DesignTimeApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public const string LocalPostgres = "Host=localhost;Database=aspnet;Username=shtik;Password=secretsquirrel";

        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(LocalPostgres, b => b.MigrationsAssembly("ShtikLive.Identity.Migrate"));
            return new ApplicationDbContext(builder.Options);
        }
    }
}