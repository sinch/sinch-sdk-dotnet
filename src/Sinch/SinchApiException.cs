using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json.Nodes;

namespace Sinch
{
    /// <summary>
    ///     Contains the information about failed request
    /// </summary>
    public sealed class SinchApiException : HttpRequestException
    {
        private SinchApiException(string message, Exception inner, HttpStatusCode statusCode) : base(message, inner,
            statusCode)
        {
            Details = new List<JsonNode>();
        }

        internal SinchApiException(HttpStatusCode statusCode, string message, Exception inner,
            ApiErrorResponse authApiError)
            : this($"{message}:{authApiError?.Error?.Message ?? authApiError?.Text}", inner, statusCode)
        {
            // https://developers.sinch.com/docs/sms/api-reference/status-codes/#4xx---user-errors
            // there can be nested error object or simple { text: "", code: "code" } not nested object with api errors
            // nested object takes precedence in fields population
            var details = authApiError?.Error;
            Status = details?.Status ?? authApiError?.Code;
            DetailedMessage = details?.Message ?? authApiError?.Text;
            Details = details?.Details ?? new List<JsonNode>();
        }

        public string? DetailedMessage { get; init; }

        public string? Status { get; init; }

        public List<JsonNode> Details { get; init; }
    }
}
