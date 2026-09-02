using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Auth;
using Sinch.Logger;

namespace Sinch.Core
{
    /// <summary>
    ///     A single place to control token fetching and common headers.
    /// </summary>
    internal interface IHttp
    {
        /// <summary>
        ///     Use to send http request without a body
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="httpMethod"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="headers"></param>
        /// <typeparam name="TResponse">The type of the response object.</typeparam>
        /// <returns></returns>
        Task<TResponse> Send<TResponse>(Uri uri, HttpMethod httpMethod,
            CancellationToken cancellationToken = default, Dictionary<string, IEnumerable<string>>? headers = null);

        /// <summary>
        ///     Use to send a http request whose content is built around a stream
        ///     (e.g. multipart content with a file attachment)
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="httpMethod"></param>
        /// <param name="stream">The stream the content is built around, or null if there isn't one.</param>
        /// <param name="buildContent">Builds the HttpContent for one attempt from the stream. Enabling to address "retry" with fresh rewound stream</param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="TResponse">The type of the response object.</typeparam>
        /// <returns></returns>
        Task<TResponse> Send<TResponse>(Uri uri, HttpMethod httpMethod, Stream? stream,
            Func<Stream?, HttpContent> buildContent, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Use to send http request with a body
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="httpMethod"></param>
        /// <param name="httpContent"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="headers"></param>
        /// <typeparam name="TRequest">The type of the request object.</typeparam>
        /// <typeparam name="TResponse">The type of the response object.</typeparam>
        /// <returns></returns>
        Task<TResponse> Send<TRequest, TResponse>(Uri uri, HttpMethod httpMethod, TRequest httpContent,
            CancellationToken cancellationToken = default, Dictionary<string, IEnumerable<string>>? headers = null);

        JsonSerializerOptions JsonSerializerOptions { get; }
    }

    /// <summary>
    ///     Represents an empty response for cases where no json is expected.
    /// </summary>
    public sealed class EmptyResponse
    {
    }

    /// <inheritdoc />
    internal sealed class Http : IHttp
    {
        private readonly Func<HttpClient> _httpClientAccessor;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly ILoggerAdapter<IHttp>? _logger;
        private readonly Lazy<ISinchAuth> _auth;

        /// <summary>
        ///     Gets the User-Agent header value for HTTP requests.
        /// </summary>
        internal static string UserAgent { get; } = BuildUserAgent();

        public Http(Lazy<ISinchAuth> auth, Func<HttpClient> httpClientAccessor, ILoggerAdapter<IHttp>? logger,
            JsonNamingPolicy jsonNamingPolicy)
        {
            _logger = logger;
            _auth = auth;
            _httpClientAccessor = httpClientAccessor;
            _jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                PropertyNamingPolicy = jsonNamingPolicy,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
        }

        public Task<TResponse> Send<TResponse>(Uri uri, HttpMethod httpMethod,
            CancellationToken cancellationToken = default, Dictionary<string, IEnumerable<string>>? headers = null)
        {
            return Send<EmptyResponse, TResponse>(uri, httpMethod, null, cancellationToken, headers);
        }

        public Task<TResponse> Send<TResponse>(Uri uri, HttpMethod httpMethod, Stream? stream,
            Func<Stream?, HttpContent> buildContent, CancellationToken cancellationToken = default)
        {
            // See https://github.com/sinch/sinch-sdk-dotnet/issues/214: content built around a
            // stream must be rebuilt for every retry attempt, since the stream is consumed by the
            // first attempt and the previous attempt's content is disposed of by then. Rewinding
            // the stream and wrapping it so that disposal doesn't reach the caller's stream. 
            // Callers just build content from the stream they're handed.
            HttpContent ContentFactory()
            {
                if (stream is null)
                {
                    return buildContent(null);
                }

                stream.Position = 0;
                return buildContent(new NonDisposingStream(stream));
            }

            return SendHttpContent<TResponse>(uri, httpMethod, ContentFactory, cancellationToken);
        }

