using Sinch;

namespace Examples.Fax
{
    public class DownloadFax
    {
        public static async Task Example()
        {
            var sinchClient = new SinchClient("PROJECT_ID", "KEY_ID", "KEY_SECRET");
            var faxId = "FAX_ID";
            await using var responseStream = await sinchClient.Fax.Faxes.DownloadContent("faxId");
            
            if (!Path.Exists("C:\\Downloads\\"))
            {
                Directory.CreateDirectory("C:\\Downloads\\");
            }
            
            await using var fileStream =
                new FileStream($"C:\\Downloads\\{faxId}.pdf", FileMode.Create, FileAccess.Write);
            await responseStream.CopyToAsync(fileStream);
        }
    }
}
