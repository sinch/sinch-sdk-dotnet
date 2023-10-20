using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Verification.Start.Request
{
    public class VerificationStartRequest
    {
        /// <summary>
        ///     Specifies the type of endpoint that will be verified and the particular endpoint.
        ///     `number` is currently the only supported endpoint type.
        /// </summary>
        public Identity Identity { get; set; }
        
        /// <summary>
        ///     The type of the verification request.
        /// </summary>
        public VerificationMethod Method { get; set; }
        
        /// <summary>
        ///     Used to pass your own reference in the request for tracking purposes.
        /// </summary>
        public string Reference { get; set; }
        
        /// <summary>
        ///     Can be used to pass custom data in the request.
        /// </summary>
        public string Custom { get; set; }
        
        /// <summary>
        ///     An optional object for flashCall verifications.
        ///     It allows you to specify dial time out parameter for flashCall.
        ///     FlashCallOptions object can be specified optionally, and only
        ///     if the verification request was triggered from your backend (no SDK client)
        ///     through an Application signed request.
        /// </summary>
        public FlashCallOptions FlashCallOptions { get; set; }
    }

    public class FlashCallOptions
    {
        /// <summary>
        ///    The dial timeout in seconds. 
        /// </summary>
        public int DialTimeout { get; set; }
    }

    [JsonConverter(typeof(EnumRecordJsonConverter<VerificationMethod>))]
    public record VerificationMethod(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     Verification by SMS message with a PIN code.
        /// </summary>
        public static readonly VerificationMethod Sms = new("sms");
        
        /// <summary>
        ///     Verification by placing a flashcall (missed call) and detecting the incoming calling number (CLI).
        /// </summary>
        public static readonly VerificationMethod FlashCall = new("flashCall");
        
        /// <summary>
        ///     Verification by placing a PSTN call to the user's phone and playing an announcement,
        ///     asking the user to press a particular digit to verify the phone number.
        /// </summary>
        public static readonly VerificationMethod Callout = new("callout");
        
        /// <summary>
        ///     Data verification. Verification by accessing internal infrastructure of mobile carriers to verify
        ///     if given verification attempt was originated from device with matching phone number.
        /// </summary>
        public static readonly VerificationMethod Seamless = new("seamless");
    }

    public class Identity
    {
        public Identity()
        {
        }

        public Identity(string endpoint, IdentityType type)
        {
            Endpoint = endpoint;
            Type = type;
        }

        /// <summary>
        ///     Currently only number type is supported.
        /// </summary>
        public IdentityType Type { get; set; }
        
        /// <summary>
        ///     For type number use an E.164-compatible phone number.
        /// </summary>
        public string Endpoint { get; set; }
    }
    
    [JsonConverter(typeof(EnumRecordJsonConverter<IdentityType>))]
    public record IdentityType(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     Verification by SMS message with a PIN code.
        /// </summary>
        public static readonly IdentityType Number = new("number");
    }
}
