using System;
using System.Collections.Generic;

namespace Sinch.Conversation.Messages.Message
{
    /// <summary>
    ///     Maps json name of App Message to it's corresponding type. Required for some nasty json ser/de
    /// </summary>
    internal static class MessagePropNameToTypeMap
    {
        public static IReadOnlyDictionary<string, Type> Map = new Dictionary<string, Type>()
        {
            { "text_message", typeof(TextMessage) },
            { "media_message", typeof(MediaMessage) },
            { "choice_message", typeof(ChoiceMessage) },
            { "card_message", typeof(CardMessage) },
            { "carousel_message", typeof(CarouselMessage) },
            { "location_message", typeof(LocationMessage) },
            { "contact_info_message", typeof(ContactInfoMessage) },
            { "list_message", typeof(ListMessage) },
            { "template_reference", typeof(TemplateReference) },
            { "template_message", typeof(TemplateMessage) },
        };
    }
}
