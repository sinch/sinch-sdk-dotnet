using System.Text.Json.Serialization;
using FluentAssertions;
using Sinch.Core;
using Xunit;

namespace Sinch.Tests.Core;

public class JsonExtensionsTests
{
    [Fact]
    public void SimpleObjectReturnsFormattedJson()
    {
        var obj = new { Name = "Test", Value = 42 };

        var result = obj.ToPrettyString().ReplaceLineEndings("\n");

        var expectedJson = "{\n  \"Name\": \"Test\",\n  \"Value\": 42\n}";

        result.Should().Be(expectedJson);
    }

    [Fact]
    public void ObjectWithJsonPropertyNameUsesPropertyName()
    {
        var obj = new TestClass { MyProperty = "value" };

        var result = obj.ToPrettyString().ReplaceLineEndings("\n");

        var expectedJson = "{\n  \"custom_name\": \"value\"\n}";

        result.Should().Be(expectedJson);
        result.Should().NotContain("MyProperty");
    }

    [Fact]
    public void NullObjectReturnsNullString()
    {
        object obj = null;

        var result = obj.ToPrettyString();

        result.Should().Be("null");
    }

    [Fact]
    public void EmptyObjectReturnsEmptyJsonObject()
    {
        var obj = new { };

        var result = obj.ToPrettyString();

        result.Should().Be("{}");
    }

    private class TestClass
    {
        [JsonPropertyName("custom_name")]
        public string MyProperty { get; set; }
    }
}
