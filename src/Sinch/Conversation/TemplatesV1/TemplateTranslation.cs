using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.TemplatesV1
{
    public class TemplateTranslation
    {
        /// <summary>
        ///     This is the definition of the template with the language specified in the language_code field.
        /// </summary>
        [JsonPropertyName("content")]
        public string? Content { get; set; }


        /// <summary>
        ///     Timestamp when the translation was created.
        /// </summary>
        [JsonPropertyName("create_time")]
        public DateTime? CreateTime { get; set; }


        /// <summary>
        ///     The BCP-47 language code, such as &#x60;en-US&#x60; or &#x60;sr-Latn&#x60;. For more information, see https://www.unicode.org/reports/tr35/#Unicode_locale_identifier.
        /// </summary>
        [JsonPropertyName("language_code")]
        public string? LanguageCode { get; set; }


        /// <summary>
        ///     Timestamp of when the translation was updated.
        /// </summary>
        [JsonPropertyName("update_time")]
        public DateTime? UpdateTime { get; set; }


        /// <summary>
        ///     List of expected variables. Can be used for request validation.
        /// </summary>
        [JsonPropertyName("variables")]
        public List<TypeTemplateVariable>? Variables { get; set; }


        /// <summary>
        ///     The version of template.
        /// </summary>
        [JsonPropertyName("version")]
        public string? Version { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(TemplateTranslation)} {{\n");
            sb.Append($"  {nameof(Content)}: ").Append(Content).Append('\n');
            sb.Append($"  {nameof(CreateTime)}: ").Append(CreateTime).Append('\n');
            sb.Append($"  {nameof(LanguageCode)}: ").Append(LanguageCode).Append('\n');
            sb.Append($"  {nameof(UpdateTime)}: ").Append(UpdateTime).Append('\n');
            sb.Append($"  {nameof(Variables)}: ").Append(Variables).Append('\n');
            sb.Append($"  {nameof(Version)}: ").Append(Version).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    public class TypeTemplateVariable
    {
        [JsonPropertyName("key")]
        public string? Key { get; set; }

        [JsonPropertyName("preview_value")]
        public string? PreviewValue { get; set; }

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(TypeTemplateVariable)} {{\n");
            sb.Append($"  {nameof(Key)}: ").Append(Key).Append('\n');
            sb.Append($"  {nameof(PreviewValue)}: ").Append(PreviewValue).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
