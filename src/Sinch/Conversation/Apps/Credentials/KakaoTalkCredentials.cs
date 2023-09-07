using System.Text;

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
#if NET7_0_OR_GREATER
        public required string KakaotalkPlusFriendId { get; set; }
#else
        public string KakaotalkPlusFriendId { get; set; }
#endif


        /// <summary>
        ///     KakaoTalk Sender Key.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string KakaotalkSenderKey { get; set; }
#else
        public string KakaotalkSenderKey { get; set; }
#endif
    }
}
