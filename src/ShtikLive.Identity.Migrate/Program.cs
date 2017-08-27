using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace ShtikLive.Identity.Migrate
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = args.Length == 1
                ? args[0]
                : DesignTimeApplicationDbContextFactory.LocalPostgres;

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(connectionString, b => b.MigrationsAssembly(typeof(Program).Assembly.GetName().Name))
                .Options;

            var context = new ApplicationDbContext(options);

            var loggerFactory = new LoggerFactory().AddConsole();

            var migrationHelper = new MigrationHelper(loggerFactory);

            Console.WriteLine("Trying migration...");
            migrationHelper.TryMigrate(context).GetAwaiter().GetResult();
            Console.WriteLine("Done.");
        }
    }
}
