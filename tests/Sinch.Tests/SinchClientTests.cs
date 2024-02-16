using System;
using System.Net.Http;
using FluentAssertions;
using Xunit;

namespace Sinch.Tests
{
    public class SinchClientTests
    {
        [Fact]
        public void InitSinchClientWithoutAllCredentials()
        {
            var sinch = new SinchClient(null, null, null);
            sinch.Should().NotBeNull();
        }

        [Fact]
        public void InitSinchClientWithoutProjectId()
        {
            var sinch = new SinchClient(null, "key", "secret");
            sinch.Should().NotBeNull();
        }

        [Fact]
        public void InitSinchClientWithoutKeyId()
        {
            var sinch = new SinchClient("key", null, "secret");
            sinch.Should().NotBeNull();
        }

        [Fact]
        public void InitSinchClientWithoutKeySecret()
        {
            var sinch = new SinchClient("key", "secret", null);
            sinch.Should().NotBeNull();
        }

        [Fact]
        public void InitSinchClientWithCustomHttpClient()
        {
            var httpClient = new HttpClient();
            var sinch = new SinchClient("TEST_PROJECT_ID", "TEST_KEY", "TEST_KEY_SECRET",
                options => { options.HttpClient = httpClient; });
            sinch.Should().NotBeNull();
            Helpers.GetPrivateField<HttpClient, SinchClient>(sinch, "_httpClient").Should().Be(httpClient);
        }

        [Theory]
        [InlineData(null, null, null,
            "Credentials are missing (keyId should have a value) (projectId should have a value) (keySecret should have a value)")]
        [InlineData("projectId", null, null,
            "Credentials are missing (keyId should have a value) (keySecret should have a value)")]
        [InlineData(null, "keyId", null,
            "Credentials are missing (projectId should have a value) (keySecret should have a value)")]
        [InlineData(null, null, "keySecret",
            "Credentials are missing (keyId should have a value) (projectId should have a value)")]
        [InlineData("projectId", "keySecret", null, "Credentials are missing (keySecret should have a value)")]
        [InlineData("projectId", null, "keySecret", "Credentials are missing (keyId should have a value)")]
        [InlineData(null, "keySecret", "keySecret", "Credentials are missing (projectId should have a value)")]
        public void ThrowAggregateExceptionWhenAccessingCommonCredentialsProducts(string projectId, string keyId,
            string keySecret, string message)
        {
            var sinch = new SinchClient(projectId, keyId, keySecret);
            var smsOp = () => sinch.Sms;
            var aggregateExceptionSms = smsOp.Should().Throw<AggregateException>().Which;
            aggregateExceptionSms.Message.Should().BeEquivalentTo(message);

            var conversationOp = () => sinch.Conversation;
            var aggregateExceptionConversation = conversationOp.Should().Throw<AggregateException>().Which;
            aggregateExceptionConversation.Message.Should().BeEquivalentTo(message);
            
            var numbersOp = () => sinch.Numbers;
            var aggregateExceptionNumbers = numbersOp.Should().Throw<AggregateException>().Which;
            aggregateExceptionNumbers.Message.Should().BeEquivalentTo(message);
            
            var authOp = () => sinch.Auth;
            var aggregateExceptionAuth = authOp.Should().Throw<AggregateException>().Which;
            aggregateExceptionAuth.Message.Should().BeEquivalentTo(message);
        }
        [Fact]
        public void InitializeOwnHttpIfNotPassed()
        {
            var sinch = new SinchClient("proj", "id", "secret");
            Helpers.GetPrivateField<HttpClient, SinchClient>(sinch, "_httpClient").Should().NotBeNull();
        }
    }
}
