using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sinch.Numbers;

namespace Sinch.Tests.Numbers
{
    public class NumberTestBase : TestBase
    {
        internal readonly ISinchNumbers Numbers;

        protected NumberTestBase()
        {
            Numbers = new Sinch.Numbers.Numbers(ProjectId,
                new Uri("https://numbers.api.sinch.com/"), default, HttpCamelCase);
        }

        protected string SerializeAsNumbersClient<T>(T value)
        {
            return JsonSerializer.Serialize(value, Numbers.JsonSerializerOptions);
        }

        protected T DeserializeAsNumbersClient<T>(string json)
        {
            // for unit tests worth checking if model is missing properties from json
            var withFailOnUnexpectedProps = new JsonSerializerOptions(Numbers.JsonSerializerOptions);
            withFailOnUnexpectedProps.UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow;
            return JsonSerializer.Deserialize<T>(json, withFailOnUnexpectedProps);
        }
    }
}
