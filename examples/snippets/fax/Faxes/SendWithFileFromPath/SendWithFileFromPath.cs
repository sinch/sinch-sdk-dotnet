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

const string filePath = "./fax.pdf";
const string singleRecipient = "+12063091975";
const string senderNumber = "+12052275207";

Console.WriteLine("Sending a fax with file from path");

using var singleRecipientRequest = new SendFaxRequest(filePath);
singleRecipientRequest.To = new List<string> { singleRecipient };
singleRecipientRequest.From = senderNumber;

var singleRecipientResponse = await sinchClient.Fax.Faxes.Send(singleRecipientRequest);

Console.WriteLine($"Single recipient response: {JsonSerializer.Serialize(singleRecipientResponse, new JsonSerializerOptions { WriteIndented = true })}");
