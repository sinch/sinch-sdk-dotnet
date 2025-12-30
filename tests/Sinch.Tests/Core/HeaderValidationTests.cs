using System.Net.Http;
using FluentAssertions;
using Sinch.Core;
using Xunit;

namespace Sinch.Tests.Core
{
    public class AuthHeaderValidationTests
    {
        private void AssertAuthHeader(string secret, string json, string headerValue, bool isValid)
        {
            HeaderValidation.ValidateAuthHeader(secret, json, headerValue).Should().Be(isValid);

            var headers = new HttpRequestMessage().Headers;
            headers.Add("x-sinch-signature", headerValue);
            HeaderValidation.ValidateAuthHeader(secret, json, headers).Should().Be(isValid);
        }

        private const string JsonBody =
            "{\"eventId\":\"01hpa0mww4m79q8j2dwn3ggbgz\",\"timestamp\":\"2024-02-10T17:22:09.412722588\",\"projectId\":\"37b62a7b-0177-abcd-efgh-e10f848de123\",\"resourceId\":\"+17818510001\",\"resourceType\":\"ACTIVE_NUMBER\",\"eventType\":\"DEPROVISIONING_FROM_VOICE_PLATFORM\",\"status\":\"SUCCEEDED\",\"failureCode\":null}";
        private const string XSinchSignatureHeaderValue = "dcf80daac2f232f4b23d5f719db3aa0fdca6af43";
        private const string CallbackSecret = "callback-secret";

        [Fact]
        public void ShouldValidateAuthHeader()
        {
            AssertAuthHeader(CallbackSecret, JsonBody, XSinchSignatureHeaderValue, isValid: true);
        }

        [Theory]
        [InlineData(CallbackSecret, null, null)]
        [InlineData(null, JsonBody, null)]
        [InlineData(null, null, XSinchSignatureHeaderValue)]
        [InlineData(null, JsonBody, XSinchSignatureHeaderValue)]
        [InlineData(CallbackSecret, null, XSinchSignatureHeaderValue)]
        [InlineData(CallbackSecret, JsonBody, null)]
        [InlineData(null, null, null)]
        public void ShouldFailValidation(string callbackSecret, string json, string authHeaderValue)
        {
            AssertAuthHeader(callbackSecret, json, authHeaderValue, isValid: false);
            // also test with empty string 
            AssertAuthHeader(callbackSecret ?? string.Empty, json ?? string.Empty, authHeaderValue ?? string.Empty,
                isValid: false);
        }
    }
}
