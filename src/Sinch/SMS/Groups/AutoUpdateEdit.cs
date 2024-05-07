namespace Sinch.SMS.Groups
{
    public class AutoUpdateEdit
    {
        /// <summary>
        ///     Short code or long number addressed in
        ///     <see href="https://community.sinch.com/t5/Glossary/MO-Mobile-Originated/ta-p/7618">MO</see>.
        /// </summary>
        public string? To { get; set; }

        /// <summary>
        ///     /// Keyword to be sent in MO to add to a group.
        /// </summary>
        public AutoUpdateAdd? Add { get; set; }

        /// <summary>
        ///     Keyword to be sent in MO to remove from a group.
        /// </summary>
        public AutoUpdateRemove? Remove { get; set; }
    }

    public class AutoUpdateAdd
    {
        /// <summary>
        ///     Keyword to be sent in
        ///     <see href="https://community.sinch.com/t5/Glossary/MO-Mobile-Originated/ta-p/7618">MO</see>
        ///     to add phone number to a group opt-in keyword like "JOIN".
        ///     If auto_update.to is dedicated long/short number or unique brand keyword like "Sinch"
        ///     if it is a shared short code.
        ///     <br /><br />
        ///     Constraints: Must be one word.
        /// </summary>
        public string? FirstWord { get; set; }

        /// <summary>
        ///     Opt-in keyword like "JOIN" if auto_update.to is shared short code.<br /><br />
        ///     Constraints: Must be one word.
        /// </summary>
        public string? SecondWord { get; set; }
    }

    public class AutoUpdateRemove
    {
        /// <summary>
        ///     Opt-out keyword like "LEAVE" If auto_update.to
        ///     is dedicated long/short number or unique brand keyword like "Sinch" if it is a shared short code.
        ///     <br /><br />
        ///     Constraints: Must be one word.
        /// </summary>
        public string? FirstWord { get; set; }

        /// <summary>
        ///     Opt-out keyword like "LEAVE" if auto_update.to is shared short code.<br /><br />
        ///     Constraints: Must be one word.
        /// </summary>
        public string? SecondWord { get; set; }
    }
}
