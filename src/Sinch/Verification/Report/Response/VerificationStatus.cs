using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Verification.Report.Response
{
    [JsonConverter(typeof(EnumRecordJsonConverter<VerificationStatus>))]
    public record VerificationStatus(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     The verification is ongoing.
        /// </summary>
        public static readonly VerificationStatus Pending = new VerificationStatus("PENDING");

        /// <summary>
        ///     The verification was successful.
        /// </summary>
        public static readonly VerificationStatus Successful = new VerificationStatus("SUCCESSFUL");

        /// <summary>
        ///     The verification attempt was made, but the number wasn't verified.
        /// </summary>
        public static readonly VerificationStatus Fail = new VerificationStatus("FAIL");

        /// <summary>
        ///     The verification attempt was denied by Sinch or your backend.
        /// </summary>
        public static readonly VerificationStatus Denied = new VerificationStatus("DENIED");

        /// <summary>
        ///     The verification attempt was aborted by requesting a new verification.
        /// </summary>
        public static readonly VerificationStatus Aborted = new VerificationStatus("ABORTED");

        /// <summary>
        ///     The verification couldn't be completed due to a network error or the number being unreachable.
        /// </summary>
        public static readonly VerificationStatus Error = new VerificationStatus("ERROR");
    }
}
