using System.Text.Json.Serialization;

namespace Sinch.Voice.Destinations
{
    /// <summary>
    ///     Marker interface for destinations valid in callout requests and call responses:
    ///     <see cref="DestinationPstn"/>, <see cref="DestinationMxp"/>, <see cref="DestinationSip"/>.
    /// </summary>
    [JsonConverter(typeof(CalloutDestinationConverter))]
    public interface ICalloutDestination : IDestination { }
}
