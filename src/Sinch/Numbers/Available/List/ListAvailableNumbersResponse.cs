using System.Collections.Generic;

namespace Sinch.Numbers.Available.List
{
    public sealed class ListAvailableNumbersResponse
    {
        public IEnumerable<AvailableNumber>? AvailableNumbers { get; set; }
    }
}
