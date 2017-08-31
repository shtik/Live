using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ShtikLive.Realtime;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
    public static class LiveWebSocketBuilderExtensions
    {
        public static IApplicationBuilder UseLiveWebSockets(this IApplicationBuilder builder)
        {
            var manager = builder.ApplicationServices.GetRequiredService<LiveWebSocketManager>();
            var subscriber = builder.ApplicationServices.GetRequiredService<RedisSubscriber>();
            subscriber.Start(manager);

            builder.UseMiddleware<LiveWebSocketMiddleware>();
            return builder;
        }
    }
}