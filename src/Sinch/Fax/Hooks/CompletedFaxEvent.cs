using System;

namespace Sinch.Fax.Hooks
{
    public class CompletedFaxEvent : GenericFaxEvent, IFaxEvent
    {
        public override FaxEventType Event { get; } = FaxEventType.CompletedFax;
    }
}
