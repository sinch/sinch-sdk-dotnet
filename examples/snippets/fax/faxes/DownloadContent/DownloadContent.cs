/// <summary>
/// Sinch .NET SDK Snippet
/// 
/// This snippet is available at https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets
/// 
/// See https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets/README.md for details
/// </summary>
using Sinch;
using Sinch.Fax;
using Sinch.Snippets.Shared;

var projectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID";
var keyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID";
var keySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET";
var faxRegion = ConfigurationHelper.GetFaxRegion() ?? "MY_FAX_REGION";

// The Fax ID whose content you want to download
const string faxId = "FAX_ID";

var client = new SinchClient(new SinchClientConfiguration()
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials()
    {
        ProjectId = projectId,
        KeyId = keyId,
        KeySecret = keySecret
    },
    FaxConfiguration = new SinchFaxConfiguration
    {
        Region = new FaxRegion(faxRegion)
    }
});

Console.WriteLine("Downloading fax content as PDF");

var contentResult = await client.Fax.Faxes.DownloadContent(faxId);

var outputPath = Path.Combine(Directory.GetCurrentDirectory(), contentResult.FileName ?? $"fax_{faxId}.pdf");
await using var fileStream = File.Create(outputPath);
await contentResult.Stream.CopyToAsync(fileStream);

Console.WriteLine($"Fax content saved to: {outputPath}");
