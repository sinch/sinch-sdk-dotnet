using System.Text.Json;

namespace Sinch.Core
{
    internal class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public static SnakeCaseNamingPolicy Instance { get; } = new();


        public override string ConvertName(string name)
        {
            return StringUtils.ToSnakeCase(name);
        }
    }
}
