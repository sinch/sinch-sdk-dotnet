using FluentAssertions;
using Sinch.Conversation.Apps;
using Sinch.Conversation.Apps.Credentials;
using Xunit;

namespace Sinch.Tests.Conversation.Apps
{
    public sealed class ChannelCredentialsTests : ConversationTestBase
    {
        [Fact]
        public void SerializeConversationChannelCredentialsLineThailandEnterprise()
        {
            var request = WithCallbackSecretAndOrdinal(
                ConversationChannelCredentialsBuilderFactory.LineEnterprise(
                    new LineThailandEnterpriseCredentials()
                    {
                        Token = "line enterprise credentials thailand token value",
                        Secret = "line enterprise credentials thailand secret value",
                        IsDefault = true
                    }),
                "callback secret",
                1);

            var actual = SerializeAsConversationClient(request);

            var expected = Helpers.LoadResources(
                "Conversation/Apps/ConversationChannelCredentials/LineThailandEnterprise.json");
            Helpers.AssertJsonEqual(expected, actual);
        }

        [Fact]
        public void SerializeConversationChannelCredentialsLineJapanEnterprise()
        {
            var request = WithCallbackSecretOrdinalAndState(
                ConversationChannelCredentialsBuilderFactory.LineEnterprise(
                    new LineJapanEnterpriseCredentials
                    {
                        Token = "line enterprise credentials japan token value",
                        Secret = "line enterprise credentials japan secret value",
                        IsDefault = true
                    }),
                "callback secret",
                1,
                "channel id",
                new ChannelIntegrationState
                {
                    Status = ChannelIntegrationStatus.Pending,
                    Description = "description value"
                });
            
            var actual = SerializeAsConversationClient(request);

            var expected = Helpers.LoadResources(
                "Conversation/Apps/ConversationChannelCredentials/LineJapanEnterprise.json");
            Helpers.AssertJsonEqual(actual, expected);
        }

        [Fact]
        public void SerializeLineCredentials()
        {
            var lineCredentials = WithCallbackSecretAndOrdinal(
                ConversationChannelCredentialsBuilderFactory.Line(new LineCredentials()
                {
                    Token = "lineChannel a token value",
                    Secret = "lineChannel a secret value",
                    IsDefault = true
                }),
                "callback secret",
                1);

            var actual = SerializeAsConversationClient(lineCredentials);

            Helpers.AssertJsonEqual(Helpers.LoadResources(
                "Conversation/Apps/ConversationChannelCredentials/Line.json"), actual);
        }

        [Fact]
        public void DeserializeLineThailandEnterprise()
        {
            var json = Helpers.LoadResources(
                "Conversation/Apps/ConversationChannelCredentials/LineThailandEnterprise.json");

            var result = DeserializeAsConversationClient<ConversationChannelCredentials>(json);

            result.Should().BeEquivalentTo(WithCallbackSecretAndOrdinal(
                ConversationChannelCredentialsBuilderFactory.LineEnterprise(
                    new LineThailandEnterpriseCredentials()
                    {
                        Token = "line enterprise credentials thailand token value",
                        Secret = "line enterprise credentials thailand secret value",
                        IsDefault = true
                    }),
                "callback secret",
                1));
        }

        [Fact]
        public void DeserializeLineJapanEnterprise()
        {
            var json = Helpers.LoadResources(
                "Conversation/Apps/ConversationChannelCredentials/LineJapanEnterprise.json");

            var result = DeserializeAsConversationClient<ConversationChannelCredentials>(json);

            result.Should().BeEquivalentTo(WithCallbackSecretOrdinalAndState(
                ConversationChannelCredentialsBuilderFactory.LineEnterprise(
                    new LineJapanEnterpriseCredentials
                    {
                        Token = "line enterprise credentials japan token value",
                        Secret = "line enterprise credentials japan secret value",
                        IsDefault = true
                    }),
                "callback secret",
                1,
                "channel id",
                new ChannelIntegrationState
                {
                    Status = ChannelIntegrationStatus.Pending,
                    Description = "description value"
                }));
        }

        [Fact]
        public void DeserializeLine()
        {
            var json = Helpers.LoadResources(
                "Conversation/Apps/ConversationChannelCredentials/Line.json");

            var result = DeserializeAsConversationClient<ConversationChannelCredentials>(json);

            result.Should().BeEquivalentTo(WithCallbackSecretAndOrdinal(
                ConversationChannelCredentialsBuilderFactory.Line(new LineCredentials()
                {
                    Token = "lineChannel a token value",
                    Secret = "lineChannel a secret value",
                    IsDefault = true
                }),
                "callback secret",
                1));
        }

        private static ConversationChannelCredentials WithCallbackSecretAndOrdinal(
            ConversationChannelCredentials credentials,
            string callbackSecret,
            int credentialOrdinalNumber)
        {
            credentials.CallbackSecret = callbackSecret;
            credentials.CredentialOrdinalNumber = credentialOrdinalNumber;
            return credentials;
        }

        private static ConversationChannelCredentials WithCallbackSecretOrdinalAndState(
            ConversationChannelCredentials credentials,
            string callbackSecret,
            int credentialOrdinalNumber,
            string channelKnownId,
            ChannelIntegrationState state)
        {
            credentials.CallbackSecret = callbackSecret;
            credentials.CredentialOrdinalNumber = credentialOrdinalNumber;
            credentials.ChannelKnownId = channelKnownId;
            credentials.State = state;
            return credentials;
        }
    }
}
