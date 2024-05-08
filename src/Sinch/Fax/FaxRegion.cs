using Sinch.Core;

namespace Sinch.Fax
{
    public record FaxRegion(string Value) : EnumRecord(Value)
    {
        public static readonly FaxRegion UsEastCost = new("use1");
        public static readonly FaxRegion Europe = new("eu1");
        public static readonly FaxRegion SouthAmerica = new("sae1");
        public static readonly FaxRegion SouthEastAsia1 = new("apse1");
        public static readonly FaxRegion SouthEastAsia2 = new("apse2");
    }
}
