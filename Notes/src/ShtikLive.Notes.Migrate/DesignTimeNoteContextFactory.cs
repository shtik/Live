using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ShtikLive.Notes.Data;

namespace ShtikLive.Notes.Migrate
{
    public class DesignTimeNoteContextFactory : IDesignTimeDbContextFactory<NoteContext>
    {
        public const string LocalPostgres = "Host=localhost;Database=notes;Username=notes;Password=secretsquirrel";

        public static readonly string MigrationAssemblyName =
            typeof(DesignTimeNoteContextFactory).Assembly.GetName().Name;

        public NoteContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder()
                .UseNpgsql(LocalPostgres, b => b.MigrationsAssembly(MigrationAssemblyName));
            return new NoteContext(builder.Options);
        }
    }
}