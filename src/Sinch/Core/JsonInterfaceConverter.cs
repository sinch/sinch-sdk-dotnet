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

           
            
            var exceptions = new List<Exception>();
            foreach (var t in types)
            {
                var typeReader = reader;
                try
                {
                    var r = JsonSerializer.Deserialize(ref typeReader, t, options);
                    if (r == null)
                    {
                        exceptions.Add(new NullReferenceException("Deserialization to {t.GetName()} is null"));
                        continue;
                    }

                    var props = r.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    if (props.All(x => x.GetValue(r) is null))
                    {
                        continue;
                    }

                    return r as T;
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
            }

            throw new AggregateException(exceptions);
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            var type = value.GetType();
            JsonSerializer.Serialize(writer, value, type, options);
        }
    }
}
