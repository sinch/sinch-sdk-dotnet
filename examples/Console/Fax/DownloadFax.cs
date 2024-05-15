using Sinch;

namespace Examples.Fax
{
    public class DownloadFax
    {
        public static async Task Example()
        {
            var sinchClient = new SinchClient("PROJECT_ID", "KEY_ID", "KEY_SECRET");
            await using var responseStream = await sinchClient.Fax.Faxes.DownloadContent("FAX_ID");
            await using var fileStream = new FileStream("C:\\Downloads\\fax.pdf", FileMode.Create, FileAccess.Write);
            await responseStream.CopyToAsync(fileStream);
        }
    }
}
