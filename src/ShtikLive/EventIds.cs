using Microsoft.Extensions.Logging;

namespace ShtikLive
{
    public static class EventIds
    {
        private const int Base = 1000;

        public static readonly EventId PresenterShowNotFound = Base + 1;
        public static readonly EventId PresenterInvalidApiKey = Base + 2;
    }
}