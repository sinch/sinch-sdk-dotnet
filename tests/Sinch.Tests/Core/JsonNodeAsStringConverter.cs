using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using FluentAssertions;
using Sinch.Core;
using Xunit;

namespace Sinch.Tests.Core
{
    public class JsonNodeAsStringConverter
    {
        private class Container
        {
            [JsonConverter(typeof(JsonNodeAsStringJsonConverter))]
            public JsonNode Value { get; set; }
        }


        [Fact]
        public void JsonNodeAsStringSerializeObject()
        {
            var container = new Container()
            {
                Value = new JsonObject()
                {
                    ["hello"] = "world"
                }
            };
            JsonSerializer.Serialize(container).Should()
                .BeEquivalentTo("{\"Value\":\"{\\u0022hello\\u0022:\\u0022world\\u0022}\"}");
        }

        [Fact]
        public void JsonNodeAsStringSerializeString()
        {
            var container = new Container()
            {
                Value = "my-string-data"
            };
            JsonSerializer.Serialize(container).Should()
                .BeEquivalentTo("{\"Value\":\"my-string-data\"}");
        }

        [Fact]
        public void JsonNodeAsStringSerializeNull()
        {
            var container = new Container()
            {
                Value = null
            };
            JsonSerializer.Serialize(container).Should()
                .BeEquivalentTo("{\"Value\":null}");
        }
    }
}
