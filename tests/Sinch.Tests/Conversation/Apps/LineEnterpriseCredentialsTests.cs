using System.Text.Json;
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

            var result = JsonSerializer.Deserialize<LineEnterpriseCredentials>(json, Conversation.JsonSerializerOptions);

            result.Should().BeEquivalentTo(new LineEnterpriseCredentials(
                new LineJapanEnterpriseCredentials
                {
                    Token = "japan token value",
                    Secret = "japan secret value",
                    IsDefault = true
                }));
        }

        [Fact]
        public void SerializeLineEnterpriseCredentialsWithLineJapan()
        {
            var lineEnterpriseCredentials = new LineEnterpriseCredentials(
                new LineJapanEnterpriseCredentials
                {
                    Token = "japan token value",
                    Secret = "japan secret value",
                    IsDefault = true
                });

            var actual = JsonSerializer.Serialize(lineEnterpriseCredentials, Conversation.JsonSerializerOptions);

            var expected = Helpers.LoadResources("Conversation/Apps/LineEnterpriseCredentialsWithLineJapan.json");

            Helpers.AssertJsonEqual(expected, actual);
        }
    }
}

