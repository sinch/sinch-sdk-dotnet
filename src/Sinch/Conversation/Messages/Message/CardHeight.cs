using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Messages.Message
{
    [JsonConverter(typeof(SinchEnumConverter<CardHeight>))]
    public enum CardHeight
    {   
        /// <summary>
        /// Enum UNSPECIFIEDHEIGHT for value: UNSPECIFIED_HEIGHT
        /// </summary>
        [EnumMember(Value = "UNSPECIFIED_HEIGHT")]
        UnspecifiedHeight = 1,

        /// <summary>
        /// Enum SHORT for value: SHORT
        /// </summary>
        [EnumMember(Value = "SHORT")]
        Short = 2,

        /// <summary>
        /// Enum MEDIUM for value: MEDIUM
        /// </summary>
        [EnumMember(Value = "MEDIUM")]
        Medium = 3,

        /// <summary>
        /// Enum TALL for value: TALL
        /// </summary>
        [EnumMember(Value = "TALL")]
        Tall = 4
    }
}
