namespace Sinch.SMS.Hooks
{
    /// <summary>
    ///     Base interface for all inbound message types.
    /// </summary>
    /// <seealso cref="ISmsWebhooks.ParseEvent"/>
    public interface IInboundMessage : ISmsEvent
    {
    }
}
