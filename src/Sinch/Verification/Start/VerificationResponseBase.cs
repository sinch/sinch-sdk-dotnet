﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Verification.Start
{
    public abstract class VerificationResponseBase
    {
        /// <summary>
        ///     Verification identifier used to query for status.
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        ///     The value of the method used for the Verification.
        /// </summary>
        public string Method { get; set; }
        
        /// <summary>
        ///     Available methods and actions which can be done after a successful Verification
        /// </summary>
        [JsonPropertyName("_links")]
        public List<Links> Links { get; set; }
    }
    
    /// <summary>
    ///     A marker interface for VerificationResponse items.
    /// </summary>
    [JsonConverter(typeof(VerificationResponseConverter))]
    public interface IVerificationResponse
    {
        
    }

    public class VerificationResponseConverter : JsonConverter<IVerificationResponse>
    {
        public override IVerificationResponse Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            var elem = JsonElement.ParseValue(ref reader);
            var descriptor = elem.EnumerateObject().FirstOrDefault(x => x.Name == "method");
            return descriptor.Value.GetString() switch
            {
                "sms" => (SmsResponse)elem.Deserialize(typeof(SmsResponse), options),
                "callout" => (PhoneCallResponse)elem.Deserialize(typeof(PhoneCallResponse), options),
                "flashCall" => (FlashCallResponse) elem.Deserialize(typeof(FlashCallResponse), options) ,
                "seamless" => (DataResponse) elem.Deserialize(typeof(DataResponse), options),
                _ => throw new JsonException($"Failed to match verification method object, got {descriptor.Name}")
            };
        }

        public override void Write(Utf8JsonWriter writer, IVerificationResponse value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}
