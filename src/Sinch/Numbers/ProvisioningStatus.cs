using System;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Numbers
{
    [JsonConverter(typeof(SinchEnumConverter<ProvisioningStatus>))]
    public enum ProvisioningStatus
    {
        [EnumMember(Value = "WAITING")]
        Waiting,

        [EnumMember(Value = "IN_PROGRESS")]
        InProgress,

        [EnumMember(Value = "FAILED")]
        Failed
    }
}
