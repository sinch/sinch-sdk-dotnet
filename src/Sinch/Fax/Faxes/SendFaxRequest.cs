using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Sinch.Core;

namespace Sinch.Fax.Faxes
{
    public class SendFaxRequest : IDisposable, IAsyncDisposable
    {
        [Obsolete("Required for system text json", true)]
        public SendFaxRequest()
        {
        }

        /// <summary>
        ///     Creates a fax with contentUrls
        /// </summary>
        /// <param name="contentUrl"></param>
        public SendFaxRequest(List<string> contentUrl)
        {
            ContentUrl = contentUrl;
        }

        [JsonIgnore]
        internal Stream? FileContent { get; }

        [JsonIgnore]
        internal string? FileName { get; }
        //
        // [JsonInclude]
        // [JsonPropertyName("files")]
        // public List<Base64File> Files { get; private set; }

        /// <summary>
        ///     Creates a fax with attached content
        /// </summary>
        /// <param name="fileContent"></param>
        /// <param name="fileName"></param>
        public SendFaxRequest(Stream fileContent, string fileName)
        {
            FileContent = fileContent;
            FileName = fileName;
        }

        /// <summary>
        ///     Creates a fax with attached content from a path
        /// </summary>
        /// <param name="filePath"></param>
        public SendFaxRequest(string filePath)
        {
            FileContent = File.OpenRead(filePath);
            FileName = Path.GetFileName(filePath);
        }

        /// <summary>
        ///     A phone number in [E.164](https://community.sinch.com/t5/Glossary/E-164/ta-p/7537) format, including the leading &#39;+&#39;.
        /// </summary>
        [JsonPropertyName("to")]
#if NET7_0_OR_GREATER
        public required string To { get; set; }
#else
        public string To { get; set; } = null!;
#endif


        /// <summary>
        ///     A phone number in [E.164](https://community.sinch.com/t5/Glossary/E-164/ta-p/7537) format, including the leading &#39;+&#39;.
        /// </summary>
        [JsonPropertyName("from")]
        public string? From { get; set; }

        /// <summary>
        ///     Give us any URL on the Internet (including ones with basic authentication) At least one file or contentUrl parameter is required. <br/><br/>
        ///     Please note: If you are passing fax a secure URL (starting with https://), make sure that your SSL certificate (including your intermediate cert, if you have one) is installed properly, valid, and up-to-date.
        ///     If the file parameter is specified as well, content from URLs will be rendered before content from files.
        /// </summary>
        [JsonPropertyName("contentUrl")]
        [JsonInclude]
        public List<string>? ContentUrl { get; private set; }

        /// <summary>
        ///     Text that will be displayed at the top of each page of the fax. 50 characters maximum. Default header text is \&quot;-\&quot;. Note that the header is not applied until the fax is transmitted, so it will not appear on fax PDFs or thumbnails.
        /// </summary>
        [JsonPropertyName("headerText")]
        public string? HeaderText { get; set; }

        /// <summary>
        ///     If true, page numbers will be displayed in the header. Default is true.
        /// </summary>
        [JsonPropertyName("headerPageNumbers")]
        public bool? HeaderPageNumbers { get; set; }

        /// <summary>
        ///     A [TZ database name](https://en.wikipedia.org/wiki/List_of_tz_database_time_zones) string specifying the timezone for the header timestamp.
        /// </summary>
        [JsonPropertyName("headerTimeZone")]
        public string? HeaderTimeZone { get; set; }


        /// <summary>
        ///     The number of seconds to wait between retries if the fax is not yet completed.
        /// </summary>
        [JsonPropertyName("retryDelaySeconds")]
        public int? RetryDelaySeconds { get; set; }

        /// <summary>
        ///     You can use this to attach labels to your call that you can use in your applications. It is a key value store.
        /// </summary>
        [JsonPropertyName("labels")]
        public Dictionary<string, string>? Labels { get; set; }

        /// <summary>
        ///     The URL to which a callback will be sent when the fax is completed. The callback will be sent as a POST request with a JSON body. The callback will be sent to the URL specified in the &#x60;callbackUrl&#x60; parameter, if provided, otherwise it will be sent to the URL specified in the &#x60;callbackUrl&#x60; field of the Fax Service object.
        /// </summary>
        [JsonPropertyName("callbackUrl")]
        public string? CallbackUrl { get; set; }

        /// <summary>
        ///     The content type of the callback.
        /// </summary>
        [JsonPropertyName("callbackContentType")]
        public CallbackContentType? CallbackContentType { get; set; }

        /// <summary>
        ///     Determines how documents are converted to black and white. Defaults to value selected on Fax Service object.
        /// </summary>
        [JsonPropertyName("imageConversionMethod")]
        public ImageConversionMethod? ImageConversionMethod { get; set; }

        /// <summary>
        ///     ID of the fax service used.
        /// </summary>
        [JsonPropertyName("serviceId")]
        public string? ServiceId { get; set; }

        /// <summary>
        ///     | The number of times the fax will be retired before cancel. Default value is set in your fax service. | The maximum number of retries is 5.
        /// </summary>
        [JsonPropertyName("maxRetries")]
        public int? MaxRetries { get; set; }

        public void Dispose()
        {
            FileContent?.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            if (FileContent != null) return FileContent.DisposeAsync();
            return ValueTask.CompletedTask;
        }
    }

    /// TODO: think about if it's worth implementing base64 send
    internal sealed class Base64File
    {
        /// <summary>
        ///     Base64 encoded file content.
        /// </summary>
        [JsonPropertyName("file")]
        public string File { get; set; }

        [JsonPropertyName("fileType")]
        public FileType FileType { get; set; }
    }

    [JsonConverter(typeof(EnumRecordJsonConverter<FileType>))]
    public record FileType(string Value) : EnumRecord(Value)
    {
        // ReSharper disable InconsistentNaming

        public static readonly FileType DOC = new("DOC");
        public static readonly FileType DOCX = new("DOCX");
        public static readonly FileType PDF = new("PDF");
        public static readonly FileType TIF = new("TIF");
        public static readonly FileType JPG = new("JPEG");
        public static readonly FileType ODT = new("ODT");
        public static readonly FileType TXT = new("TXT");
        public static readonly FileType PNG = new("PNG");

        // ReSharper restore InconsistentNaming
    }
}
