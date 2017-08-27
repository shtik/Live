using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ShtikLive.Realtime
{
    public class LiveWebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly LiveWebSocketManager _sockets;

        public LiveWebSocketMiddleware(RequestDelegate next, LiveWebSocketManager sockets)
        {
            _next = next;
            _sockets = sockets;
        }

        public Task Invoke(HttpContext context)
        {
            return !context.WebSockets.IsWebSocketRequest
                ? _next.Invoke(context)
                : _sockets.Process(context);
        }
    }
}
