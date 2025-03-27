using FluentAssertions;
using Newtonsoft.Json.Linq;
using Sinch.Conversation;
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
            var request = new ConversationChannelCredentials(
                new LineThailandEnterpriseCredentials()
                {
                    Token = "line enterprise credentials thailand token value",
                    Secret = "line enterprise credentials thailand secret value",
                    IsDefault = true
                }
            )
            {
                Channel = ConversationChannel.Line,
                CallbackSecret = "callback secret",
                CredentialOrdinalNumber = 1,
            };


            var actual = SerializeAsConversationClient(request);

            var expected = Helpers.LoadResources(
                "Conversation/Apps/ConversationChannelCredentials/LineThailandEnterprise.json");
            Helpers.AssertJsonEqual(expected, actual);
        }

        [Fact]
        public void SerializeConversationChannelCredentialsLineJapanEnterprise()
        {
            var request = new ConversationChannelCredentials(
                new LineJapanEnterpriseCredentials
                {
                    Token = "line enterprise credentials japan token value",
                    Secret = "line enterprise credentials japan secret value",
                    IsDefault = true
                })
            {
                Channel = ConversationChannel.Line,
                CallbackSecret = "callback secret",
                CredentialOrdinalNumber = 1,
                ChannelKnownId = "channel id",
                State = new ChannelIntegrationState
                {
                    Status = ChannelIntegrationStatus.Pending,
                    Description = "description value"
                }
            };


            var actual = SerializeAsConversationClient(request);

            var expected = Helpers.LoadResources(
                "Conversation/Apps/ConversationChannelCredentials/LineJapanEnterprise.json");
            Helpers.AssertJsonEqual(actual, expected);
        }

        [Fact]
        public void SerializeLineCredentials()
        {
            var lineCredentials = new ConversationChannelCredentials(new LineCredentials()
            {
                Token = "lineChannel a token value",
                Secret = "lineChannel a secret value",
                IsDefault = true
            })
            {
                Channel = ConversationChannel.Line,
                CallbackSecret = "callback secret",
                CredentialOrdinalNumber = 1,
            };

            var actual = SerializeAsConversationClient(lineCredentials);

            Helpers.AssertJsonEqual(Helpers.LoadResources(
                "Conversation/Apps/ConversationChannelCredentials/Line.json"), actual);
        }
    }
}
