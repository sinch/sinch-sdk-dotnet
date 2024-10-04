using System;
using System.Net.Http;
using FluentAssertions;
using Sinch.Conversation;
using Xunit;

namespace Sinch.Tests
{
    public class SinchClientTests
    {
        [Theory]
        [InlineData(null, null, null)]
        [InlineData("projectId", null, null)]
        [InlineData(null, "keyId", null)]
        [InlineData(null, null, "keySecret")]
        [InlineData("projectId", "keySecret", null)]
        [InlineData("projectId", null, "keySecret")]
        [InlineData(null, "keySecret", "keySecret")]
        public void InitSinchClientWithoutCredentials(string projectId, string keyId, string keySecret)
        {
            var sinch = new SinchClient(projectId, keyId, keySecret);
            sinch.Should().NotBeNull();
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
        public void GetServiceWithoutExceptionsIfCredentialsAreSet()
        {
            var sinch = new SinchClient("projectId", "keyId", "keySecret");
            sinch.Conversation.Should().NotBeNull();
            sinch.Sms.Should().NotBeNull();
            sinch.Auth.Should().NotBeNull();
            sinch.Numbers.Should().NotBeNull();
        }

        [Fact]
        public void InitializeOwnHttpIfNotPassed()
        {
            var sinch = new SinchClient("proj", "id", "secret");
            Helpers.GetPrivateField<HttpClient, SinchClient>(sinch, "_httpClient").Should().NotBeNull();
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
    }
}
