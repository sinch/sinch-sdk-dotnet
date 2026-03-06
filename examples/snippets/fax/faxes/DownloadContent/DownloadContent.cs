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

var sinchClient = new SinchClient(new SinchClientConfiguration()
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials()
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

Console.WriteLine("Downloading fax content as PDF");

// The Fax ID whose content you want to download
const string faxId = "FAX_ID";

var contentResult = await sinchClient.Fax.Faxes.DownloadContent(faxId);

var outputPath = Path.Combine(Directory.GetCurrentDirectory(), contentResult.FileName ?? $"fax_{faxId}.pdf");
await using var fileStream = File.Create(outputPath);
await contentResult.Stream.CopyToAsync(fileStream);

Console.WriteLine($"Fax content saved to: {outputPath}");
