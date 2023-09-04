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


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class KakaoTalkCredentials {\n");
            sb.Append("  KakaotalkPlusFriendId: ").Append(KakaotalkPlusFriendId).Append("\n");
            sb.Append("  KakaotalkSenderKey: ").Append(KakaotalkSenderKey).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
