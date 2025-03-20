using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Apps.Create
{
    /// <summary>
    ///     This object contains additional settings related to callback processing.
    /// </summary>
    public sealed class CallbackSettings
    {
        /// <summary>
        ///     Optional. Secret can be used to sign contents of delivery receipts for a message that was sent with the default callback URL overridden (using the [&#x60;callback_url&#x60; field](https://developers.sinch.com/docs/conversation/api-reference/conversation/tag/Messages/#tag/Messages/operation/Messages_SendMessage!path&#x3D;callback_url&amp;t&#x3D;request)). You can then use the secret to verify the signature.
        /// </summary>
        [JsonPropertyName("secret_for_overridden_callback_urls")]
        public string? SecretForOverriddenCallbackUrls { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(CallbackSettings)} {{\n");
            sb.Append($"  {nameof(SecretForOverriddenCallbackUrls)}: ").Append(SecretForOverriddenCallbackUrls).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }

    }
}
