using Sinch.Core;

namespace Sinch.Fax.Faxes
{
    /// <summary>
    /// The direction of the fax.
    /// </summary>
    public record Direction(string Value) : EnumRecord(Value)
    {
        /// <summary>
        /// The fax was received on one of your sinch numbers.
        /// </summary>
        public static readonly Direction Inbound = new("INBOUND");

        /// <summary>
        /// The fax was sent by you via the api.
        /// </summary>
        public static readonly Direction Outbound = new("OUTBOUND");
    }
}
