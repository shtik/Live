using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ShtikLive.Realtime
{
    public class LiveWebSocketManager
    {
        private readonly ConcurrentDictionary<string, MessageQueueSet> _showSockets = new ConcurrentDictionary<string, MessageQueueSet>(StringComparer.OrdinalIgnoreCase);
        private readonly ILogger _logger;

        public LiveWebSocketManager(ILogger<LiveWebSocketManager> logger)
        {
            _logger = logger;
        }

        public async Task Process(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest) throw new InvalidOperationException();

            var socket = await context.WebSockets.AcceptWebSocketAsync();
            var show = context.Request.Path.ToString().Trim('/');
            var messageQueue = new MessageQueue();
            var queueSet = _showSockets.GetOrAdd(show, _ => new MessageQueueSet());
            queueSet.Add(messageQueue);

            var ct = context.RequestAborted;
            while (!ct.IsCancellationRequested)
            {
                var message = await messageQueue.GetMessage();

                if (socket.State != WebSocketState.Open || ct.IsCancellationRequested)
                {
                    break;
                }

                try
                {
                    await socket.SendAsync(message, WebSocketMessageType.Text, true, ct);
                }
                catch (Exception ex)
                {
                    _logger.LogError(EventIds.SendError, ex, ex.Message);
                    break;
                }
            }

            if (queueSet.Remove(messageQueue) == 0)
            {
                _showSockets.TryRemove(show, out _);
            }

            await CloseSocket(socket, ct);

            TryDispose(socket);
        }

        private void TryDispose(IDisposable socket)
        {
            try
            {
                socket.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError(EventIds.CloseError, ex, ex.Message);
            }
        }

        private async Task CloseSocket(WebSocket socket, CancellationToken ct)
        {
            try
            {
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(EventIds.CloseError, ex, ex.Message);
            }
        }

        public void Send(string show, string message)
        {
            if (_showSockets.TryGetValue(show, out var messageQueueSet))
            {
                messageQueueSet.Send(message);
            }
        }
    }
}