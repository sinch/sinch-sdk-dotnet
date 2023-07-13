namespace Sinch.Numbers
{
    /// <summary>
    ///     https://developers.sinch.com/docs/numbers/api-reference/numbers/tag/Active-Number/#tag/Active-Number/operation/NumberService_ListActiveNumbers
    /// </summary>
    public sealed class SmsConfiguration
    {
        /// <summary>
        ///     The servicePlanId can be found in the
        ///     <see href="https://dashboard.sinch.com/sms/api/rest">Sinch Customer Dashboard</see>. The service plan ID is what
        ///     ties this to the configured SMS service.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string ServicePlanId { get; set; }
#else
        public string ServicePlanId { get; set; }
#endif


        /// <summary>
        ///     Only for US virtual numbers.
        ///     This campaign ID relates to
        ///     <see href="https://community.sinch.com/t5/10DLC/What-is-10DLC/ta-p/7845">10DLC numbers</see>.
        ///     So, it is the current campaign ID for this number. The campaign_id is found on your TCR platform.
        /// </summary>
        public string CampaignId { get; set; }

        /// <summary>
        ///     This object is temporary and will appear while the scheduled provisioning for SMS is processing.
        ///     Once it has successfully processed, only the ID of the SMS configuration will display.
        /// </summary>
        public ScheduledProvisioning ScheduledProvisioning { get; }
    }
}
