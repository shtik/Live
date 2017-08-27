using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShtikLive.Realtime
{
    public class LiveWebSocketManager
    {
        private readonly ConcurrentDictionary<string, List<WebSocket>> _showSockets = new ConcurrentDictionary<string, List<WebSocket>>(StringComparer.OrdinalIgnoreCase);

        public void Add(string show, WebSocket socket)
        {
            var list = _showSockets.GetOrAdd(show, _ => new List<WebSocket>());
            lock (((ICollection)list).SyncRoot)
            {
                if (!list.Contains(socket))
                {
                    list.Add(socket);
                }
            }
        }

        public bool TryGet(string show, out IEnumerable<WebSocket> sockets)
        {
            if (_showSockets.TryGetValue(show, out var list))
            {
                sockets = list.AsEnumerable();
                return true;
            }
            sockets = Enumerable.Empty<WebSocket>();
            return false;
        }

        public async Task SendAsync(string show, string data, CancellationToken ct = default(CancellationToken))
        {
            if (_showSockets.TryGetValue(show, out var list))
            {
                var buffer = Encoding.UTF8.GetBytes(data);
                var segment = new ArraySegment<byte>(buffer);
                await Task.WhenAll(list.Select(ws => ws.SendAsync(segment, WebSocketMessageType.Text, true, ct)));
            }
        }
    }
}