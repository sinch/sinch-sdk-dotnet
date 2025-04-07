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
    }
}
