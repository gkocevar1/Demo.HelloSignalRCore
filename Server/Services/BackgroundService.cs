using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Server.Hubs;

namespace Server.Services
{
    public class BackgroundService : IBackgroundService
    {
        private const int DelaySeconds = 5;
        private IHubContext<ValuesHub> _hubContext;

        public BackgroundService(IHubContext<ValuesHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task SendMessageAsync(string connectionId, string message)
        {
            var timer = new Timer(async state =>
            {
                await _hubContext.Clients.Client(connectionId).SendAsync("receiveMessage", state);
            }, message, DelaySeconds * 1000, Timeout.Infinite);
            return Task.CompletedTask;
        }
    }
}