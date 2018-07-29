using System.Threading.Tasks;

namespace Server.Services
{
    public interface IBackgroundService
    {
        Task SendMessageAsync(string connectionId, string message);
    }
}