using System.Text;

namespace Sinch.Voice.Applications.QueryNumber
{
    /// <summary>
    ///     The number item object.
    /// </summary>
    public sealed class NumberItem
    {
        /// <summary>
        ///     The type of the number.
        /// </summary>
        public NumberType NumberType { get; set; }

        /// <summary>
        ///     The ISO 3166-1 formatted country code.
        /// </summary>
        public string CountryId { get; set; }


        /// <summary>
        ///     The number in E.164 format.
        /// </summary>
        public string NormalizedNumber { get; set; }


        /// <summary>
        ///     Concerns whether the call is restricted or not.
        /// </summary>
        public bool Restricted { get; set; }


        /// <summary>
        ///     Gets or Sets Rate
        /// </summary>
        public Rate Rate { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class NumberItem {\n");
            sb.Append("  CountryId: ").Append(CountryId).Append("\n");
            sb.Append("  NumberType: ").Append(NumberType).Append("\n");
            sb.Append("  NormalizedNumber: ").Append(NormalizedNumber).Append("\n");
            sb.Append("  Restricted: ").Append(Restricted).Append("\n");
            sb.Append("  Rate: ").Append(Rate).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
