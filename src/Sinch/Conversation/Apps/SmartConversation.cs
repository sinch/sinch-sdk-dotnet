namespace Sinch.Conversation.Apps
{
    /// <summary>
    ///     This object is required for apps that subscribe to Smart Conversations features.
    ///     Note that this functionality is available for open beta testing.
    /// </summary>
    /// <param name="Enabled">
    ///     Set to true to allow messages processed by this app to be analyzed by Smart Conversations.
    /// </param>
    public record SmartConversation(bool Enabled);
}
