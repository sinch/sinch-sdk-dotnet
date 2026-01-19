using FluentAssertions;
using Sinch.Conversation.Apps;
using Xunit;

namespace Sinch.Tests.Conversation.Apps
{
    public sealed class LineEnterpriseCredentialsTests : ConversationTestBase
    {
        [Fact]
        public void DeserializeLineEnterpriseCredentialsWithLineJapan()
        {
            var json = Helpers.LoadResources("Conversation/Apps/LineEnterpriseCredentialsWithLineJapan.json");

            var result = DeserializeAsConversationClient<LineEnterpriseCredentials>(json);

            result.Should().BeEquivalentTo(new LineEnterpriseCredentials(
                new LineJapanEnterpriseCredentials
                {
                    Token = "japan token value",
                    Secret = "japan secret value",
                    IsDefault = true
                }));
        }
    }
}

