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
        private List<string> _incomingFaxIds = new(); // track fax ids which was sent

        [HttpPost]
        [Route("event-json")]
        public async Task<IActionResult> HandleJsonEvent([FromBody] IFaxEvent faxEvent)
        {
            switch (faxEvent)
            {
                case IncomingFaxEvent incomingFaxEvent:
                    // just track fax ids for future
                    _incomingFaxIds.Add(incomingFaxEvent.Fax.Id);
                    break;
                case CompletedFaxEvent completedFaxEvent:
                    // download if fax completed 
                    foreach (var file in completedFaxEvent.Files)
                    {
                        var bytes = Convert.FromBase64String(file.File);
                        var contents = new MemoryStream(bytes);
                        var fileName = completedFaxEvent.Fax.Id + "." + file.FileType.Value.ToLower();
                        await SaveFile(fileName, contents);
                    }
                    break;
                default:
                    return BadRequest();
            }

            return Ok();
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
        public async Task<IActionResult> HandleJsonEvent([FromForm] FormDataFaxEvent faxEvent)
        {
            if (string.IsNullOrEmpty(faxEvent.Event) || string.IsNullOrEmpty(faxEvent.Fax))
            {
                return BadRequest();
            }

            var @event = new FaxEventType(faxEvent.Event);
            var fax = JsonSerializer.Deserialize<Fax>(faxEvent.Fax)!;

            if (faxEvent.File != null)
            {
                await using var stream = faxEvent.File.OpenReadStream();
                await SaveFile(faxEvent.File.FileName, stream);

                return Ok($"I caught event - {@event}; of fax id - {fax.Id}; with filename: {faxEvent.File.FileName}!");
            }

            return BadRequest();
        }

        private static async Task SaveFile(string fileName, Stream stream)
        {
            const string directory = @"C:\Downloads\";
            if (!Path.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            await using var fileStream =
                new FileStream(Path.Combine(directory, fileName), FileMode.Create,
                    FileAccess.Write);
            await stream.CopyToAsync(fileStream);
        }
    }
}
