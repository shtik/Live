using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace ShtikLive.Realtime
{
    public class RedisSubscriber
    {
        private readonly ConnectionMultiplexer _redis;
        private ISubscriber _subscriber;
        private LiveWebSocketManager _socketManager;

        public RedisSubscriber(ConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public void Start(LiveWebSocketManager socketManager, CancellationToken ct = default (CancellationToken))
        {
            _socketManager = socketManager;
            _subscriber = _redis.GetSubscriber();
            _subscriber.Subscribe("shtik:messaging", HandleMessage);
            ct.Register(Cancel);
        }

        private void HandleMessage(RedisChannel channel, RedisValue value)
        {
            var message = JsonConvert.DeserializeObject<Message>(value);
            var show = $"{message.Presenter}/{message.Slug}";
            _socketManager.Send(show, value);
        }

        private void Cancel()
        {
            _subscriber.UnsubscribeAll();
        }
    }
}