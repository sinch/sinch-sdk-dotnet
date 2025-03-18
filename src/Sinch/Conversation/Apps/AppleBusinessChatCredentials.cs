using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Apps
{
    /// <summary>
    ///     If you are including the AppleBC channel in the &#x60;channel_identifier&#x60; property, you must include this object.
    /// </summary>
    public sealed class AppleBusinessChatCredentials
    {
        /// <summary>
        ///     The ID that identifies a Business Chat Account (BCA).
        /// </summary>
        [JsonPropertyName("business_chat_account_id")]
#if NET7_0_OR_GREATER
        public required string BusinessChatAccountId { get; set; }
#else
        public string BusinessChatAccountId { get; set; } = null!;
#endif


        /// <summary>
        ///     Merchant ID, required if our client wants to use Apple Pay.
        /// </summary>
        [JsonPropertyName("merchant_id")]
        public string? MerchantId { get; set; }


        /// <summary>
        ///     Certificate reference, required if our client wants to use Apple Pay.
        /// </summary>
        [JsonPropertyName("apple_pay_certificate_reference")]
        public string? ApplePayCertificateReference { get; set; }


        /// <summary>
        ///     Certificate password, required if our client wants to use Apple Pay.
        /// </summary>
        [JsonPropertyName("apple_pay_certificate_password")]
        public string? ApplePayCertificatePassword { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(AppleBusinessChatCredentials)} {{\n");
            sb.Append($"  {nameof(BusinessChatAccountId)}: ").Append(BusinessChatAccountId).Append('\n');
            sb.Append($"  {nameof(MerchantId)}: ").Append(MerchantId).Append('\n');
            sb.Append($"  {nameof(ApplePayCertificateReference)}: ").Append(ApplePayCertificateReference).Append('\n');
            sb.Append($"  {nameof(ApplePayCertificatePassword)}: ").Append(ApplePayCertificatePassword).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
