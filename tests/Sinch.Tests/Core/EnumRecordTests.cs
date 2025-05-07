using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using Sinch.Core;
using Xunit;

namespace Sinch.Tests.Core
{
    [JsonConverter(typeof(EnumRecordCaseInsensitiveJsonConverter<EnumRecordExample>))]
    public record EnumRecordExample(string Value) : EnumRecord(Value)
    {
        public static readonly EnumRecordExample VariantA = new EnumRecordExample("varianta");
        // for real case, all values in EnumRecordCaseInsensitiveJsonConverter enums should be lowercase
        public static readonly EnumRecordExample VariantB = new EnumRecordExample("VariantB");
    }
    
    public class EnumRecordTests
    {
      
        [Theory]
        [InlineData("\"variantA\"")]
        [InlineData("\"VariantA\"")]
        [InlineData("\"varianta\"")]
        [InlineData("\"VARIANTA\"")]
        public void DeserializeCaseInsensitive(string variant)
        {
            var result = JsonSerializer.Deserialize<EnumRecordExample>(variant);

            result.Should().BeEquivalentTo(EnumRecordExample.VariantA);
        }
        
        [Fact]
        public void SerializePreservingValue()
        {
            var variant = EnumRecordExample.VariantB;
            var result = JsonSerializer.Serialize(variant);
            
            result.Should().BeEquivalentTo("\"VariantB\"");
        }
    }
    
    
}