        private async Task<TResponse> SendHttpContent<TResponse>(Uri uri, HttpMethod httpMethod,
            Func<HttpContent?> contentFactory,
            CancellationToken cancellationToken = default, Dictionary<string, IEnumerable<string>>? headers = null)
        {
            var retry = true;
            while (true)
            {
                _logger?.LogDebug("Sending request to {uri}", uri);

                // See https://github.com/sinch/sinch-sdk-dotnet/issues/214
                // A fresh HttpContent is built for every attempt
                var httpContent = contentFactory();

#if DEBUG
                Debug.WriteLine($"Http Method: {httpMethod}");
                Debug.WriteLine($"Request uri: {uri}");
#endif

                var msg = new HttpRequestMessage();
                HttpResponseMessage result;
                try
                {
                    msg.RequestUri = uri;
                    msg.Method = httpMethod;
                    msg.Content = httpContent;

                    (var token, retry) = await Authenticate(msg, retry, cancellationToken);

                    msg.Headers.Authorization = new AuthenticationHeaderValue(_auth.Value.Scheme, token);

                    msg.Headers.Add("User-Agent", UserAgent);

                    if (headers != null && headers.Any())
                    {
                        AddOrOverrideHeaders(msg, headers);
                    }

                    // Get HttpClient from factory/accessor - do not dispose as factory manages lifetime
                    var httpClient = _httpClientAccessor();
                    result = await httpClient.SendAsync(msg, cancellationToken);
                }
                finally
                {
                    // History: https://github.com/sinch/sinch-sdk-dotnet/issues/214
                    // The previous code reused one HttpContent instance across every retry attempt
                    // Disposing msg after the first attempt cascaded into disposing that shared
                    // content, so the retry crashed with ObjectDisposedException instead of
                    // resending the request
                    // Content is now rebuilt fresh for every attempt (see contentFactory above),
                    // so it's never shared across retries
                    // We keep this explicit detach-then-dispose so cleanup order stays
                    // intentional, not implicit
                    msg.Content = null;
                    msg.Dispose();
                    httpContent?.Dispose();
                }

                if (result.StatusCode == HttpStatusCode.Unauthorized && retry)
                {
                    // Only retry for Bearer (OAuth) tokens that the server explicitly reports as
                    // expired via www-authenticate. Any other scheme (e.g. Basic, the signed-auth
                    // scheme ApplicationSignedAuth uses) or a Bearer 401 without that header is a
                    // genuine auth failure, won't be retried because will get same answer again
                    const string wwwAuthenticateHeader = "www-authenticate";
                    var isExpiredBearerToken = _auth.Value.Scheme == AuthSchemes.Bearer &&
                        result.Headers.Contains(wwwAuthenticateHeader) &&
                        result.Headers.GetValues(wwwAuthenticateHeader).Any(x => x.Contains("expired"));

                    if (isExpiredBearerToken)
                    {
                        retry = false;
                        result.Dispose();
                        continue;
                    }

                    _logger?.LogDebug("OAuth Unauthorized");
                }

                try
                {
                    await result.EnsureSuccessApiStatusCode(_jsonSerializerOptions);

                    _logger?.LogDebug("Finished processing request for {uri}", uri);

#if DEBUG
                    try
                    {
                        var responseStr = await result.Content.ReadAsStringAsync(cancellationToken);
                        Debug.WriteLine($"Response string: {responseStr}");
                        using var jDoc = JsonDocument.Parse(responseStr);
                        Debug.WriteLine(
                            $"Response content: {JsonSerializer.Serialize(jDoc, new JsonSerializerOptions() { WriteIndented = true })}");
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine($"Failed to parse json {e.Message}");
                    }
#endif

                    // if empty response is expected, any non-related response is dropped
                    if (typeof(TResponse) == typeof(EmptyResponse))
                    {
                        // if not empty content, check what is there for debug purposes.
                        // C# EmptyContent class is internal, so checking it by the name
                        // for more details, see: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Net.Http/src/System/Net/Http/EmptyContent.cs
                        if (result.Content.GetType().Name != "EmptyContent")
                        {
                            _logger?.LogDebug("Expected empty content, but got {content}",
                                await result.Content.ReadAsStringAsync(cancellationToken));
                        }

                        return (TResponse)(object)new EmptyResponse();
                    }

                    // NOTE: there will probably be other files supported in the future
                    if (result.IsPdf())
                    {
                        if (typeof(TResponse) != typeof(ContentResult))
                        {
                            throw new InvalidOperationException(
                                $"Received pdf, but expected response type is not a {nameof(ContentResult)}.");
                        }

                        // Keep the PDF response streaming, but let the returned stream own the HTTP response.
                        // The wrapper exposes the stream to callers while disposing the response with the result.
                        // yes, the header currently returns double quotes ""IFOFJSLJ12313.pdf""
                        var fileName = result.Content.Headers.ContentDisposition?.FileName?.Trim('"');
                        var responseStream = await result.Content.ReadAsStreamAsync(cancellationToken);
                        var response = result;
                        result = null!;
                        return (TResponse)(object)new ContentResult()
                        {
                            Stream = new OwnedResponseStream(responseStream, response),
                            FileName = fileName
                        };
                    }

                    if (result.IsJson())
                    {
                        return await result.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken,
                                   options: _jsonSerializerOptions)
                               ?? throw new InvalidOperationException(
                                   $"{typeof(TResponse).Name} is null");
                    }

                    // unexpected content, log warning and throw exception
                    _logger?.LogWarning("Response is not json, but {content}",
                        await result.Content.ReadAsStringAsync(cancellationToken));

                    throw new InvalidOperationException("The response is not Json or EmptyResponse");
                }
                finally
                {
                    if (result is not null)
                    {
                        result.Dispose();
                    }
                }
            }
        }

