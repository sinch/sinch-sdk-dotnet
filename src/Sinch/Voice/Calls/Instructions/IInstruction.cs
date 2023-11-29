using System.Text.Json.Serialization;

namespace Sinch.Voice.Calls.Instructions
{
    [JsonDerivedType(typeof(Answer))]
    [JsonDerivedType(typeof(Say))]
    [JsonDerivedType(typeof(PlayFiles))]
    [JsonDerivedType(typeof(SendDtmf))]
    [JsonDerivedType(typeof(SetCookie))]
    [JsonDerivedType(typeof(StartRecording))]
    [JsonDerivedType(typeof(StopRecording))]
    public interface IInstruction
    {
        public string Name { get; }
    }
}
