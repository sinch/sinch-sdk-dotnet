/// <summary>
/// Sinch .NET SDK Snippet
/// 
/// This snippet demonstrates sending a fax with contentUrl to multiple recipients.
/// This is useful for testing multipart/form-data encoding against the real Sinch Fax API.
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

Console.WriteLine("Sending a fax with contentUrl to multiple recipients");

const string phoneNumber = "my-virtual-number";

var sendFaxResponse = await sinchClient.Fax.Faxes.Send(
    new SendFaxRequest
    {
        To = [phoneNumber],
        ContentUrl = new List<string> { "https://developers.sinch.com/fax/fax.pdf" }
    }
);

var jsonResponse = JsonSerializer.Serialize(sendFaxResponse, new JsonSerializerOptions()
{
    WriteIndented = true
});

Console.WriteLine($"Response: {jsonResponse}");

