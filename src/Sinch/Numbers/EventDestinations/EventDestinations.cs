using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Numbers.EventDestinations
{
    /// <summary>
    /// You can set up event destination URLs to receive event notifications when your numbers are updated.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When delivering events, the order is not guaranteed. For example, a failed event scheduled for retry
    /// will not block other events that were queued.
    /// </para>
    /// <para>
    /// The client's event destination handler must implement the state machine logic to determine how to handle
    /// unexpected events, such as "old" events or invalid state transitions. In these cases, the handler may
    /// use the API to GET the latest state for the resource.
    /// </para>
    /// <para>
    /// The event destination handler is expected to ingest the event and respond with HTTP 200 OK. Domain-specific
    /// business logic and processes should be executed outside of the event destination request as internal asynchronous jobs.
    /// </para>
    /// <para>
    /// To use event destinations, add the following IP addresses to your allowlist:
    /// </para>
    /// <list type="bullet">
    /// <item>54.76.19.159</item>
    /// <item>54.78.194.39</item>
    /// <item>54.155.83.128</item>
    /// </list>
    /// <para>
    /// <b>Secure Sinch event endpoints with HMAC</b><br/>
    /// Implementing HMAC (Hash-based Message Authentication Code) on your Sinch event endpoints ensures the
    /// integrity of data and prevents tampering during transmission.
    /// </para>
    /// <para>
    /// We recommend configuring an HMAC secret for your project using the event destinations. When sending
    /// number events, the HTTP POST requests will include the header <c>X-Sinch-Signature</c> with the computed HMAC.
    /// </para>
    /// <para>
    /// <b>Note:</b> The HMAC secret is configured per project. If you use the Numbers API with multiple projects,
    /// ensure the HMAC secret is configured for each project, and fetch it for either imported or purchased numbers
    /// from the dedicated endpoints.
    /// </para>
    /// <para>
    /// <b>HMAC Verification</b><br/>
    /// Verify the HMAC code received with every event in your event handler. When receiving a new event on your
    /// endpoint URL, compute the HMAC of the payload using the secret and compare it with the value in the
    /// <c>X-Sinch-Signature</c> header.
    /// </para>
    /// <para>
    /// <b>Note:</b> Compute the HMAC on the plain text value before parsing the JSON payload.
    /// </para>
    /// </remarks>
    public interface ISinchNumbersEventDestination
    {
        /// <summary>
        ///     Returns the event destination for your project
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<EventDestination> Get(CancellationToken cancellationToken = default);

        /// <summary>
        ///     Updates the event destination    configuration for your project
        /// </summary>
        /// <param name="hmacSecret">The HMAC secret to be updated</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<EventDestination> Update(string hmacSecret, CancellationToken cancellationToken = default);
    }

    internal sealed class SinchNumbersEventDestination : ISinchNumbersEventDestination
    {
        private readonly Uri _baseAddress;
        private readonly IHttp _http;
        private readonly ILoggerAdapter<ISinchNumbersEventDestination>? _logger;
        private readonly string _projectId;

        public SinchNumbersEventDestination(string projectId, Uri baseAddress, ILoggerAdapter<ISinchNumbersEventDestination>? logger,
            IHttp http)
        {
            _projectId = projectId;
            _baseAddress = baseAddress;
            _logger = logger;
            _http = http;
        }

        public Task<EventDestination> Get(CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Fetching event destinations for {projectId}", _projectId);
            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/callbackConfiguration");
            return _http.Send<EventDestination>(uri, HttpMethod.Get, cancellationToken);
        }

        public Task<EventDestination> Update(string hmacSecret, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(hmacSecret))
            {
                throw new ArgumentNullException(nameof(hmacSecret));
            }

            _logger?.LogDebug("Updating event destinations for {projectId}", _projectId);
            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/callbackConfiguration");
            return _http.Send<object, EventDestination>(uri, HttpMethod.Patch, new
            {
                hmacSecret = hmacSecret,
            }, cancellationToken);
        }
    }
}
