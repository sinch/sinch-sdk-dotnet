using System;

namespace Sinch.Fax.Hooks
{
    public class IncomingFaxEvent : GenericFaxEvent, IFaxEvent
    {
        public override FaxEventType Event { get; } = FaxEventType.IncomingFax;
    }
}
