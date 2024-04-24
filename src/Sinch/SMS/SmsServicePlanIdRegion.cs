namespace Sinch.SMS
{
    /// <summary>
    ///     The following regions can be set to be used in SDK as a hosting region when using `service_plan_id` option
    /// </summary>
    public record SmsServicePlanIdRegion(string Value)
    {
        /// <summary>
        ///     USA
        /// </summary>
        public static readonly SmsServicePlanIdRegion Us = new("us");

        /// <summary>
        ///     Ireland, Sweden
        /// </summary>
        public static readonly SmsServicePlanIdRegion Eu = new("eu");

        /// <summary>
        ///     Australia
        /// </summary>
        public static readonly SmsServicePlanIdRegion Au = new("au");

        /// <summary>
        ///     Brazil
        /// </summary>
        public static readonly SmsServicePlanIdRegion Br = new("br");

        /// <summary>
        ///     Canada
        /// </summary>
        public static readonly SmsServicePlanIdRegion Ca = new("ca");
    }
}
