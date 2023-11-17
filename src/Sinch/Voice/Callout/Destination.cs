namespace Sinch.Voice.Callout
{
    public class Destination
    {
        /// <summary>
        ///     Can be of type number for PSTN endpoints or of type username for data endpoints.
        /// </summary>
#if NET7_0_OR_GREATER
        public required DestinationType Type { get; set; }
#else
        public DestinationType Type { get; set; }
#endif
            /// <summary>
            ///     If the type is number the value of the endpoint is a phone number.
            ///     If the type is username the value is the username for a data endpoint.
            /// </summary>
#if NET7_0_OR_GREATER
        public required string Endpoint { get; set; }
#else
        public string Endpoint { get; set; }
#endif
    }
}
