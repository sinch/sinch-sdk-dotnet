using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Fax.Faxes
{
    /// <summary>
    /// Fax object, see https://developers.sinch.com/docs/fax for more information
    /// </summary>
    public sealed class Fax
    {
        /// <summary>
        ///     The id of a fax
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Direction fax was sent, inbound someone sent a fax to your sinch number, outbound you sent a fax to someone
        /// </summary>
        [JsonPropertyName("direction")]
        public Direction? Direction { get; set; }

        /// <summary>
        ///     A phone number in [E.164](https://community.sinch.com/t5/Glossary/E-164/ta-p/7537) format, including the leading &#39;+&#39;.
        /// </summary>
        [JsonPropertyName("from")]
        public string? From { get; set; }

        /// <summary>
        ///     A phone number in [E.164](https://community.sinch.com/t5/Glossary/E-164/ta-p/7537) format, including the leading &#39;+&#39;.
        /// </summary>
        [JsonPropertyName("to")]
        public string? To { get; set; }

        /// <summary>
        ///     Give us any URL on the Internet (including ones with basic authentication) At least one file or contentUrl parameter is required. <br/><br/>
        ///     Please note: If you are passing fax a secure URL (starting with https://), make sure that your SSL certificate (including your intermediate cert, if you have one) is installed properly, valid, and up-to-date.
        ///     If the file parameter is specified as well, content from URLs will be rendered before content from files.
        /// </summary>
        [JsonPropertyName("contentUrl")]
        public List<string>? ContentUrl { get; set; }

        /// <summary>
        ///     The number of pages in the fax.
        /// </summary>
        [JsonPropertyName("numberOfPages")]
        public int NumberOfPages { get; set; }

        /// <summary>
        /// Gets or Sets Status
        /// </summary>
        [JsonPropertyName("status")]
        public FaxStatus? Status { get; set; }

        /// <summary>
        ///     Gets or Sets Price
        /// </summary>
        [JsonPropertyName("price")]
        public Money? Price { get; set; }

        /// <summary>
        ///     The bar codes found in the fax. This field is populated when sinch detects bar codes on incoming faxes.
        /// </summary>
        [JsonPropertyName("barCodes")]
        public List<Barcode>? BarCodes { get; set; }

        /// <summary>
        ///     A timestamp representing the time when the initial API call was made.
        /// </summary>
        [JsonPropertyName("createTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        ///     If the job is complete, this is a timestamp representing the time the job was completed.
        /// </summary>
        [JsonPropertyName("completedTime")]
        public DateTime? CompletedTime { get; set; }

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

        [JsonPropertyName("cancelTimeoutMinutes")]
        public int? CancelTimeoutMinutes { get; set; }

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
        /// The content type of the callback.
        /// </summary>
        [JsonPropertyName("callbackUrlContentType")]
        public CallbackUrlContentType? CallbackUrlContentType { get; set; }

        /// <summary>
        /// Determines how documents are converted to black and white. Defaults to value selected on Fax Service object.
        /// </summary>
        [JsonPropertyName("imageConversionMethod")]
        public ImageConversionMethod? ImageConversionMethod { get; set; }

        /// <summary>
        /// Gets or Sets ErrorType
        /// </summary>
        [JsonPropertyName("errorType")]
        public ErrorType? ErrorType { get; set; }

        /// <summary>
        ///     One of the error codes listed in the [Fax Error Messages section](https://developers.sinch.com/docs/fax/api-reference/fax/tag/Error-Messages).
        /// </summary>
        [JsonPropertyName("errorCode")]
        public int? ErrorCode { get; set; }

        /// <summary>
        ///     One of the error codes listed in the [Fax Error Messages section](https://developers.sinch.com/docs/fax/api-reference/fax/tag/Error-Messages).
        /// </summary>
        [JsonPropertyName("errorMessage")]
        public string? ErrorMessage { get; set; }

        /// <summary>
        ///     The &#x60;Id&#x60; of the project associated with the call.
        /// </summary>
        [JsonPropertyName("projectId")]
        public string? ProjectId { get; set; }

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

        /// <summary>
        ///     The number of times the fax has been retried.
        /// </summary>
        [JsonPropertyName("retryCount")]
        public int? RetryCount { get; set; }

        /// <summary>
        ///     Only shown on the fax result. This indicates if the content of the fax is stored with Sinch. (true or false)
        /// </summary>
        [JsonPropertyName("hasFile")]
        public bool? HasFile { get; set; }

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(Fax)} {{\n");
            sb.Append($"  {nameof(Id)}: ").Append(Id).Append('\n');
            sb.Append($"  {nameof(Direction)}: ").Append(Direction).Append('\n');
            sb.Append($"  {nameof(From)}: ").Append(From).Append('\n');
            sb.Append($"  {nameof(To)}: ").Append(To).Append('\n');
            sb.Append($"  {nameof(ContentUrl)}: ").Append(ContentUrl).Append('\n');
            sb.Append($"  {nameof(NumberOfPages)}: ").Append(NumberOfPages).Append('\n');
            sb.Append($"  {nameof(Status)}: ").Append(Status).Append('\n');
            sb.Append($"  {nameof(Price)}: ").Append(Price).Append('\n');
            sb.Append($"  {nameof(BarCodes)}: ").Append(BarCodes).Append('\n');
            sb.Append($"  {nameof(CreateTime)}: ").Append(CreateTime).Append('\n');
            sb.Append($"  {nameof(CompletedTime)}: ").Append(CompletedTime).Append('\n');
            sb.Append($"  {nameof(HeaderText)}: ").Append(HeaderText).Append('\n');
            sb.Append($"  {nameof(HeaderPageNumbers)}: ").Append(HeaderPageNumbers).Append('\n');
            sb.Append($"  {nameof(HeaderTimeZone)}: ").Append(HeaderTimeZone).Append('\n');
            sb.Append($"  {nameof(RetryDelaySeconds)}: ").Append(RetryDelaySeconds).Append('\n');
            sb.Append($"  {nameof(Labels)}: ").Append(Labels).Append('\n');
            sb.Append($"  {nameof(CallbackUrl)}: ").Append(CallbackUrl).Append('\n');
            sb.Append($"  {nameof(CallbackUrlContentType)}: ").Append(CallbackUrlContentType).Append('\n');
            sb.Append($"  {nameof(ImageConversionMethod)}: ").Append(ImageConversionMethod).Append('\n');
            sb.Append($"  {nameof(ErrorType)}: ").Append(ErrorType).Append('\n');
            sb.Append($"  {nameof(ErrorCode)}: ").Append(ErrorCode).Append('\n');
            sb.Append($"  {nameof(ProjectId)}: ").Append(ProjectId).Append('\n');
            sb.Append($"  {nameof(ServiceId)}: ").Append(ServiceId).Append('\n');
            sb.Append($"  {nameof(MaxRetries)}: ").Append(MaxRetries).Append('\n');
            sb.Append($"  {nameof(RetryCount)}: ").Append(RetryCount).Append('\n');
            sb.Append($"  {nameof(HasFile)}: ").Append(HasFile).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    /// <summary>
    /// The content type of the callback.
    /// </summary>
    /// <value>The content type of the callback.</value>
    [JsonConverter(typeof(EnumRecordJsonConverter<CallbackUrlContentType>))]
    public record CallbackUrlContentType(string Value) : EnumRecord(Value)
    {
        public static readonly CallbackUrlContentType MultipartFormData = new("multipart/form-data");
        public static readonly CallbackUrlContentType ApplicationJson = new("application/json");

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
