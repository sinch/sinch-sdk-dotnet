using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
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
            var sinch = new SinchClient(new SinchClientConfiguration()
            {
                SinchUnifiedCredentials = new SinchUnifiedCredentials()
                {
                    ProjectId = projectId,
                    KeyId = keyId,
                    KeySecret = keySecret,
                }
            });
            sinch.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null, null, null,
            "Credentials are missing (ProjectId should have a value) (KeyId should have a value) (KeySecret should have a value)")]
        [InlineData("projectId", null, null,
            "Credentials are missing (KeyId should have a value) (KeySecret should have a value)")]
        [InlineData(null, "keyId", null,
            "Credentials are missing (ProjectId should have a value) (KeySecret should have a value)")]
        [InlineData(null, null, "keySecret",
            "Credentials are missing (ProjectId should have a value) (KeyId should have a value)")]
        [InlineData("projectId", "keySecret", null, "Credentials are missing (KeySecret should have a value)")]
        [InlineData("projectId", null, "keySecret", "Credentials are missing (KeyId should have a value)")]
        [InlineData(null, "keySecret", "keySecret", "Credentials are missing (ProjectId should have a value)")]
        public async Task ThrowAggregateExceptionWhenAccessingUnifiedCredentialsProducts(string projectId, string keyId,
            string keySecret, string message)
        {
            var sinch = new SinchClient(new SinchClientConfiguration()
            {
                SinchUnifiedCredentials = new SinchUnifiedCredentials()
                {
                    ProjectId = projectId,
                    KeyId = keyId,
                    KeySecret = keySecret,
                }
            });
            var smsOp = () => sinch.Sms.Batches.Get("1");
            var aggregateExceptionSms = (await smsOp.Should().ThrowAsync<AggregateException>()).Which;
            aggregateExceptionSms.Message.Should().BeEquivalentTo(message);

            var conversationOp = () => sinch.Conversation.Messages.Get("1");
            var aggregateExceptionConversation = (await conversationOp.Should().ThrowAsync<AggregateException>()).Which;
            aggregateExceptionConversation.Message.Should().BeEquivalentTo(message);

            var numbersOp = () => sinch.Numbers.Get("+31231321");
            var aggregateExceptionNumbers = (await numbersOp.Should().ThrowAsync<AggregateException>()).Which;
            aggregateExceptionNumbers.Message.Should().BeEquivalentTo(message);

            var authOp = () => sinch.Auth;
            var aggregateExceptionAuth = authOp.Should().Throw<AggregateException>().Which;
            aggregateExceptionAuth.Message.Should().BeEquivalentTo(message);
        }

        [Fact]
        public void GetServiceWithoutExceptionsIfCredentialsAreSet()
        {
            var sinch = new SinchClient(new SinchClientConfiguration()
            {
                SinchUnifiedCredentials = new SinchUnifiedCredentials()
                {
                    ProjectId = "projectid",
                    KeyId = "keyid",
                    KeySecret = "keysecret",
                }
            });
            sinch.Conversation.Should().NotBeNull();
            sinch.Sms.Should().NotBeNull();
            sinch.Auth.Should().NotBeNull();
            sinch.Numbers.Should().NotBeNull();
        }

        [Fact]
        public void InitializeOwnHttpClientFactoryIfNotPassed()
        {
            var sinch = new SinchClient(new SinchClientConfiguration()
            {
                SinchUnifiedCredentials = new SinchUnifiedCredentials()
                {
                    ProjectId = "projectid",
                    KeyId = "keyid",
                    KeySecret = "keysecret",
                }
            });
            Helpers.GetPrivateField<Func<HttpClient>, SinchClient>(sinch, "_httpClientAccessor").Should().NotBeNull();
        }

        [Fact]
        public void InitSinchClientWithCustomHttpClientFactory()
        {
            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            var sinch = new SinchClient(new SinchClientConfiguration()
            {
                SinchOptions = new SinchOptions()
                {
                    HttpClientFactory = httpClientFactory,
                }
            });
            sinch.Should().NotBeNull();
            var accessor = Helpers.GetPrivateField<Func<HttpClient>, SinchClient>(sinch, "_httpClientAccessor");
            accessor.Should().NotBeNull();
        }
    }
}
