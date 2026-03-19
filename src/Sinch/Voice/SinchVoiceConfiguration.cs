namespace Sinch.Voice
{
    public sealed class SinchVoiceConfiguration
    {
        public required string AppKey { get; init; }

        public required string AppSecret { get; init; }

        public string? VoiceUrlOverride { get; init; }

        public string? ApplicationManagementUrlOverride { get; init; }

        public VoiceRegion Region { get; init; } = VoiceRegion.Global;
    }
}
