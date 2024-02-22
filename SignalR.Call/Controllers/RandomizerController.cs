using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace SignalR.Call.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RandomizerController : ControllerBase
    {
        private readonly IHubContext<RandomizerHub, IRandomizerClient> _hub;
        private readonly TimerManager _timer;
        public RandomizerController(IHubContext<RandomizerHub, IRandomizerClient> hub, TimerManager timer)
        {
            _hub = hub;
            _timer = timer;
        }

        [HttpGet("SendRandomNumber")]
        public ActionResult<int> SendRandomNumber()
        {
            var randomValue = Random.Shared.Next(1, 51) * 2;
            if (!_timer.IsTimerStarted)
            {
                _timer.PrepareTimer(() =>
                    _hub.Clients.All
                        .SendClientRandomEvenNumber(randomValue));
            }
            return Ok(randomValue);
        }
    }
}
