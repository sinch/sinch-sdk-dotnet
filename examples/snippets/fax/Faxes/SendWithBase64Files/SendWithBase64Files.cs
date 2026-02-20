/// <summary>
/// Sinch .NET SDK Snippet
/// 
/// This snippet is available at https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets
/// 
/// See https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets/README.md for details
/// </summary>
using System.Text;
using System.Text.Json;
using Sinch;
using Sinch.Fax.Faxes;
using Sinch.Snippets.Shared;

var sinchClient = new SinchClient(new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials
    {
        ProjectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID",
        KeyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID",
        KeySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET"
    }
});

const string recipient = "my-virtual-number";
const string senderNumber = "my-virtual-number"; // Must be a number associated with your account

// Generate base64-encoded content from a string
// In a real scenario, you would read actual file content: File.ReadAllBytes("path/to/file.txt")
var sampleContent = "Sample Text Document - This is page 1 content\nThis demonstrates sending text files as base64.";
var base64FileContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(sampleContent));

Console.WriteLine("Sending a fax with base64-encoded text files");

var recipientRequest = new SendFaxRequest([
    new()
    {
        File = base64FileContent,
        FileType = FileType.TXT
    }
])
{
    To = new List<string> { recipient },
    From = senderNumber
};

var singleRecipientResponse = await sinchClient.Fax.Faxes.Send(recipientRequest);

Console.WriteLine($"Recipient response: {JsonSerializer.Serialize(singleRecipientResponse, new JsonSerializerOptions { WriteIndented = true })}");
