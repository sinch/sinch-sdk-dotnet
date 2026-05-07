using System.Threading;
using System.Threading.Tasks;

namespace Sinch.Numbers.EventDestinations
{
    /// <summary>
    ///     Manage the event destination configuration for your Numbers project.
    ///     You can set up event destination URLs to receive event notifications when your numbers are updated.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     When delivering events, the order is not guaranteed (for example, a failed event scheduled for retry
    ///     will not block other events that were queued).
    /// </para>
    /// <para>
    ///     The event handler must implement the state machine that can decide what to do with unexpected events,
    ///     for example, "old" events or invalid state transitions. In these cases, the handler could use the API
    ///     to GET the latest state for the resource.
    /// </para>
    /// <para>
    ///     The event handler is expected to ingest the event and respond with 200 OK. The domain-specific business
    ///     logic and processes should be executed outside of the event request, as internal asynchronous jobs.
    /// </para>
    /// <para>
    ///     To receive events, add the following IP addresses to your allowlist:
    /// </para>
    /// <list type="bullet">
    ///     <item>54.76.19.159</item>
    ///     <item>54.78.194.39</item>
    ///     <item>54.155.83.128</item>
    /// </list>
    /// <para>
    ///     <b>Secure Event Destination Endpoints with HMAC</b><br/>
    ///     Implementing HMAC (Hash-based Message Authentication Code) on your event destination endpoints will
    ///     ensure the integrity of data, preventing tampering during transmission.
    /// </para>
    /// <para>
    ///     An HMAC is a common approach in the industry — it is a special code that can be used to verify
    ///     that a message has not been tampered with during transmission.
    /// </para>
    /// <para>
    ///     We recommend configuring an HMAC secret for your project using the event destination configuration.
    ///     Then, when sending the number events, the HTTP POST requests will include the header
    ///     <c>X-Sinch-Signature</c> with the computed HMAC.
    /// </para>
    /// <para>
    ///     <b>Note:</b> The HMAC secret is configured per project. If you are using the Numbers API with multiple
    ///     projects, make sure you configure the HMAC secret in each project, and fetch it for either imported or
    ///     purchased numbers from the dedicated endpoints.
    /// </para>
    /// <para>
    ///     <b>HMAC Verification</b><br/>
    ///     We recommend verifying the HMAC code received with every event in your event handler. When receiving a
    ///     new event on your event destination URL, compute the HMAC of the payload using the secret and compare it
    ///     with the value received in the <c>X-Sinch-Signature</c> header.
    /// </para>
    /// <para>
    ///     <b>Note:</b> Compute the HMAC on the plain text value before parsing the JSON payload.
    /// </para>
    /// </remarks>
    public interface ISinchNumbersEventDestinations
    {
        /// <summary>
        ///     Returns the event destination configuration for your project.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<EventDestination> Get(CancellationToken cancellationToken = default);

        /// <summary>
        ///     Updates the event destination configuration for your project.
        /// </summary>
        /// <param name="hmacSecret">The HMAC secret to be updated.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<EventDestination> Update(string hmacSecret, CancellationToken cancellationToken = default);
    }
}
