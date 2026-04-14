using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sinch.Core
{
    /// <summary>
    ///     Non-generic marker interface used by <see cref="Http" /> to detect
    ///     <see cref="Optional{T}.Unset" /> without reflection at serialization time.
    /// </summary>
    internal interface IOptional
    {
        /// <summary><c>true</c> when the state is <see cref="Optional{T}.Unset" />.</summary>
        bool IsUnset { get; }
    }

    /// <summary>
    ///     Represents an optional field in a partial-update (PATCH / merge-patch) request, following
    ///     <see href="https://www.rfc-editor.org/rfc/rfc7396">RFC 7396 — JSON Merge Patch</see> semantics.
    ///     <para>
    ///         Three distinct states are supported:
    ///         <list type="bullet">
    ///             <item>
    ///                 <see cref="Unset{T}" /> — field was not supplied by the caller; it is
    ///                 <b>omitted entirely</b> from the serialized JSON payload.
    ///             </item>
    ///             <item>
    ///                 <see cref="Null{T}" /> — caller explicitly set the field to <c>null</c>;
    ///                 it is serialized as <c>"field": null</c>, signalling the server to clear the value.
    ///             </item>
    ///             <item>
    ///                 <see cref="Value{T}" /> — caller supplied a concrete value; it is serialized
    ///                 normally as <c>"field": &lt;value&gt;</c>.
    ///             </item>
    ///         </list>
    ///     </para>
    ///     <para>
    ///         The <see cref="OptionalJsonConverterFactory" /> is registered via the
    ///         <see cref="JsonConverterAttribute" /> on this type, so no per-property
    ///         <c>[JsonConverter]</c> decoration is needed on request DTOs.
    ///     </para>
    /// </summary>
    /// <typeparam name="T">The type of the underlying value.</typeparam>
    [JsonConverter(typeof(OptionalJsonConverterFactory))]
    public abstract record Optional<T> : IOptional
    {
        bool IOptional.IsUnset => this is Unset;

        /// <summary>Field was not provided — will be omitted from the serialized JSON payload.</summary>
        public sealed record Unset : Optional<T>;

        /// <summary>Field was explicitly set to <c>null</c> — will serialize as <c>null</c> in JSON.</summary>
        public sealed record Null : Optional<T>;

        /// <summary>Field was set to a concrete value — will serialize with that value.</summary>
        public sealed record Value(T Data) : Optional<T>;

        /// <summary>Creates an instance representing "field not provided".</summary>
        public static Optional<T> CreateUnset() => new Unset();

        /// <summary>Creates an instance representing "field explicitly set to null".</summary>
        public static Optional<T> CreateNull() => new Null();

        /// <summary>Creates an instance representing "field set to a value".</summary>
        public static Optional<T> CreateValue(T data) => new Value(data);
    }

    /// <summary>
    ///     <see cref="JsonConverterFactory" /> that resolves <see cref="OptionalJsonConverter{T}" />
    ///     for any closed <see cref="Optional{T}" /> type.  Registered via the
    ///     <see cref="JsonConverterAttribute" /> on <see cref="Optional{T}" />, so STJ resolves it
    ///     automatically — no per-property decoration required.
    /// </summary>
    internal sealed class OptionalJsonConverterFactory : JsonConverterFactory
    {
        /// <inheritdoc />
        public override bool CanConvert(Type typeToConvert)
            => typeToConvert.IsGenericType &&
               typeToConvert.GetGenericTypeDefinition() == typeof(Optional<>);

        /// <inheritdoc />
        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var innerType = typeToConvert.GetGenericArguments()[0];
            var converterType = typeof(OptionalJsonConverter<>).MakeGenericType(innerType);
            return (JsonConverter)Activator.CreateInstance(converterType)!;
        }
    }

    /// <summary>
    ///     Serializes and deserializes <see cref="Optional{T}" /> values.
    ///     <list type="bullet">
    ///         <item><see cref="Optional{T}.Unset" /> — writes nothing (property omitted from output).</item>
    ///         <item><see cref="Optional{T}.Null" /> — writes <c>null</c>.</item>
    ///         <item><see cref="Optional{T}.Value" /> — writes the inner value using the standard serializer.</item>
    ///     </list>
    /// </summary>
    internal sealed class OptionalJsonConverter<T> : JsonConverter<Optional<T>>
    {
        /// <inheritdoc />
        public override Optional<T> Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return Optional<T>.CreateNull();

            var value = JsonSerializer.Deserialize<T>(ref reader, options);
            return Optional<T>.CreateValue(value!);
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, Optional<T> value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case Optional<T>.Unset:
                    // Field omitted intentionally — nothing to write.
                    return;
                case Optional<T>.Null:
                    writer.WriteNullValue();
                    return;
                case Optional<T>.Value v:
                    JsonSerializer.Serialize(writer, v.Data, options);
                    return;
            }
        }
    }
}
