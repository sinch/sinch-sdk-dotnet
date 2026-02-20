/// <summary>
/// Sinch .NET SDK Snippet
/// 
/// This snippet demonstrates sending a fax to multiple recipients using the contentUrl.
/// Each recipient in the "to" list will receive the fax independently.
/// 
/// See https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets/README.md for details
/// </summary>

using System.Text.Json;
using Sinch;
using Sinch.Core;
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

Console.WriteLine("Sending a fax with contentUrl to multiple recipients");

const string senderNumber = "my-virtual-number";
const string recipient1 = "recipient1-phone-number";
const string recipient2 = "recipient2-phone-number";

var response = await sinchClient.Fax.Faxes.Send(
    new SendFaxRequest
    {
        To = [recipient1, recipient2],
        From = senderNumber,
        ContentUrl = new List<string> { "https://developers.sinch.com/fax/fax.pdf" }
    }
);

Console.WriteLine($"Successfully sent fax to {response.Count} recipients:");
Console.WriteLine($"Response: {response.ToPrettyString()}");
