using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShtikLive.Realtime;
using StackExchange.Redis;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class LiveWebSocketServiceExtensions
    {
        public static IServiceCollection AddLiveWebSockets(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton(ConnectionMultiplexer.Connect(config.GetValue("Redis:Host", "localhost")));
            services.AddSingleton<LiveWebSocketManager>();
            services.AddSingleton<RedisSubscriber>();
            return services;
        }
    }
}