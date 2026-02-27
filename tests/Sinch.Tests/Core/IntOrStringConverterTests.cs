using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using Sinch.Core;
using Xunit;

namespace Sinch.Tests.Core
{
    public class IntOrStringConverterTests
    {
        public sealed class Container
        {
            [JsonConverter(typeof(IntOrStringConverter))]
            public string? Code { get; init; }
        }

        [Fact]
        public void Read_WhenJsonIsNumber_ReturnsStringRepresentation()
        {
            var json = "{\"Code\": 400}";

            var result = JsonSerializer.Deserialize<Container>(json);

            result!.Code.Should().Be("400");
        }

        [Fact]
        public void Read_WhenJsonIsNonNumericString_ReturnsString()
        {
            var json = "{\"Code\": \"Forbidden\"}";

            var result = JsonSerializer.Deserialize<Container>(json);

            result!.Code.Should().Be("Forbidden");
        }

        [Fact]
        public void Read_WhenJsonIsNumericString_ReturnsString()
        {
            var json = "{\"Code\": \"400\"}";

            var result = JsonSerializer.Deserialize<Container>(json);

            result!.Code.Should().Be("400");
        }

        [Fact]
        public void Read_WhenJsonIsNull_ReturnsNull()
        {
            var json = "{\"Code\": null}";

            var result = JsonSerializer.Deserialize<Container>(json);

            result!.Code.Should().BeNull();
        }

        [Fact]
        public void Write_WhenValueIsString_WritesJsonString()
        {
            var container = new Container { Code = "Forbidden" };

            var json = JsonSerializer.Serialize(container);

            json.Should().Contain("\"Forbidden\"");
        }

        [Fact]
        public void Write_WhenValueIsNull_WritesJsonNull()
        {
            var container = new Container { Code = null };

            var json = JsonSerializer.Serialize(container);

            json.Should().Contain("null");
        }
    }
}
