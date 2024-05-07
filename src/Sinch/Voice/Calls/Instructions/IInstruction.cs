using System.Text.Json.Serialization;

namespace Sinch.Voice.Calls.Instructions
{
    /// <summary>
    ///     Marker interface for Call's instructions.
    /// </summary>
    [JsonDerivedType(typeof(Answer))]
    [JsonDerivedType(typeof(Say))]
    [JsonDerivedType(typeof(PlayFiles))]
    [JsonDerivedType(typeof(SendDtmf))]
    [JsonDerivedType(typeof(SetCookie))]
    [JsonDerivedType(typeof(StartRecording))]
    [JsonDerivedType(typeof(StopRecording))]
    public interface IInstruction
    {
        [JsonPropertyName("name")]
        public string Name { get; }
    }
}
