using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

namespace ShtikLive.Realtime
{
    internal class MessageQueueSet
    {
        private ImmutableHashSet<MessageQueue> _messageQueues = ImmutableHashSet<MessageQueue>.Empty;

        public void Add(MessageQueue messageQueue)
        {
            ImmutableInterlocked.Update(ref _messageQueues, set => set.Add(messageQueue));
        }

        public int Remove(MessageQueue messageQueue)
        {
            ImmutableInterlocked.Update(ref _messageQueues, q => q.Remove(messageQueue));
            return _messageQueues.Count;
        }

        public void Send(string message)
        {
            var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
            foreach (var queue in _messageQueues.AsEnumerable())
            {
                queue.AddMessage(buffer);
            }
        }
    }
}