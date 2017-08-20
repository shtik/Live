using Microsoft.Extensions.Logging;

namespace ShtikLive.Notes.Migrate
{
    public static class EventIds
    {
        public static readonly EventId MigrationTestConnectFailed = 1001;
        public static readonly EventId MigrationFailed = 1002;
    }
}