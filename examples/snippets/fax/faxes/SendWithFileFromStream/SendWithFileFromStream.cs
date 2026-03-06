/// <summary>
/// Sinch .NET SDK Snippet
/// 
/// This snippet is available at https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets
/// 
/// See https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets/README.md for details
/// </summary>
using Sinch;
using Sinch.Core;
using Sinch.Fax;
using Sinch.Fax.Faxes;
using Sinch.Snippets.Shared;

var sinchClient = new SinchClient(new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials
    {
        ProjectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID",
        KeyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID",
        KeySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET"
    },
    FaxConfiguration = new SinchFaxConfiguration
    {
        Region = new FaxRegion(ConfigurationHelper.GetFaxRegion() ?? "MY_FAX_REGION")
    }
});

const string recipient = "RECIPIENT_PHONE_NUMBER";
const string fileName = "sample.txt";

Console.WriteLine("Sending a fax with file from stream to a recipient");

var fileContent = File.ReadAllBytes("./sample.txt");
await using var singleRecipientStream = new MemoryStream(fileContent);
using var singleRecipientRequest = SendFaxRequest.FromStream(singleRecipientStream, fileName);
singleRecipientRequest.To = [recipient];

var response = await sinchClient.Fax.Faxes.Send(singleRecipientRequest);

Console.WriteLine($"Response: {response.ToPrettyString()}");
