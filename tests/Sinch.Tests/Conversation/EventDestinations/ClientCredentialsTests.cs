using System.Text.Json;
using FluentAssertions;
using Sinch.Conversation.EventDestinations;
using Xunit;

namespace Sinch.Tests.Conversation.EventDestinations
{
    public class ClientCredentialsTests : ConversationTestBase
    {
        [Fact]
        public void DeserializeClientCredentialsWithAllFields()
        {
            var json = Helpers.LoadResources("Conversation/EventDestinations/ClientCredentialsWithAllFields.json");
            var result = JsonSerializer.Deserialize<ClientCredentials>(json, Conversation.JsonSerializerOptions);

            result.Should().NotBeNull();
            result!.ClientId.Should().Be("test-client-id");
            result.ClientSecret.Should().Be("test-client-secret");
            result.Endpoint.Should().Be("https://example.com/oauth/token");
            result.Scope.Should().Be("read write");
            result.ResponseType.Should().Be("code");
            result.TokenRequestType.Should().Be("BASIC");
        }

        [Fact]
        public void DeserializeClientCredentialsWithRequiredFieldsOnly()
        {
            var json = Helpers.LoadResources("Conversation/EventDestinations/ClientCredentialsWithRequiredFieldsOnly.json");
            var result = JsonSerializer.Deserialize<ClientCredentials>(json, Conversation.JsonSerializerOptions);

            result.Should().NotBeNull();
            result!.ClientId.Should().Be("test-client-id");
            result.ClientSecret.Should().Be("test-client-secret");
            result.Endpoint.Should().Be("https://example.com/oauth/token");
            result.Scope.Should().BeNull();
            result.ResponseType.Should().BeNull();
            result.TokenRequestType.Should().BeNull();
        }

        [Fact]
        public void DeserializeClientCredentialsWithPartialOptionalFields()
        {
            var json = Helpers.LoadResources("Conversation/EventDestinations/ClientCredentialsWithScope.json");
            var result = JsonSerializer.Deserialize<ClientCredentials>(json, Conversation.JsonSerializerOptions);

            result.Should().NotBeNull();
            result!.ClientId.Should().Be("test-client-id");
            result.ClientSecret.Should().Be("test-client-secret");
            result.Endpoint.Should().Be("https://example.com/oauth/token");
            result.Scope.Should().Be("read write");
            result.ResponseType.Should().BeNull();
            result.TokenRequestType.Should().BeNull();
        }

        [Fact]
        public void SerializeClientCredentialsRoundTrip()
        {
            var original = new ClientCredentials
            {
                ClientId = "test-client-id",
                ClientSecret = "test-client-secret",
                Endpoint = "https://example.com/oauth/token",
                Scope = "read write",
                ResponseType = "code",
                TokenRequestType = "BASIC"
            };

            var json = JsonSerializer.Serialize(original, Conversation.JsonSerializerOptions);
            var deserialized = JsonSerializer.Deserialize<ClientCredentials>(json, Conversation.JsonSerializerOptions);

            deserialized.Should().NotBeNull();
            deserialized!.ClientId.Should().Be(original.ClientId);
            deserialized.ClientSecret.Should().Be(original.ClientSecret);
            deserialized.Endpoint.Should().Be(original.Endpoint);
            deserialized.Scope.Should().Be(original.Scope);
            deserialized.ResponseType.Should().Be(original.ResponseType);
            deserialized.TokenRequestType.Should().Be(original.TokenRequestType);
        }
    }
}
