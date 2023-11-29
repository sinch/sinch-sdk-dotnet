using System.Text.Json.Serialization;

namespace Sinch.Voice.Calls.Actions
{
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
        public string Name { get; }
    }
}
