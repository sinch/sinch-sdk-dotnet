using System;
using System.Collections.Generic;
using System.Text;

namespace Sinch.Conversation.TemplatesV2
{
    public class Template
    {
        /// <summary>
        ///     The id of the template. Specify this yourself during creation. Otherwise, we will generate an ID for you. This must
        ///     be unique for a given project.
        /// </summary>

        public required string Id { get; set; }



        /// <summary>
        ///     The description of the template.
        /// </summary>
        public string? Description { get; set; }


        /// <summary>
        ///     The version of the template. While creating a template, this will be defaulted to 1. When updating a template, you
        ///     must supply the latest version of the template in order for the update to be successful.
        /// </summary>
        public int Version { get; set; }


        /// <summary>
        ///     The default translation to use if translation not specified. Specified as a BCP-47 &#x60;language_code&#x60; and
        ///     the &#x60;language_code&#x60; must exist in the translations list.
        /// </summary>
        public string? DefaultTranslation { get; set; }


        /// <summary>
        ///     Gets or Sets Translations
        /// </summary>
        public List<TemplateTranslation>? Translations { get; set; }


        /// <summary>
        ///     Timestamp when the template was created.
        /// </summary>
        public DateTime? CreateTime { get; set; }


        /// <summary>
        ///     Timestamp when the template was updated.
        /// </summary>
        public DateTime? UpdateTime { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class V2TemplateResponse {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  Version: ").Append(Version).Append("\n");
            sb.Append("  DefaultTranslation: ").Append(DefaultTranslation).Append("\n");
            sb.Append("  Translations: ").Append(Translations).Append("\n");
            sb.Append("  CreateTime: ").Append(CreateTime).Append("\n");
            sb.Append("  UpdateTime: ").Append(UpdateTime).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
