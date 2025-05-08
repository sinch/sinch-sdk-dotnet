using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using Sinch.Core;
using Xunit;

namespace Sinch.Tests.Core
{
    public class UnixTimestampSecondsJsonConverterTests
    {
        private readonly Container _withDateTime = new Container()
        {
            Utc = Helpers.ParseUtc("2025-04-01T02:03:04Z")
        };

        public sealed class Container
        {
            [JsonConverter(typeof(UnixTimestampSecondsJsonConverter))]
            public DateTime? Utc { get; set; }
        }

        [Fact]
        public void ShouldSerializeDateTimeToUnixTimestampSeconds()
        {
            var json = JsonSerializer.Serialize(_withDateTime);
            Helpers.AssertJsonEqual("{ \"Utc\": \"1743472984\"}", json);
        }

        [Fact]
        public void ShouldDeserializeUtcTimestampInSecondsToDateTime()
        {
            var str = "{ \"Utc\": \"1743472984\"}";

            var deserialized = JsonSerializer.Deserialize<Container>(str);

            deserialized.Should().BeEquivalentTo(_withDateTime);
        }

        [Fact]
        public void ShouldThrowJsonExceptionIfValueNotString()
        {
            var str = "{ \"Utc\": 1743472984 }";

            var deserializedOp = () => JsonSerializer.Deserialize<Container>(str);

            deserializedOp.Should().Throw<JsonException>().Which.Message.Should()
                .StartWith("Expected String token type. Got ");
        }

        [Fact]
        public void ShouldBeNull()
        {
            var str = "{ \"Utc\": null }";

            var deserialize = JsonSerializer.Deserialize<Container>(str);

            deserialize.Utc.Should().BeNull();
        }

        [Fact]
        public void ShouldThrowIfEmptyString()
        {
            var str = "{ \"Utc\": \"\" }";

            var deserializedOp = () => JsonSerializer.Deserialize<Container>(str);

            deserializedOp.Should().Throw<JsonException>().Which.Message.Should().Be(
                "Expected Unix timestamp in seconds as a string representing a number, got an empty string.");
        }
    }
}
