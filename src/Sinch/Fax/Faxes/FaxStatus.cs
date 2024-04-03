namespace Sinch.Fax.Faxes
{
    /// <summary>
    /// The status of the fax
    /// </summary>
    public enum FaxStatus
    {
        /// <summary>
        /// The operation is currently in a queue on a server and should be executed very soon.
        /// </summary>
        QUEUED,
        /// <summary>
        /// The fax is currently being sent (OUTBOUND) or received (INBOUND).
        /// </summary>
        IN_PROGRESS,
        /// <summary>
        /// The fax operation succeeded. Everything went as normally planned.
        /// </summary>
        COMPLETED,
        /// <summary>
        /// The fax operation failed. Details of the error can be found in the error_code field. For OUTBOUND fax, this means that NONE of the recipients received the fax.
        /// </summary>
        /// <see cref="ErrorType"/>"/>
        FAILURE

    }
}
