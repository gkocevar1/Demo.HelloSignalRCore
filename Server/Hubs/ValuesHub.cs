using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Server.Hubs
{
    public class ValuesHub : Hub
    {
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
    }
}
