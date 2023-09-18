namespace Sinch.Numbers.Active
{
    /// <summary>
    ///     Represents the order by options for sorting.
    /// </summary>
    public record OrderBy(string Value)
    {
        /// <summary>
        ///     Sort by phone number.
        /// </summary>
        public static readonly OrderBy PhoneNumber = new("phoneNumber");

        /// <summary>
        ///     Sort by display name.
        /// </summary>
        public static readonly OrderBy DisplayName = new("displayName");
    }
}
