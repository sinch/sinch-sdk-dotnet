using System.Runtime.Serialization;

namespace Sinch.Numbers.Active
{
    public enum OrderBy
    {
        [EnumMember(Value = "phoneNumber")]
        PhoneNumber,
        [EnumMember(Value = "displayName")]
        DisplayName
    }

    internal static class Extension
    {
        internal static string ToRequiredString(this OrderBy orderBy)
        {
            // unfortunately, non exhaustive pattern matching not working as expected and forces to cover _ case
#pragma warning disable CS8524 // The switch expression does not handle some values of its input type (it is not exhaustive) involving an unnamed enum value.
            return orderBy switch
            {
                OrderBy.PhoneNumber => "phoneNumber",
                OrderBy.DisplayName => "displayName"
            };
#pragma warning restore CS8524 // The switch expression does not handle some values of its input type (it is not exhaustive) involving an unnamed enum value.
        }
    }
}
