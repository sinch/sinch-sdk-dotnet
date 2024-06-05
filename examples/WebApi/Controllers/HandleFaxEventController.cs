using Microsoft.AspNetCore.Mvc;
using Sinch.Fax.Hooks;

namespace WebApiExamples.Controllers
{
    [ApiController]
    [Route("fax")]
    public class HandleFaxEventController : ControllerBase
    {
        [HttpPost]
        [Route("event")]
        public IActionResult HandleEvent([FromBody] IFaxEvent faxEvent)
        {
            switch (faxEvent)
            {
                case IncomingFaxEvent incomingFaxEvent:
                    break;
                case CompletedFaxEvent completedFaxEvent:
                    break;
                default:
                    return BadRequest();
            }

            return BadRequest();
        }
    }
}
