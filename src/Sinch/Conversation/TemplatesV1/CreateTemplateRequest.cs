using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.TemplatesV1
{
    public sealed class CreateTemplateRequest
    {
      
         /// <summary>
        ///     Gets or Sets Channel
        /// </summary>
        [JsonPropertyName("channel")]
        public TemplateChannel? Channel { get; set; }


        /// <summary>
        ///     Timestamp when the template was created.
        /// </summary>
        [JsonPropertyName("create_time")]
        public DateTime? CreateTime { get; set; }


        // TODO: make required when net9 PR merges
        /// <summary>
        ///     The default translation to use if not specified. Specified as a BCP-47 &#x60;language_code&#x60; and the &#x60;language_code&#x60; must exist in the translations list.
        /// </summary>
        [JsonPropertyName("default_translation")]
        public string? DefaultTranslation { get; set; }


        /// <summary>
        ///     The description of the template.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }


        /// <summary>
        ///     The id of the template. Specify this yourself during creation otherwise we will generate an ID for you. This has to be unique for a given project.
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }


        // TODO: make required when net9 PR merges
        /// <summary>
        ///     List of translations for the template.
        /// </summary>
        [JsonPropertyName("translations")]
        public IList<TemplateTranslation>? Translations { get; set; }


        /// <summary>
        ///     Timestamp when the template was updated.
        /// </summary>
        [JsonPropertyName("update_time")]
        public DateTime? UpdateTime { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(Template)} {{\n");
            sb.Append($"  {nameof(Channel)}: ").Append(Channel).Append('\n');
            sb.Append($"  {nameof(CreateTime)}: ").Append(CreateTime).Append('\n');
            sb.Append($"  {nameof(DefaultTranslation)}: ").Append(DefaultTranslation).Append('\n');
            sb.Append($"  {nameof(Description)}: ").Append(Description).Append('\n');
            sb.Append($"  {nameof(Id)}: ").Append(Id).Append('\n');
            sb.Append($"  {nameof(Translations)}: ").Append(Translations).Append('\n');
            sb.Append($"  {nameof(UpdateTime)}: ").Append(UpdateTime).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
