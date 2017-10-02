using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
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
}