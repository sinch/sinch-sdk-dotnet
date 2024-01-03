using System.Text.Json.Serialization;

namespace Sinch.Voice.Calls.Actions
{
    /// <summary>
    ///     Marker interface for Call Actions.
    /// </summary>
    [JsonDerivedType(typeof(ConnectConf))]
    [JsonDerivedType(typeof(ConnectMxp))]
    [JsonDerivedType(typeof(ConnectPstn))]
    [JsonDerivedType(typeof(ConnectSip))]
    [JsonDerivedType(typeof(Continue))]
    [JsonDerivedType(typeof(Hangup))]
    [JsonDerivedType(typeof(Park))]
    [JsonDerivedType(typeof(RunMenu))]
    public interface IAction
    {
        [JsonPropertyName("name")]
        public string Name { get; }
    }
}
