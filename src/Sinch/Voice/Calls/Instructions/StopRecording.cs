namespace Sinch.Voice.Calls.Instructions
{
    /// <summary>
    ///     Stops the recording of the call.
    /// </summary>
    public sealed class StopRecording : Instruction
    {
        public override string Name { get; } = "stopRecording";
    }
}
