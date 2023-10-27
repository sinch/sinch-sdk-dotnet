using System;
using System.Net;
using System.Net.Http;
#if NET6_0_OR_GREATER
using System.Collections.Generic;
using System.Text.Json.Nodes;
#endif

namespace Sinch
{
    /// <summary>
    ///     Contains the information about failed request
    /// </summary>
    public sealed class ApiException : HttpRequestException
    {
        private ApiException(string message, Exception inner, HttpStatusCode statusCode) : base(message, inner,
            statusCode)
        {
#if NET6_0_OR_GREATER
            Details = new List<JsonNode>();
#endif
        }

        internal ApiException(HttpStatusCode statusCode, string message, Exception inner, ApiErrorResponse authApiError)
            : this($"{message}:{authApiError?.Error?.Message ?? authApiError?.Text}", inner, statusCode)
        {
            // https://developers.sinch.com/docs/sms/api-reference/status-codes/#4xx---user-errors
            // there can be nested error object or simple { text: "", code: "code" } not nested object with api errors
            // nested object takes precedence in fields population
            var details = authApiError?.Error;
            Status = details?.Status ?? authApiError?.Code;
            DetailedMessage = details?.Message ?? authApiError?.Text;

#if NET6_0_OR_GREATER
            Details = details?.Details ?? new List<JsonNode>();
#endif
        }

        public string DetailedMessage { get; init; }

        public string Status { get; init; }

#if NET6_0_OR_GREATER
        public List<JsonNode> Details { get; init; }
#endif
    }
}
