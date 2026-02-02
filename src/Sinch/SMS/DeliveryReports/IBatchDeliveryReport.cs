namespace Sinch.SMS.DeliveryReports
{
    /// <summary>
    ///     Batch-level delivery report (summary across multiple recipients).
    /// </summary>
    public interface IBatchDeliveryReport : IDeliveryReport
    {
        DeliveryReportType Type { get; }
    }
}
