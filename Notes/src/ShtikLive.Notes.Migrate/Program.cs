using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShtikLive.Notes.Data;

namespace ShtikLive.Notes.Migrate
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = args.Length == 1
                ? args[0]
                : DesignTimeNoteContextFactory.LocalPostgres;

            var options = new DbContextOptionsBuilder<NoteContext>()
                .UseNpgsql(connectionString, b => b.MigrationsAssembly(DesignTimeNoteContextFactory.MigrationAssemblyName))
                .Options;

            var context = new NoteContext(options);

            var loggerFactory = new LoggerFactory().AddConsole();

            var migrationHelper = new MigrationHelper(loggerFactory);

            Console.WriteLine("Trying migration...");
            migrationHelper.TryMigrate(context).GetAwaiter().GetResult();
            Console.WriteLine("Done.");
        }
    }
}
