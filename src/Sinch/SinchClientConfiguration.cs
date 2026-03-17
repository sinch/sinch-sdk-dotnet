using Sinch.Auth;
using Sinch.Conversation;
using Sinch.Fax;
using Sinch.Numbers;
using Sinch.SMS;
using Sinch.Verification;
using Sinch.Voice;

namespace Sinch
{
    public sealed class SinchClientConfiguration
    {
        public SinchUnifiedCredentials? SinchUnifiedCredentials { get; init; }

        /// <summary>
        ///     Optional. See: <see cref="SinchOptions" />
        /// </summary>
        public SinchOptions? SinchOptions { get; init; }

        public SinchNumbersConfiguration NumbersConfiguration { get; init; } = new();

        public SinchOAuthConfiguration SinchOAuthConfiguration { get; init; } = new();

        public SinchSmsConfiguration SmsConfiguration { get; init; } = new();

        public SinchConversationConfiguration ConversationConfiguration { get; init; } = new();

        public SinchFaxConfiguration FaxConfiguration { get; init; } = new();

        public SinchVerificationConfiguration? VerificationConfiguration { get; init; }
        public SinchVoiceConfiguration? VoiceConfiguration { get; init; }
    }

    public sealed class SinchUnifiedCredentials
    {
        /// <summary>
        ///     Your Sinch Account key id.
        /// </summary>
        public required string KeyId { get; init; }

        /// <summary>
        ///     Your Sinch Account key secret.
        /// </summary>
        public required string KeySecret { get; init; }

        /// <summary>
        ///     Your project id.
        /// </summary>
        public required string ProjectId { get; init; }
    }
}
