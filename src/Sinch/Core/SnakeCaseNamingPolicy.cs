using System.Text.Json;

namespace Sinch.Core
{
    internal sealed class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public static SnakeCaseNamingPolicy Instance { get; } = new();

        private SnakeCaseNamingPolicy() {}

        public override string ConvertName(string name)
        {
            return StringUtils.ToSnakeCase(name);
        }
    }
}
