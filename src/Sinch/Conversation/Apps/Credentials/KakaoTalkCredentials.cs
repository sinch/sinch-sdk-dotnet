using System.Text.Json.Serialization;

namespace Sinch.Conversation.Apps.Credentials
{
    /// <summary>
    ///     If you are including the KakaoTalk channel in the &#x60;channel_identifier&#x60; property, you must include this object.
    /// </summary>
    public sealed class KakaoTalkCredentials
    {
        /// <summary>
        ///     KakaoTalk Business Channel ID.
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
        public string? ApiKey { get; set; }
    }
}
