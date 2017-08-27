using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShtikLive.Realtime
{
    public class MessageQueue
    {
        private readonly Queue<ArraySegment<byte>> _messages = new Queue<ArraySegment<byte>>();
        private TaskCompletionSource<ArraySegment<byte>> _tcs;

        public void AddMessage(ArraySegment<byte> message)
        {
            if (_tcs != null)
            {
                _tcs.SetResult(message);
                _tcs = new TaskCompletionSource<ArraySegment<byte>>();
            }
            else
            {
                _messages.Enqueue(message);
            }
        }

        public ValueTask<ArraySegment<byte>> GetMessage()
        {
            if (_messages.TryDequeue(out var message))
            {
                return new ValueTask<ArraySegment<byte>>(message);
            }
            _tcs = new TaskCompletionSource<ArraySegment<byte>>();
            return new ValueTask<ArraySegment<byte>>(_tcs.Task);
        }
    }
}