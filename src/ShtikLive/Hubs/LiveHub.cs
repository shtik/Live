using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using ShtikLive.Hubs.Models;
using ShtikLive.Models.Live;

namespace ShtikLive.Hubs
{
    public class LiveHub : Hub
    {
        public async Task Join(string groupName)
        {
            await Groups.AddAsync(Context.ConnectionId, groupName);
        }

        public async Task Leave(string groupName)
        {
            await Groups.RemoveAsync(Context.ConnectionId, groupName);
        }

        public async Task Send(string groupName, LiveMessage message)
        {
            await Clients.Group(groupName).InvokeAsync("Send", message);
        }
    }

    public static class LiveHubExtensions
    {
        public static Task SendSlideAvailable(this IHubContext<LiveHub> context, string presenter, string slug, int slideNumber) =>
            SendSlideAvailable(context, $"{presenter}/{slug}", slideNumber);

        public static Task SendSlideAvailable(this IHubContext<LiveHub> context, string groupName, int slideNumber) =>
            context.Clients.Group(groupName).InvokeAsync("SlideAvailable", new SlideAvailable {Number = slideNumber});
    }
}