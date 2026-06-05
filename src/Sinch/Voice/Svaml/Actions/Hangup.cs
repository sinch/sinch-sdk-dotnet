namespace Sinch.Voice.Svaml.Actions
{
    /// <summary>
    ///      Hangs up a call.
    /// </summary>
    public sealed class Hangup : IAction
    {
        public string Name { get; } = "hangup";
    }
}
