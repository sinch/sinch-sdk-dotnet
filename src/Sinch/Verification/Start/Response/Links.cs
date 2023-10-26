namespace Sinch.Verification.Start.Response
{
    public class Links
    {
        /// <summary>
        ///     The related action that can be performed on the initiated Verification.
        /// </summary>
        public string Rel { get; set; }
        
        /// <summary>
        ///     The complete URL to perform the specified action,
        ///     localized to the DataCenter which handled the original Verification request.
        /// </summary>
        public string Href { get; set; }
        
        /// <summary>
        ///     The HTTP method to use when performing the action using the linked localized URL.
        /// </summary>
        public string Method { get; set; }
    }
}
