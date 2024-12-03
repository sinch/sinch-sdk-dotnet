namespace Sinch.Conversation.Apps.Credentials
{
    /// <summary>
    ///     If you are including the Telegram Bot channel in the &#x60;channel_identifier&#x60; property, you must include this object.
    /// </summary>
    public sealed class TelegramCredentials
    {
        /// <summary>
        ///     The token for the Telegram bot to which you are connecting.
        /// </summary>

        public required string Token { get; set; }

    }
}
