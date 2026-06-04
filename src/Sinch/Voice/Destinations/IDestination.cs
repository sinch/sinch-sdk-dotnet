namespace Sinch.Voice.Destinations
{
    /// <summary>Base contract for all Voice destination types.</summary>
    public interface IDestination
    {
        /// <summary>The destination type discriminator.</summary>
        DestinationType Type { get; }
        /// <summary>The phone number, username, SIP address, or WebSocket endpoint.</summary>
        string Endpoint { get; }
    }
}
