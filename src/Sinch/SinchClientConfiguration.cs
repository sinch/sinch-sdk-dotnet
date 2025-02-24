using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Sinch.Auth;
using Sinch.Conversation;
using Sinch.Fax;
using Sinch.Numbers;
using Sinch.SMS;

namespace Sinch
{
    public sealed class SinchClientConfiguration
    {
        public SinchCommonCredentials? SinchCommonCredentials { get; init; }

        /// <summary>
        ///     Optional. See: <see cref="SinchOptions" />
        /// </summary>
        public SinchOptions? SinchOptions { get; init; }

        public SinchNumbersConfiguration NumbersConfiguration { get; init; } = new SinchNumbersConfiguration();

        public OAuthConfiguration OAuthConfiguration { get; init; } = new OAuthConfiguration();

        public SinchSmsConfiguration SmsConfiguration { get; init; } = new SinchSmsConfiguration();

        public SinchConversationConfiguration ConversationConfiguration { get; init; } =
            new SinchConversationConfiguration();

        public SinchFaxConfiguration FaxConfiguration { get; init; } = new SinchFaxConfiguration();
    }

    public sealed class SinchCommonCredentials
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

        internal void Validate()
        {
            var exceptions = new List<Exception>();
            if (string.IsNullOrEmpty(ProjectId))
                exceptions.Add(new InvalidOperationException($"{nameof(ProjectId)} should have a value"));
            
            if (string.IsNullOrEmpty(KeyId))
                exceptions.Add(new InvalidOperationException($"{nameof(KeyId)} should have a value"));

            if (string.IsNullOrEmpty(KeySecret))
                exceptions.Add(new InvalidOperationException($"{nameof(KeySecret)} should have a value"));

            if (exceptions.Any()) throw new AggregateException("Credentials are missing", exceptions);
        }
    }
}
