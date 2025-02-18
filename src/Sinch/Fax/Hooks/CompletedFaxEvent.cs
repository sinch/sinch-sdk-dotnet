using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Sinch.Fax.Faxes;

namespace Sinch.Fax.Hooks
{
    public sealed class CompletedFaxEvent : GenericFaxEvent, IFaxEvent
    {
        public override FaxEventType Event { get; } = FaxEventType.CompletedFax;

        /// <summary>
        ///     Base64 list of files
        /// </summary>
        [JsonPropertyName("files")]
        public List<Base64File> Files { get; set; } = new();
    }
}
