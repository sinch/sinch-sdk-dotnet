namespace Sinch.Fax.Faxes
{


    /// <summary>
    /// The direction of the fax.
    /// </summary>
    /// 
    public enum Direction
    {
        /// <summary>
        /// The fax was received on one of your sinch numbers.
        /// </summary>
        INBOUND,
        /// <summary>
        /// The fax was sent by you via the api.
        /// </summary>
        OUTBOUND
    }

}
