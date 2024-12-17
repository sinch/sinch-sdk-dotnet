using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.TemplatesV1
{
    public class UpdateTemplateRequest
    {
        // TODO: make required when net9 merges
        /// <summary>
        ///     Id of a template to update
        /// </summary>
        [JsonIgnore]
        public string? Id { get; set; }

        /// <summary>
        ///     The default translation to use if translation not specified. Specified as a BCP-47 &#x60;language_code&#x60; and the &#x60;language_code&#x60; must exist in the translations list.
        /// </summary>
        [JsonPropertyName("default_translation")]
        public string? DefaultTranslation { get; set; }

        /// <summary>
        ///     The set of field mask paths.
        /// </summary>
        [JsonIgnore]
        public List<string>? UpdateMask { get; set; }

        /// <summary>
        ///     Gets or Sets Translations
        /// </summary>
        [JsonPropertyName("translations")]
        public List<TemplateTranslation>? Translations { get; set; }

        /// <summary>
        ///     The description of the template.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }


        /// <summary>
        ///     Timestamp when the template was created.
        /// </summary>
        [JsonPropertyName("create_time")]
        public DateTime? CreateTime { get; set; }


        /// <summary>
        ///     Timestamp when the template was updated.
        /// </summary>
        [JsonPropertyName("update_time")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        ///     Gets or Sets Channel
        /// </summary>
        [JsonPropertyName("channel")]
        public TemplateChannel? Channel { get; set; }

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class UpdateTemplateRequest {\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  DefaultTranslation: ").Append(DefaultTranslation).Append("\n");
            sb.Append("  Translations: ").Append(Translations).Append("\n");
            sb.Append("  CreateTime: ").Append(CreateTime).Append("\n");
            sb.Append("  UpdateTime: ").Append(UpdateTime).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
