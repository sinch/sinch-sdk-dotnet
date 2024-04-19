using System;
using System.Net;
using System.Net.Http;

namespace Sinch.Auth
{
    public sealed class SinchAuthException : HttpRequestException
    {
        private SinchAuthException(HttpStatusCode statusCode, string? message, Exception? inner) : base(message, inner,
            statusCode)
        {
        }

        internal SinchAuthException(HttpStatusCode statusCode, string? message, Exception? inner, AuthApiError? authApiError)
            : this(statusCode, message, inner)
        {
            Error = authApiError?.Error;
            ErrorDescription = authApiError?.ErrorDescription;
            ErrorHint = authApiError?.ErrorHint;
            ErrorVerbose = authApiError?.ErrorVerbose;
        }

        public string? Error { get; }

        public string? ErrorVerbose { get; }

        public string? ErrorDescription { get; }

        public string? ErrorHint { get; }
    }
}
