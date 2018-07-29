using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Server.Hubs;
using Server.Services;

namespace Server.Controllers
{
    [Produces("application/json")]
    [Route("api/Values")]
    public class ValuesController : Controller
    {
        private readonly IBackgroundService _backgroundService;

        public ValuesController(IBackgroundService backgroundService)
        {
            _backgroundService = backgroundService;
        }

        // GET: api/Values/message
        [HttpGet("{connId}/{message}")]
        public async Task<IActionResult> Get(string connId, string message)
        {
            // Start background task
            await _backgroundService.SendMessageAsync(connId, message);
            return Ok();
        }
    }
}
