namespace Sinch.SMS.DeliveryReports
{
    /// <summary>
    ///     Per-recipient delivery report (status for a single recipient).
    /// </summary>
    public interface IRecipientDeliveryReport : IDeliveryReport
    {
        RecipientDeliveryReportType Type { get; set; }
    }
}
