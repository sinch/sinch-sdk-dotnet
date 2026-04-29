using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Sinch.SMS.SinchEvents;
using Xunit;

namespace Sinch.Tests.Sms
{
    public class HmacAuthenticationValidationTests
    {
        [Fact]
        public void ValidateAuthenticationHeader_WithSupportedAlgorithm_ReturnsTrue()
        {
            // Arrange
            const string secret = "my_secret_key";
            const string jsonPayload = "{\"event\":\"test\"}";
            const string timestamp = "1736760161";
            const string nonce = "01JHFFHWYY7HSS4FWTMDTQEK8V";
            const string algorithm = "HmacSHA256";

            const string toBeSigned = $"{jsonPayload}.{nonce}.{timestamp}";
            using var hmac = new System.Security.Cryptography.HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(toBeSigned));
            var signature = Convert.ToBase64String(hash);

            var headers = new Dictionary<string, IEnumerable<string>>(StringComparer.OrdinalIgnoreCase)
            {
                ["x-sinch-webhook-signature-timestamp"] = [timestamp],
                ["x-sinch-webhook-signature-nonce"] = [nonce],
                ["x-sinch-webhook-signature-algorithm"] = [algorithm],
                ["x-sinch-webhook-signature"] = [signature]
            };

            // Act
            var result = HmacAuthenticationValidation.ValidateAuthenticationHeader(secret, headers, jsonPayload);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void ValidateAuthenticationHeader_WithUnsupportedAlgorithm_ThrowsException()
        {
            // Arrange
            const string secret = "some_secret";
            const string jsonPayload = "{}";
            const string timestamp = "1234567890";
            const string nonce = "nonce-value";
            const string algorithm = "HmacSHA512"; // unsupported

            var headers = new Dictionary<string, IEnumerable<string>>(StringComparer.OrdinalIgnoreCase)
            {
                ["x-sinch-webhook-signature-timestamp"] = [timestamp],
                ["x-sinch-webhook-signature-nonce"] = [nonce],
                ["x-sinch-webhook-signature-algorithm"] = [algorithm],
                ["x-sinch-webhook-signature"] = ["whatever"]
            };

            // Act
            Action act = () => HmacAuthenticationValidation.ValidateAuthenticationHeader(secret, headers, jsonPayload);

            // Assert
            act.Should().Throw<NotSupportedException>().WithMessage("*Unsupported HMAC algorithm*");
        }

        [Theory]
        [MemberData(nameof(MissingHeaderCases))]
        public void ValidateAuthenticationHeader_MissingHeader_ReturnsFalse(string missingHeader)
        {
            // Arrange
            const string secret = "secret";
            const string jsonPayload = "{}";

            var headers = new Dictionary<string, IEnumerable<string>>(StringComparer.OrdinalIgnoreCase)
            {
                ["x-sinch-webhook-signature-timestamp"] = ["123"],
                ["x-sinch-webhook-signature-nonce"] = ["n"],
                ["x-sinch-webhook-signature-algorithm"] = ["HmacSHA256"],
                ["x-sinch-webhook-signature"] = ["s"]
            };

            // Remove the header under test
            headers.Remove(missingHeader);

            // Act
            var result = HmacAuthenticationValidation.ValidateAuthenticationHeader(secret, headers, jsonPayload);

            // Assert
            result.Should().BeFalse();
        }

        public static System.Collections.Generic.IEnumerable<object[]> MissingHeaderCases()
        {
            yield return ["x-sinch-webhook-signature"];
            yield return ["x-sinch-webhook-signature-timestamp"];
            yield return ["x-sinch-webhook-signature-nonce"];
            yield return ["x-sinch-webhook-signature-algorithm"];
        }

        [Fact]
        public void ValidateAuthenticationHeader_EmptySecret_ReturnsFalse()
        {
            // Arrange
            const string secret = "";
            const string jsonPayload = "{}";
            const string timestamp = "123";
            const string nonce = "n";
            const string algorithm = "HmacSHA256";

            var headers = new Dictionary<string, IEnumerable<string>>(StringComparer.OrdinalIgnoreCase)
            {
                ["x-sinch-webhook-signature-timestamp"] = [timestamp],
                ["x-sinch-webhook-signature-nonce"] = [nonce],
                ["x-sinch-webhook-signature-algorithm"] = [algorithm],
                ["x-sinch-webhook-signature"] = ["sig"]
            };

            // Act
            var result = HmacAuthenticationValidation.ValidateAuthenticationHeader(secret, headers, jsonPayload);

            // Assert
            result.Should().BeFalse();
        }
    }
}
