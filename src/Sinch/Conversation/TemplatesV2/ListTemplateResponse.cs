using System.Collections.Generic;

namespace Sinch.Conversation.TemplatesV2
{
    public sealed class ListTemplatesResponse
    {
        /// <summary>
        ///     List of templates associated with the referenced conversation.
        /// </summary>
        public List<Template>? Templates { get; set; }
    }
}
