using System;
using System.Collections.Generic;
using FluentAssertions;
using Sinch.Conversation.EventDestinations;
using Xunit;

namespace Sinch.Tests.Conversation.Webhooks
{
    public class HmacAuthenticationValidationTests
    {
        private const string TimestampHeader = "x-sinch-webhook-signature-timestamp";
        private const string NonceHeader = "x-sinch-webhook-signature-nonce";
        private const string AlgorithmHeader = "x-sinch-webhook-signature-algorithm";
        private const string SignatureHeader = "x-sinch-webhook-signature";

        // Valid fixture, all tests that expect true use these values
        private const string ValidSecret = "my_secret_key";
        private const string ValidJsonPayload = "{\"event\":\"test\"}";
        private const string ValidTimestamp = "1736760161";
        private const string ValidNonce = "01JHFFHWYY7HSS4FWTMDTQEK8V";
        private const string ValidAlgorithm = "HmacSHA256";
        private const string ValidSignature = "ehhrg9MUuhpJUCo2drsI3zoqWViaojp8d6RahfBY3cg=";

        [Fact]
        public void ValidateAuthenticationHeader_WithSingleValueHeaders_ReturnsTrue()
        {
            IDictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                [TimestampHeader] = ValidTimestamp,
                [NonceHeader] = ValidNonce,
                [AlgorithmHeader] = ValidAlgorithm,
                [SignatureHeader] = ValidSignature
            };

            var result = HmacAuthenticationValidation.ValidateAuthenticationHeader(ValidSecret, headers, ValidJsonPayload);

            result.Should().BeTrue();
        }

        [Fact]
        public void ValidateAuthenticationHeader_WithCaseInsensitiveHeaders_ReturnsTrue()
        {
            var headers = new Dictionary<string, IEnumerable<string>>
            {
                [TimestampHeader.ToUpperInvariant()] = [ValidTimestamp],
                [NonceHeader.ToUpperInvariant()] = [ValidNonce],
                [AlgorithmHeader.ToUpperInvariant()] = [ValidAlgorithm],
                [SignatureHeader.ToUpperInvariant()] = [ValidSignature]
            };

            var result = HmacAuthenticationValidation.ValidateAuthenticationHeader(ValidSecret, headers, ValidJsonPayload);

            result.Should().BeTrue();
        }

        [Fact]
        public void ValidateAuthenticationHeader_WithInvalidSignature_ReturnsFalse()
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                [TimestampHeader] = ValidTimestamp,
                [NonceHeader] = ValidNonce,
                [AlgorithmHeader] = ValidAlgorithm,
                [SignatureHeader] = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA="
            };

            var result = HmacAuthenticationValidation.ValidateAuthenticationHeader(ValidSecret, headers, ValidJsonPayload);

            result.Should().BeFalse();
        }

        [Fact]
        public void ValidateAuthenticationHeader_WithUnsupportedAlgorithm_ThrowsException()
        {
            const string unsupportedAlgorithm = "HmacSHA512";

            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                [TimestampHeader] = ValidTimestamp,
                [NonceHeader] = ValidNonce,
                [AlgorithmHeader] = unsupportedAlgorithm,
                [SignatureHeader] = ValidSignature
            };

            Action act = () => HmacAuthenticationValidation.ValidateAuthenticationHeader(ValidSecret, headers, ValidJsonPayload);

            act.Should().Throw<NotSupportedException>().WithMessage("*Unsupported HMAC algorithm*");
        }

        [Theory]
        [InlineData(SignatureHeader)]
        [InlineData(TimestampHeader)]
        [InlineData(NonceHeader)]
        [InlineData(AlgorithmHeader)]
        public void ValidateAuthenticationHeader_MissingHeader_ReturnsFalse(string missingHeader)
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                [TimestampHeader] = ValidTimestamp,
                [NonceHeader] = ValidNonce,
                [AlgorithmHeader] = ValidAlgorithm,
                [SignatureHeader] = ValidSignature
            };

            headers.Remove(missingHeader);

            var result = HmacAuthenticationValidation.ValidateAuthenticationHeader(ValidSecret, headers, ValidJsonPayload);

            result.Should().BeFalse();
        }

        [Fact]
        public void ValidateAuthenticationHeader_EmptySecret_ReturnsFalse()
        {
            var emptySecret = string.Empty;

            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                [TimestampHeader] = ValidTimestamp,
                [NonceHeader] = ValidNonce,
                [AlgorithmHeader] = ValidAlgorithm,
                [SignatureHeader] = ValidSignature
            };

            var result = HmacAuthenticationValidation.ValidateAuthenticationHeader(emptySecret, headers, ValidJsonPayload);

            result.Should().BeFalse();
        }
    }
}
