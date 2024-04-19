using System;
using System.Buffers;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sinch.Core
{
    public static class JsonExtensions
    {
        public static object? ToObject(this JsonElement element, Type type, JsonSerializerOptions? options = null)
        {
            var bufferWriter = new ArrayBufferWriter<byte>();
            using (var writer = new Utf8JsonWriter(bufferWriter))
            {
                element.WriteTo(writer);
            }

            return JsonSerializer.Deserialize(bufferWriter.WrittenSpan, type, options);
        }

        public static bool IsTypeOf(this JsonElement element, Type type, JsonSerializerOptions options)
        {
            var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            return element.EnumerateObject()
                .All(jsonProp => props.Any(prop =>
                {
                    var nameAttr =
                        prop.GetCustomAttribute(typeof(JsonPropertyNameAttribute)) as JsonPropertyNameAttribute;
                    if (nameAttr is { Name: not null })
                    {
                        return jsonProp.Name == nameAttr.Name;
                    }

                    var propName = options.PropertyNamingPolicy?.ConvertName(prop.Name) ?? prop.Name;
                    return propName == jsonProp.Name;
                }));
        }
    }
}
