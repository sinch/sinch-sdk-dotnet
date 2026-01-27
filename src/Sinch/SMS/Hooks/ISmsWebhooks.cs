using System.Collections.Generic;
using System.Text.Json;

namespace Sinch.SMS.Hooks
{
    /// <summary>
    ///     SMS WebHooks service.
    ///     <para>
    ///     A callback is an HTTP POST request with a notification made by the Sinch SMS REST API to a URI
    ///     of your choosing.
    ///     </para>
    ///     <para>
    ///     The REST API expects the receiving server to respond with a response code within the <c>2xx</c>
    ///     success range. For <c>5xx</c> the callback will be retried. For <c>429</c> the callback will be
    ///     retried and the throughput will be lowered. For other status codes in the <c>4xx</c> range the
    ///     callback will not be retried. The first initial retry will happen 5 seconds after the first try.
    ///     The next attempt is after 10 seconds, then after 20 seconds, after 40 seconds, after 80 seconds,
    ///     doubling on every attempt. The last retry will be at 81920 seconds (or 22 hours 45 minutes) after
    ///     the initial failed attempt.
    ///     </para>
    ///     <para>
    ///     The SMS REST API offers the following callback options which can be configured for your account
    ///     upon request to your account manager:
    ///     </para>
    ///     <list type="bullet">
    ///         <item><description>Callback with mutual authentication over TLS (HTTPS) connection by provisioning the callback URL with client keystore and password.</description></item>
    ///         <item><description>Callback with basic authentication by provisioning the callback URL with username and password.</description></item>
    ///         <item><description>Callback with OAuth 2.0 by provisioning the callback URL with username, password and the URL to fetch OAuth access token.</description></item>
    ///         <item><description>Callback using AWS SNS by provisioning the callback URL with an Access Key ID, Secret Key and Region.</description></item>
    ///     </list>
    /// </summary>
    /// <seealso href="https://developers.sinch.com/docs/sms/api-reference/sms/tag/Webhooks/">SMS Webhooks Documentation</seealso>
    public interface ISmsWebhooks
    {
        internal JsonSerializerOptions JsonSerializerOptions { get; }

        /// <summary>
        ///     Parse a webhook event from JSON payload.
        /// </summary>
        /// <param name="json">The raw JSON payload from the webhook request body.</param>
        /// <returns>
        ///     Parsed SMS event. Use pattern matching to handle specific event types:
        ///     <list type="bullet">
        ///         <item><description><see cref="TextMessage"/> - Incoming text message (mo_text)</description></item>
        ///         <item><description><see cref="BinaryMessage"/> - Incoming binary message (mo_binary)</description></item>
        ///         <item><description><see cref="DeliveryReport"/> - Batch delivery report (delivery_report_sms, delivery_report_mms)</description></item>
        ///         <item><description><see cref="RecipientDeliveryReportSms"/> - Per-recipient delivery report (recipient_delivery_report_sms, recipient_delivery_report_mms)</description></item>
        ///     </list>
        /// </returns>
        /// <exception cref="System.Text.Json.JsonException">Thrown when JSON is invalid or cannot be deserialized.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown when event type is unknown or deserialization fails.</exception>
        ISmsEvent ParseEvent(string json);

        /// <summary>
        ///     Validate webhook authentication using HMAC signature.
        /// </summary>
        /// <param name="secret">Your webhook secret from the Sinch Dashboard.</param>
        /// <param name="headers">
        ///     All HTTP headers from the webhook request. Use a case-insensitive dictionary.
        ///     Required headers:
        ///     <list type="bullet">
        ///         <item><description>x-sinch-webhook-signature - Base64-encoded HMAC signature</description></item>
        ///         <item><description>x-sinch-webhook-signature-timestamp - Unix timestamp</description></item>
        ///         <item><description>x-sinch-webhook-signature-nonce - Random nonce</description></item>
        ///         <item><description>x-sinch-webhook-signature-algorithm - HMAC algorithm (e.g., HmacSHA256)</description></item>
        ///     </list>
        /// </param>
        /// <param name="body">The raw JSON payload from the request body (must be the exact string, not re-serialized).</param>
        /// <returns>True if signature is valid, false otherwise.</returns>
        /// <remarks>
        ///     <para>
        ///     The signature is computed as: <c>Base64(HMAC(secret, "{body}.{nonce}.{timestamp}"))</c>
        ///     </para>
        ///     <para>
        ///     Supported algorithms: HmacSHA256, HmacSHA384, HmacSHA512
        ///     </para>
        /// </remarks>
        bool ValidateAuthenticationHeader(
            string secret,
            IDictionary<string, string> headers,
            string body);
    }
}
