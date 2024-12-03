namespace Sinch.Voice.Callouts.Callout
{
    public class Destination
    {
        /// <summary>
        ///     Can be of type number for PSTN endpoints or of type username for data endpoints.
        /// </summary>

        public required DestinationType Type { get; set; }

        /// <summary>
        ///     If the type is number the value of the endpoint is a phone number.
        ///     If the type is username the value is the username for a data endpoint.
        /// </summary>

        public required string Endpoint { get; set; }

    }
}
