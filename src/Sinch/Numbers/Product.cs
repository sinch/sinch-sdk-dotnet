using System.Text.Json.Serialization;

namespace Sinch.Numbers
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Product
    {
        /// <summary>
        ///     The SMS product can use the number.
        /// </summary>
        Sms,

        /// <summary>
        ///     The Voice product can use the number.
        /// </summary>
        Voice
    }
}
