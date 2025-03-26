namespace Sinch.Conversation.Apps
{
    /// <summary>
    ///     This object is required for apps that subscribe to Smart Conversations features.
    ///     Note that this functionality is available for open beta testing.
    /// </summary>
    public sealed class SmartConversation
    {
        public SmartConversation(bool enabled)
        {
            Enabled = enabled;
        }
        
        /// <summary>
        ///     Set to true to allow messages processed by this app to be analyzed by Smart Conversations.
        /// </summary>
        public bool Enabled { get; set; }
    }
}
