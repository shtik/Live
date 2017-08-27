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

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                await _next.Invoke(context);
                return;
            }

            var newSocket = await context.WebSockets.AcceptWebSocketAsync();
            _sockets.Add(context.Request.Path, newSocket);
        }
    }
}
