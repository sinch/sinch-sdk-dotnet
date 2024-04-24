using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Voice.Conferences.ManageParticipants
{
    public sealed class ManageParticipantRequest
    {
        /// <summary>
        ///     Action to apply on conference participant.
        /// </summary>
#if NET7_0_OR_GREATER
        public required Command Command { get; set; }
#else
        public Command Command { get; set; } = null!;
#endif
        /// <inheritdoc cref="MohClass" />
        public MohClass? Moh { get; set; }
    }

    [JsonConverter(typeof(EnumRecordJsonConverter<Command>))]
    public sealed record Command(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     Mutes participant.
        /// </summary>
        public static readonly Command Mute = new("mute");

        /// <summary>
        ///     Unmutes participant.
        /// </summary>
        public static readonly Command Unmute = new("unmute");

        /// <summary>
        ///     Puts participant on hold.
        /// </summary>
        public static readonly Command OnHold = new("onhold");

        /// <summary>
        ///     Returns participant to conference.
        /// </summary>
        public static readonly Command Resume = new("resume");
    }
}
