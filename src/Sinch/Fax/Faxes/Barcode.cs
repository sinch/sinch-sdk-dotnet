namespace Sinch.Fax.Faxes
{
    public enum BarCodeType
    {
        CODE_128,
        DATA_MATRIX
    }

    /// <summary>
    /// The bar codes found in the fax. This field is populated when sinch detects bar codes on incoming faxes.
    /// </summary>
    public class Barcode
    {
        /// <summary>
        /// The type of barcode found.
        /// </summary>
        public BarCodeType Type { get; set; }

        /// <summary>
        /// The page number on which the barcode was found.
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// The information of the barcode.
        /// </summary>
        public string value { get; set; }
    }
}
