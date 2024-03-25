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
        public static readonly SmsHostingRegion Us = new("us");

        /// <summary>
        ///     Ireland, Sweden
        /// </summary>
        public static readonly SmsHostingRegion Eu = new("eu");
        
        /// <summary>
        ///     Australia
        /// </summary>
        public static readonly SmsHostingRegion Au = new("au");
        
        /// <summary>
        ///     Brazil
        /// </summary>
        public static readonly SmsHostingRegion Br = new("br");
        
        /// <summary>
        ///     Canada
        /// </summary>
        public static readonly SmsHostingRegion Ca = new("ca");
    }
}
