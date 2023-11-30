namespace Sinch.Voice.Calls.Actions
{
    /// <summary>
    ///      Hangs up a call.
    /// </summary>
    public class Hangup : IAction
    {
        public string Name { get; } = "hangup";
    }
}
