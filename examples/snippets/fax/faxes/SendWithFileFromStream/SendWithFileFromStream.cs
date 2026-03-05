/// <summary>
/// Sinch .NET SDK Snippet
/// 
/// This snippet is available at https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets
/// 
/// See https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets/README.md for details
/// </summary>
using Sinch;
using Sinch.Core;
using Sinch.Fax.Faxes;
using Sinch.Snippets.Shared;

var projectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID";
var keyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID";
var keySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET";

// The phone number of the recipient you want to send a fax to
const string recipientPhoneNumber = "RECIPIENT_PHONE_NUMBER";
const string fileName = "sample.txt";

var fileContent = File.ReadAllBytes("./sample.txt");
await using var singleRecipientStream = new MemoryStream(fileContent);

await using var singleRecipientRequest = SendFaxRequest.FromStream(singleRecipientStream, fileName);
singleRecipientRequest.To = [recipientPhoneNumber];

var client = new SinchClient(new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials
    {
        ProjectId = projectId,
        KeyId = keyId,
        KeySecret = keySecret
    }
});

Console.WriteLine("Sending a fax with file from stream to a recipient");

var response = await client.Fax.Faxes.Send(singleRecipientRequest);

Console.WriteLine($"Response: {response.ToPrettyString()}");
