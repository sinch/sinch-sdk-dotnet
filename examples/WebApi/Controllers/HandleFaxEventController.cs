using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Sinch.Fax.Faxes;
using Sinch.Fax.Hooks;

namespace WebApiExamples.Controllers
{
    [ApiController]
    [Route("fax")]
    public class HandleFaxEventController : ControllerBase
    {
        [HttpPost]
        [Route("event-json")]
        public IActionResult HandleJsonEvent([FromBody] IFaxEvent faxEvent)
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
        
        // To handle multi part form data event, utilize provided class below
        public class FormDataFaxEvent
        {
            [BindProperty(Name = "event")]
            public string? Event { get; set; }
            
            public IFormFile? File { get; set; }
            
            [BindProperty(Name = "eventTime")]
            public DateTime? EventTime { get; set; }
            
            [BindProperty(Name = "fax")]
            public string? Fax { get; set; }
        }
        
        [HttpPost]
        [Route("event-form-data")]
        public IActionResult HandleJsonEvent([FromForm] FormDataFaxEvent faxEvent)
        {
            if (string.IsNullOrEmpty(faxEvent.Event) || string.IsNullOrEmpty(faxEvent.Fax))
            {
                return BadRequest();
            }
            
            var @event = new FaxEventType(faxEvent.Event);
            var fax = JsonSerializer.Deserialize<Fax>(faxEvent.Fax)!;
            
            if (faxEvent.File != null)
            {
                return Ok($"I caught event - {@event}; of fax id - {fax.Id}; with filename: {faxEvent.File.FileName}!");
            }
            
            return BadRequest();
        }
    }
}
