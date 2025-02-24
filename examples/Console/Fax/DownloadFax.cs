using Sinch;

namespace Examples.Fax
{
    public class DownloadFax
    {
        public static async Task Example()
        {
            var sinchClient = new SinchClient(new SinchClientConfiguration()
            {
                SinchCommonCredentials = new SinchCommonCredentials()
                {
                    ProjectId = "PROJECT_ID",
                    KeyId = "KEY_ID",
                    KeySecret = "KEY_SECRET"
                }
            });
            const string faxId = "FAX_ID";

            await using var contentResult = await sinchClient.Fax.Faxes.DownloadContent("faxId");
            const string directory = @"C:\Downloads\";
            if (!Path.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            await using var fileStream =
                new FileStream(Path.Combine(directory, contentResult.FileName ?? $"{faxId}.pdf"), FileMode.Create,
                    FileAccess.Write);
            await contentResult.Stream.CopyToAsync(fileStream);
        }
    }
}
