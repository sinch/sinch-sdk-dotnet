using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sinch.Core
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    internal class JsonInterfaceConverterAttribute : JsonConverterAttribute
    {
        public JsonInterfaceConverterAttribute(Type convertedType) : base(convertedType)
        {
        }
    }

    public class InterfaceConverter<T> : JsonConverter<T> where T : class
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var type = typeof(T);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p));

            var elem = JsonElement.ParseValue(ref reader);
            dynamic obj = null;
            foreach (var @class in types)
            {
                if (elem.IsTypeOf(@class, options))
                {
                    obj = elem.ToObject(@class, options);
                    break;
                }
            }

            return (T)obj;
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            var type = value.GetType();
            JsonSerializer.Serialize(writer, value, type, options);
        }
    }
}
