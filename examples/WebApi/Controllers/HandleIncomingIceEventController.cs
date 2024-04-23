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
        [Route("event")]
        public IActionResult HandleEvent([FromBody] IVoiceEvent incomingEvent)
        {
            switch (incomingEvent)
            {
                case AnsweredCallEvent answeredCallEvent:
                    break;
                case DisconnectedCallEvent disconnectedCallEvent:
                    break;
                case IncomingCallEvent incomingCallEvent:
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
                case NotificationEvent notificationEvent:
                    break;
                case PromptInputEvent promptInputEvent:
                    break;
                default:
                    return BadRequest();
            }

            return BadRequest();
        }
    }
}