        public JsonSerializerOptions JsonSerializerOptions => _jsonSerializerOptions;

        private static void AddOrOverrideHeaders(HttpRequestMessage msg,
            Dictionary<string, IEnumerable<string>> headers)
        {
            foreach (var header in headers)
            {
                if (msg.Headers.Contains(header.Key))
                {
                    msg.Headers.Remove(header.Key);
                }

                msg.Headers.Add(header.Key, header.Value);
            }
        }

        private async Task<(string token, bool retry)> Authenticate(
            HttpRequestMessage msg, bool retry,
            CancellationToken cancellationToken = default)
        {
            string token;
            // Due to all the additional params appSignAuth is requiring,
            // it's makes sense to still keep it in Http to manage all the details.
            // TODO: Refactoring as part of DEVEXP-1187, ApplicationSignedAuth breaks the ISinchAuth contract
            if (_auth.Value is ApplicationSignedAuth appSignAuth)
            {
                var now = DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture);
                const string headerName = "x-timestamp";
                msg.Headers.Add(headerName, now);

                var bytes = Array.Empty<byte>();
                if (msg.Content is not null)
                {
                    bytes = await msg.Content.ReadAsByteArrayAsync(cancellationToken);
                }

                if (msg.RequestUri is null)
                {
                    throw new NullReferenceException("HttpRequestMessage request uri is null");
                }

                token = appSignAuth.GetSignedAuth(
                    bytes,
                    msg.Method.ToString().ToUpperInvariant(), msg.RequestUri!.PathAndQuery,
                    $"{headerName}:{now}", msg.Content?.Headers.ContentType?.ToString());
                retry = false;
            }
            else
            {
                // try force get new token if retrying
                token = await _auth.Value.GetAuthToken(force: !retry);
            }

            return (token, retry);
        }

        public async Task<TResponse> Send<TRequest, TResponse>(Uri uri, HttpMethod httpMethod, TRequest? request,
            CancellationToken cancellationToken = default, Dictionary<string, IEnumerable<string>>? headers = null)
        {
            HttpContent? ContentFactory()
            {
                return request == null ? null : JsonContent.Create(request, options: _jsonSerializerOptions);
            }

            return await SendHttpContent<TResponse>(uri: uri, httpMethod: httpMethod, ContentFactory,
                cancellationToken: cancellationToken, headers: headers);
        }

        private static string BuildUserAgent()
        {
            var sdkVersion = new AssemblyName(typeof(Http).GetTypeInfo().Assembly.FullName!).Version!.ToString(3);
            var frameworkDescription = RuntimeInformation.FrameworkDescription;
            var runtimeIdentifier = RuntimeInformation.RuntimeIdentifier;

            return $"sinch-sdk/{sdkVersion} (csharp/{frameworkDescription}; {runtimeIdentifier};)";
        }

        private abstract class StreamWrapper : Stream
        {
            protected readonly Stream _inner;

            protected StreamWrapper(Stream inner)
            {
                _inner = inner;
            }

            public override bool CanRead => _inner.CanRead;
            public override bool CanSeek => _inner.CanSeek;
            public override bool CanWrite => _inner.CanWrite;
            public override long Length => _inner.Length;
            public override long Position
            {
                get => _inner.Position;
                set => _inner.Position = value;
            }

            public override void Flush() => _inner.Flush();
            public override int Read(byte[] buffer, int offset, int count) => _inner.Read(buffer, offset, count);
            public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) =>
                _inner.ReadAsync(buffer, offset, count, cancellationToken);
            public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default) =>
                _inner.ReadAsync(buffer, cancellationToken);
            public override long Seek(long offset, SeekOrigin origin) => _inner.Seek(offset, origin);
            public override void SetLength(long value) => _inner.SetLength(value);
            public override void Write(byte[] buffer, int offset, int count) => _inner.Write(buffer, offset, count);
        }

        /// <summary>
        ///     Wraps a caller-owned stream so that disposing the HttpContent built around it
        ///     (after a retry attempt) never disposes the caller's stream. Purely an
        ///     implementation detail of the stream-aware Send overload above.
        /// </summary>
        private sealed class NonDisposingStream : StreamWrapper
        {
            public NonDisposingStream(Stream inner) : base(inner)
            {
            }

            protected override void Dispose(bool disposing)
            {
                // Intentionally do not dispose the wrapped stream. The caller owns it.
            }
        }

        private sealed class OwnedResponseStream : StreamWrapper
        {
            private readonly HttpResponseMessage _owner;

            public OwnedResponseStream(Stream inner, HttpResponseMessage owner) : base(inner)
            {
                _owner = owner;
            }

            public override ValueTask DisposeAsync()
            {
                _owner.Dispose();
                return ValueTask.CompletedTask;
            }

            protected override void Dispose(bool disposing)
            {
                _owner.Dispose();
            }
        }
    }
}
