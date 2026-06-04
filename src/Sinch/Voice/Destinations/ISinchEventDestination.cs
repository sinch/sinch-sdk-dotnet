using System.Text.Json.Serialization;

namespace Sinch.Voice.Destinations
{
    /// <summary>
    ///     Marker interface for destinations valid in Voice Sinch Event payloads (ICE / DICE <c>to</c> field):
    ///     <see cref="DestinationPstn"/>, <see cref="DestinationMxp"/>, <see cref="DestinationSip"/>, <see cref="DestinationDid"/>.
    /// </summary>
    [JsonConverter(typeof(SinchEventDestinationConverter))]
    public interface ISinchEventDestination : IDestination { }
}
