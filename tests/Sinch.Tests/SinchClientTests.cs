using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Sinch.Conversation;
using Sinch.SMS;
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
                },
                SmsConfiguration = new SinchSmsConfiguration() { Region = SmsRegion.Us },
                ConversationConfiguration = new SinchConversationConfiguration() { Region = ConversationRegion.Us }
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
        public void SmsWithServicePlanId_DoesNotRequireUnifiedCredentials()
        {
            var sinch = new SinchClient(new SinchClientConfiguration()
            {
                SmsConfiguration = SinchSmsConfiguration.WithServicePlanId("servicePlanId", "apiToken", SmsServicePlanIdRegion.Us)
            });
            sinch.Sms.Should().NotBeNull();
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
                },
                SmsConfiguration = new SinchSmsConfiguration() { Region = SmsRegion.Us },
                ConversationConfiguration = new SinchConversationConfiguration() { Region = ConversationRegion.Us }
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

        [Fact]
        public void Numbers_SinchEvents_ParseEvent_DoesNotRequireCredentials()
        {
            var sinch = new SinchClient();
            var json = Helpers.LoadResources("Numbers/SinchEvents/NumberSinchEvent.json");

            var sinchEvent = sinch.Numbers.SinchEvents.ParseEvent(json);

            sinchEvent.Should().NotBeNull();
            sinchEvent.EventId.Should().Be("abcd1234efghijklmnop567890");
        }

        [Fact]
        public void Verification_SinchEvents_ParseEvent_DoesNotRequireConfiguration()
        {
            var sinch = new SinchClient();
            var json = Helpers.LoadResources("Verification/SinchEvents/VerificationStartEvent.json");

            var sinchEvent = sinch.Verification.SinchEvents.ParseEvent(json);

            sinchEvent.Should().BeOfType<Sinch.Verification.SinchEvents.VerificationStartEvent>();
        }

        [Fact]
        public void Verification_SinchEvents_SerializeResponse_DoesNotRequireConfiguration()
        {
            var sinch = new SinchClient();
            var expected = Helpers.LoadResources("Verification/SinchEvents/VerificationStartEventResponseSms.json");
            var response = new Sinch.Verification.SinchEvents.VerificationStartEventResponseSms
            {
                Action = Sinch.Verification.SinchEvents.Action.Allow,
                Sms = new Sinch.Verification.SinchEvents.Sms
                {
                    Code = "123",
                    AcceptLanguage = new List<string> { "en-US" }
                }
            };

            var json = sinch.Verification.SinchEvents.SerializeResponse(response);

            Helpers.AssertJsonEqual(expected, json);
        }
    }
}
