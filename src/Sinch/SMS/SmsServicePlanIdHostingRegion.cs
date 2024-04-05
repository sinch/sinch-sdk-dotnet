namespace Sinch.SMS
{
    /// <summary>
    ///     The following regions can be set to be used in SDK as a hosting region when using `service_plan_id` option
    /// </summary>
    public record SmsServicePlanIdHostingRegion(string Value)
    {
        /// <summary>
        ///     USA
        /// </summary>
        public static readonly SmsServicePlanIdHostingRegion Us = new("us");

        /// <summary>
        ///     Ireland, Sweden
        /// </summary>
        public static readonly SmsServicePlanIdHostingRegion Eu = new("eu");

        /// <summary>
        ///     Australia
        /// </summary>
        public static readonly SmsServicePlanIdHostingRegion Au = new("au");

        /// <summary>
        ///     Brazil
        /// </summary>
        public static readonly SmsServicePlanIdHostingRegion Br = new("br");

        /// <summary>
        ///     Canada
        /// </summary>
        public static readonly SmsServicePlanIdHostingRegion Ca = new("ca");
    }
}
