using System.Text.Json.Serialization;

namespace Sinch.SMS.Hooks
{
    /// <summary>
    ///     An inbound message is a message sent to one of your short codes or long numbers from a mobile phone.
    ///     To receive inbound message callbacks, a URL needs to be added to your REST API.
    ///     This URL can be specified in your <see href="https://dashboard.sinch.com/sms/api">Dashboard</see>.
    /// </summary>
    public sealed class IncomingBinarySms : IncomingTextSms
    {
        /// <summary>
        ///     The message content Base64 encoded. <br/><br/>
        ///     Max 140 bytes together with udh.
        /// </summary>
        [JsonPropertyName("body")]
        public override string Body { get; set; }

        /// <summary>
        ///     The UDH header of a binary message HEX encoded. <br/><br/>
        ///     Max 140 bytes together with body.
        /// </summary>
        [JsonPropertyName("udh")]
        public string Udh { get; set; }
    }
}
