using Microsoft.AspNetCore.Mvc;
using Sinch;
using Sinch.Voice.Calls.Actions;
using Sinch.Voice.Calls.Instructions;
using Sinch.Voice.Hooks;

namespace WebApiExamples.Controllers
{
    [ApiController]
    [Route("voice")]
    public class HandleIncomingIceEventController : ControllerBase
    {
        private readonly ISinchClient _sinchClient;

        public HandleIncomingIceEventController(ISinchClient sinchClient)
        {
            _sinchClient = sinchClient;
        }

        [HttpPost]
        [Route("ice-event")]
        public IActionResult HandleEvent([FromBody] IncomingCallEvent incomingCallEvent)
        {
            var response = new CallEventResponse()
            {
                Action = new Hangup(),
                Instructions = new List<IInstruction>()
                {
                    new Say()
                    {
                        Text = "Thank you for calling Sinch! This call will now end.",
                        Locale = "en-US"
                    }
                }
            };
            return Ok(response);
        }
    }
}
