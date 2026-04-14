using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Numbers.VoiceConfigurations
{
    /// <summary>
    ///     Handles <see cref="Optional{T}" /> wrapping <see cref="VoiceConfiguration" />.
    ///     <para>
    ///         <see cref="VoiceConfiguration" /> is polymorphic and requires the dedicated
    ///         <see cref="VoiceConfigurationConverter" /> for inner value discrimination.
    ///         This converter handles the <see cref="Optional{T}" /> envelope
    ///         (unset → omit, null → write null, value → delegate to <see cref="VoiceConfigurationConverter" />).
    ///     </para>
    /// </summary>
    internal sealed class OptionalVoiceConfigurationConverter : JsonConverter<Optional<VoiceConfiguration>>
    {
        private static readonly VoiceConfigurationConverter InnerConverter = new();

        public override Optional<VoiceConfiguration> Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return Optional<VoiceConfiguration>.CreateNull();

            var value = InnerConverter.Read(ref reader, typeof(VoiceConfiguration), options);
            return value is null
                ? Optional<VoiceConfiguration>.CreateNull()
                : Optional<VoiceConfiguration>.CreateValue(value);
        }

        public override void Write(Utf8JsonWriter writer, Optional<VoiceConfiguration> value,
            JsonSerializerOptions options)
        {
            switch (value)
            {
                case Optional<VoiceConfiguration>.Unset:
                    return;
                case Optional<VoiceConfiguration>.Null:
                    writer.WriteNullValue();
                    return;
                case Optional<VoiceConfiguration>.Value v:
                    InnerConverter.Write(writer, v.Data, options);
                    return;
            }
        }
    }
}
