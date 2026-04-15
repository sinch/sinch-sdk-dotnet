using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Sinch.Conversation.Messages.Message;

namespace Sinch.Conversation.TemplatesV2
{
    public sealed class TemplateTranslation
    {
        [JsonConstructor]
        private TemplateTranslation() { }

        public TemplateTranslation(ChoiceMessage choiceMessage) => ChoiceMessage = choiceMessage;

        public TemplateTranslation(LocationMessage locationMessage) => LocationMessage = locationMessage;

        public TemplateTranslation(MediaMessage mediaMessage) => MediaMessage = mediaMessage;

        public TemplateTranslation(TemplateMessage templateMessage) => TemplateMessage = templateMessage;

        public TemplateTranslation(ListMessage listMessage) => ListMessage = listMessage;

        public TemplateTranslation(TextMessage textMessage) => TextMessage = textMessage;

        public TemplateTranslation(CardMessage cardMessage) => CardMessage = cardMessage;

        public TemplateTranslation(CarouselMessage carouselMessage) => CarouselMessage = carouselMessage;

        public TemplateTranslation(ContactInfoMessage contactInfoMessage) => ContactInfoMessage = contactInfoMessage;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TextMessage? TextMessage { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public CardMessage? CardMessage { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public CarouselMessage? CarouselMessage { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ChoiceMessage? ChoiceMessage { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public LocationMessage? LocationMessage { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public MediaMessage? MediaMessage { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TemplateMessage? TemplateMessage { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ListMessage? ListMessage { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ContactInfoMessage? ContactInfoMessage { get; init; }

        /// <summary>
        ///     The BCP-47 language code, such as &#x60;en-US&#x60; or &#x60;sr-Latn&#x60;. For more information, see http://www.unicode.org/reports/tr35/#Unicode_locale_identifier.
        /// </summary>

        public required string LanguageCode { get; set; }

        /// <summary>
        ///     The version of the translation.
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        ///     Field to override the omnichannel template by referring to a channel-specific template.
        ///     The key in the map must point to a valid conversation channel.
        /// </summary>
        public Dictionary<string, ChannelTemplateOverride>? ChannelTemplateOverrides { get; set; }

        /// <summary>
        ///     List of expected variables. Can be used for request validation.
        /// </summary>
        public List<TypeTemplateVariable>? Variables { get; set; }


        /// <summary>
        ///     Timestamp when the translation was created.
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        ///     Timestamp of when the translation was updated.
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class TemplateTranslation {\n");
            sb.Append("  LanguageCode: ").Append(LanguageCode).Append("\n");
            sb.Append("  Version: ").Append(Version).Append("\n");
            sb.Append("  TextMessage: ").Append(TextMessage).Append("\n");
            sb.Append("  CardMessage: ").Append(CardMessage).Append("\n");
            sb.Append("  CarouselMessage: ").Append(CarouselMessage).Append("\n");
            sb.Append("  ChoiceMessage: ").Append(ChoiceMessage).Append("\n");
            sb.Append("  LocationMessage: ").Append(LocationMessage).Append("\n");
            sb.Append("  MediaMessage: ").Append(MediaMessage).Append("\n");
            sb.Append("  TemplateMessage: ").Append(TemplateMessage).Append("\n");
            sb.Append("  ListMessage: ").Append(ListMessage).Append("\n");
            sb.Append("  ContactInfoMessage: ").Append(ContactInfoMessage).Append("\n");
            sb.Append("  ChannelTemplateOverrides: ").Append(ChannelTemplateOverrides).Append("\n");
            sb.Append("  Variables: ").Append(Variables).Append("\n");
            sb.Append("  CreateTime: ").Append(CreateTime).Append("\n");
            sb.Append("  UpdateTime: ").Append(UpdateTime).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
