using System.Collections.Generic;

namespace Sinch.Conversation.TemplatesV2
{
    public sealed class ListTemplateTranslationResponse
    {
        /// <summary>
        ///     List of template translations associated with the referenced template.
        /// </summary>
        public IEnumerable<TemplateTranslation>? Translations { get; set; }
    }
}
