namespace Sinch.Fax.Faxes
{
    /// <summary>
    /// When sending a fax, you can choose between two different methods of converting the document to a faxable image. Default is Halftone and should be used for most documents. Monochrome should be used for documents that are already black and white.
    /// </summary>
    public enum ImageConversionMethod
    {
        HALFTONE,
        MONOCHROME
    }

}
