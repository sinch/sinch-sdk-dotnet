using System;
using System.Text.Json;
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
            return JsonSerializer.Deserialize<T>(json, Numbers.JsonSerializerOptions);
        }
    }
}
