using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Voice.Calls.Actions
{
    /// <summary>
    ///     Determines how an application-to-application call is connected.
    /// </summary>
    public sealed class ConnectMxp : IAction
    {
        public string Name { get; } = "connectMxp";

        /// <inheritdoc cref="Destination" />
        [JsonPropertyName("destination")]
        public Destination? Destination { get; set; }

        /// <summary>
        ///     An optional parameter that allows you to specify or override call headers provided to the receiving Sinch SDK
        ///     client. Read more about call headers
        ///     <see href="https://developers.sinch.com/docs/voice/api-reference/voice/call-headers/">here</see>.
        /// </summary>
        [JsonPropertyName("callheaders")]
        public List<CallHeader>? CallHeaders { get; set; }
    }

    /// <summary>
    ///     Allows you to specify or override the final destination of the call. If the final destination of the call is not
    ///     dialed, this is a required parameter.
    /// </summary>
    public sealed class Destination
    {
        /// <summary>
        ///     The type of the definition.
        /// </summary>
        [JsonPropertyName("type")]
#if NET7_0_OR_GREATER
        public required DestinationType Type { get; set; }
#else
        public string Type { get; set; } = null!;
#endif

        /// <summary>
        ///     The phone number or username of the desired call destination.
        /// </summary>
        [JsonPropertyName("endpoint")]
#if NET7_0_OR_GREATER
        public required string Endpoint { get; set; }
#else
        public string Endpoint { get; set; } = null!;
#endif


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ConnectMxp {\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  Endpoint: ").Append(Endpoint).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    /// <summary>
    ///     The type of the definition.
    /// </summary>
    /// <param name="Value"></param>
    [JsonConverter(typeof(EnumRecordJsonConverter<DestinationType>))]
    public record DestinationType(string Value) : EnumRecord(Value)
    {
        public static readonly DestinationType Number = new("number");
        public static readonly DestinationType Username = new("username");
    }
}
