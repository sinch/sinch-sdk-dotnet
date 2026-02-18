using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Fax.Emails
{
    /// <summary>
    /// Allows you to set permissions for sending and receiving faxes to this email/phone number combination.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<PhoneNumberPermission>))]
    public record PhoneNumberPermission(string Value) : EnumRecord(Value)
    {
        public static readonly PhoneNumberPermission Both = new("both");
        public static readonly PhoneNumberPermission Send = new("send");
        public static readonly PhoneNumberPermission Receive = new("receive");
    }
}
