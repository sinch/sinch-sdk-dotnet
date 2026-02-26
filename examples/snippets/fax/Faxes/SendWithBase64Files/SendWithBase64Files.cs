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

const string recipient = "RECIPIENT_PHONE_NUMBER";

var fileContent = File.ReadAllBytes("./sample.txt");
var base64FileContent = Convert.ToBase64String(fileContent);

Console.WriteLine("Sending a fax with base64-encoded text files");

var recipientRequest = new SendFaxRequest([
    new()
    {
        File = base64FileContent,
        FileType = FileType.TXT
    }
])
{
    To = [recipient]
};

var response = await sinchClient.Fax.Faxes.Send(recipientRequest);

Console.WriteLine($"Response: {response.ToPrettyString()}");
