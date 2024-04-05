namespace Sinch.Voice.Calls.Instructions
{
    /// <summary>
    ///     Stops the recording of the call.
    /// </summary>
    public sealed class StopRecording : IInstruction
    {
        public string Name { get; } = "stopRecording";
    }
}
