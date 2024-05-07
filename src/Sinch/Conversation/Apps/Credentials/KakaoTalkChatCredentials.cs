using System.Text.Json.Serialization;

namespace Sinch.Conversation.Apps.Credentials
{
    /// <summary>
    ///     If you are including the KakaoTalkChat channel in the &#x60;channel_identifier&#x60; property, you must include this object.
    /// </summary>
    public sealed class KakaoTalkChatCredentials
    {
        /// <summary>
        ///     Kakaotalk Plus friend ID.
        /// </summary>
        [JsonPropertyName("kakaotalk_plus_friend_id")]
#if NET7_0_OR_GREATER
        public required string KakaoTalkPlusFriendId { get; set; }
#else
        public string KakaoTalkPlusFriendId { get; set; } = null!;
#endif

        /// <summary>
        ///     InfoBank API KEY.
        /// </summary>
        [JsonPropertyName("api_key")]
#if NET7_0_OR_GREATER
        public required string ApiKey { get; set; }
#else
        public string ApiKey { get; set; } = null!;
#endif
    }
}
