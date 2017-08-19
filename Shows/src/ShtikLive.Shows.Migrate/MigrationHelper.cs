using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using Polly;
using ShtikLive.Shows.Data;

namespace ShtikLive.Shows.Migrate
{
    public class MigrationHelper
    {
        private readonly ILogger<MigrationHelper> _logger;

        public MigrationHelper(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<MigrationHelper>();
        }

        public async Task TryMigrate(DbContextOptions<ShowContext> options)
        {
            await TryMigrate(new ShowContext(options));
        }

        public async Task TryMigrate(ShowContext context)
        {
            using (context)
            {
                await TryConnect(context);

                await TryRunMigration(context);
            }
        }

        private async Task TryRunMigration(ShowContext context)
        {
            try
            {
                await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                // Ignored
                LoggerExtensions.LogError(_logger, EventIds.MigrationFailed, ex, "Migration failed to run: {message}", ex.Message);
            }
        }

        private async Task TryConnect(ShowContext context)
        {
            try
            {
                await Policy
                    .Handle<NpgsqlException>((ex) =>
                    {
                        LoggerExtensions.LogWarning(_logger, EventIds.MigrationTestConnectFailed, ex, "TryMigrate test connect failed, retrying.");
                        return true;
                    })
                    .WaitAndRetryAsync(5, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)))
                    .ExecuteAsync(async () =>
                    {
                        await context.Database.OpenConnectionAsync();
                        context.Database.CloseConnection();
                    });
            }
            catch (Exception ex)
            {
                LoggerExtensions.LogError(_logger, EventIds.MigrationTestConnectFailed, ex, "TryMigrate could not connect to database.");
                throw;
            }
        }
    }
}