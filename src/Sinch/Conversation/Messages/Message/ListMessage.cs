using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Messages.Message
{
    /// <summary>
    ///     A message containing a list of options to choose from
    /// </summary>
    public sealed class ListMessage
    {
        /// <summary>
        ///     A title for the message that is displayed near the products or choices.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string Title { get; set; }
#else
        public string Title { get; set; }
#endif


        /// <summary>
        ///     This is an optional field, containing a description for the message.
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        ///     List of ListSection objects containing choices to be presented in the list message.
        /// </summary>
#if NET7_0_OR_GREATER
        public required List<ListSection> Sections { get; set; }
#else
        public List<ListSection> Sections { get; set; }
#endif


        /// <summary>
        ///     Gets or Sets MessageProperties
        /// </summary>
        public ListMessageMessageProperties MessageProperties { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ListMessage {\n");
            sb.Append("  Title: ").Append(Title).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  Sections: ").Append(Sections).Append("\n");
            sb.Append("  MessageProperties: ").Append(MessageProperties).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    /// <summary>
    ///     Section for interactive whatsapp messages containing ListItem
    /// </summary>
    public sealed class ListSection
    {
        /// <summary>
        ///     Optional parameter. Title for list section.
        /// </summary>
        public string Title { get; set; }


        /// <summary>
        ///     Gets or Sets Items
        /// </summary>
        public List<IListItem> Items { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class ListSection {\n");
            sb.Append("  Title: ").Append(Title).Append("\n");
            sb.Append("  Items: ").Append(Items).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    [JsonInterfaceConverter(typeof(InterfaceConverter<IListItem>))]
    public interface IListItem
    {
    }

    public class ListItemChoiceWrapper : IListItem
    {
        public ListItemChoice Choice { get; set; }
    }
    
    public class ListItemProductWrapper : IListItem
    {
        public ListItemProduct Product { get;  set; }
    }

    /// <summary>
    ///     Additional properties for the message. Required if sending a product list message.
    /// </summary>
    public sealed class ListMessageMessageProperties
    {
        /// <summary>
        ///     Required if sending a product list message. The ID of the catalog to which the products belong.
        /// </summary>
        public string CatalogId { get; set; }


        /// <summary>
        ///     Optional. Sets the text for the menu of a choice list message.
        /// </summary>
        public string Menu { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class ListMessageMessageProperties {\n");
            sb.Append("  CatalogId: ").Append(CatalogId).Append("\n");
            sb.Append("  Menu: ").Append(Menu).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
