using System.Text.Json.Serialization;
using FluentAssertions;
using Sinch.Core;
using Xunit;

namespace Sinch.Tests.Core
{
    public class JsonExtensionsTests
    {
        [Fact]
        public void SimpleObjectReturnsFormattedJson()
        {
            var obj = new { Name = "Test", Value = 42 };

            var result = obj.ToJson();

            result.Should().Contain("\"Name\": \"Test\"");
            result.Should().Contain("\"Value\": 42");
            result.Should().Contain("\n");
        }

        [Fact]
        public void ObjectWithJsonPropertyNameUsesPropertyName()
        {
            var obj = new TestClass { MyProperty = "value" };

            var result = obj.ToJson();

            result.Should().Contain("\"custom_name\"");
            result.Should().NotContain("\"MyProperty\"");
        }

        [Fact]
        public void NullObjectReturnsNullString()
        {
            object? obj = null;

            var result = obj.ToJson();

            result.Should().Be("null");
        }

        [Fact]
        public void EmptyObjectReturnsEmptyJsonObject()
        {
            var obj = new { };

            var result = obj.ToJson();

            result.Should().Be("{}");
        }

        private class TestClass
        {
            [JsonPropertyName("custom_name")]
            public string? MyProperty { get; set; }
        }
    }
}
