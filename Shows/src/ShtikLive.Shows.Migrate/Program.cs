using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShtikLive.Shows.Data;

namespace ShtikLive.Shows.Migrate
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = args.Length == 1
                ? args[0]
                : DesignTimeShowContextFactory.LocalPostgres;

            var options = new DbContextOptionsBuilder<ShowContext>()
                .UseNpgsql(connectionString, b => b.MigrationsAssembly(DesignTimeShowContextFactory.MigrationAssemblyName))
                .Options;

            var context = new ShowContext(options);

            var loggerFactory = new LoggerFactory().AddConsole();

            var migrationHelper = new MigrationHelper(loggerFactory);

            Console.WriteLine("Trying migration...");
            migrationHelper.TryMigrate(context).GetAwaiter().GetResult();
            Console.WriteLine("Done.");
        }
    }
}
