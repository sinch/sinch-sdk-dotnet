/// <summary>
/// Sinch .NET SDK Snippet
/// 
/// This snippet is available at https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets
/// 
/// See https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets/README.md for details
/// </summary>
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

const string singleRecipient = "my-virtual-number";
const string senderNumber = "my-virtual-number"; // Must be a number associated with your account
const string faxContentUrl = "https://developers.sinch.com/fax/fax.pdf";

Console.WriteLine("Sending a fax with contentUrl to a recipient");

var singleRecipientResponse = await sinchClient.Fax.Faxes.Send(
    new SendFaxRequest
    {
        To = new List<string> { singleRecipient },
        From = senderNumber,
        ContentUrl = new List<string> { faxContentUrl }
    }
);

Console.WriteLine($"Recipient response: {JsonSerializer.Serialize(singleRecipientResponse, new JsonSerializerOptions { WriteIndented = true })}");
