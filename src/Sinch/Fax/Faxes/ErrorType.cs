namespace Sinch.Fax.Faxes
{
    /// <summary>
    /// When your receive a callback from the fax api notifying you of a failure, it will contain two values that can help you understand what went wrong: an errorCode and an type.
    /// The type will give you a general idea of why the operation failed, whereas the errorCode describes the issue in more detail.Below we list the error_codes for the API, segmented by their corresponding error_type.
    /// </summary>
    /// 
    public enum ErrorType
    {
        DOCUMENT_CONVERSION_ERROR,
        CALL_ERROR,
        FAX_ERROR,
        LINE_ERROR,
        faxError

    }
}
