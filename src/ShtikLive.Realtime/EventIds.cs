using Microsoft.Extensions.Logging;

namespace ShtikLive.Realtime
{
    internal static class EventIds
    {
        public static readonly EventId SendMessage = 1001;

        public static readonly EventId SendError = 5001;
        public static readonly EventId CloseError = 5002;
        public static readonly EventId DisposeError = 5003;
    }
}