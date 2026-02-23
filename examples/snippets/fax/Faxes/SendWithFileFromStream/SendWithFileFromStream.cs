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

var sinchClient = new SinchClient(new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials
    {
        ProjectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID",
        KeyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID",
        KeySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET"
    }
});

const string senderNumber = "my-virtual-number";
const string recipient = "recipient-phone-number";
const string fileName = "sample.txt";

Console.WriteLine("Sending a fax with file from stream to a recipient");

var fileContent = File.ReadAllBytes("./sample.txt");
await using var singleRecipientStream = new MemoryStream(fileContent);
using var singleRecipientRequest = new SendFaxRequest(singleRecipientStream, fileName)
{
    To = new List<string> { recipient },
    From = senderNumber
};

var response = await sinchClient.Fax.Faxes.Send(singleRecipientRequest);

Console.WriteLine($"Response: {response.ToPrettyString()}");
