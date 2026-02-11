using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Fax.Emails
{
    /// <summary>
    /// Allows you to set permissions for sending and receiving faxes to this email/phone number combination.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<EmailPermissions>))]
    public record EmailPermissions(string Value) : EnumRecord(Value)
    {
        public static readonly EmailPermissions Both = new("both");
        public static readonly EmailPermissions Send = new("send");
        public static readonly EmailPermissions Receive = new("receive");
    }
}
