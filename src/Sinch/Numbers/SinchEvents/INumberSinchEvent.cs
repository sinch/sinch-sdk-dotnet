using System;

namespace Sinch.Numbers.SinchEvents
{
    /// <summary>
    ///     Represents a Sinch Numbers event notification.
    /// </summary>
    public interface INumberSinchEvent
    {
        /// <summary>
        ///     The ID of the event.
        /// </summary>
        string? EventId { get; }

        /// <summary>
        ///     The date and time when the Sinch event was created and added to the Sinch events queue.
        /// </summary>
        DateTime? Timestamp { get; }

        /// <summary>
        ///     The ID of the project to which the event belongs.
        /// </summary>
        string? ProjectId { get; }

        /// <summary>
        ///     The unique identifier of the resource, depending on the resource type.
        ///     For example, a phone number, a hosting order ID, or a brand ID.
        /// </summary>
        string? ResourceId { get; }

        /// <summary>
        ///     The type of the resource.
        /// </summary>
        ResourceType? ResourceType { get; }

        /// <summary>
        ///     The type of the event.
        /// </summary>
        EventType? EventType { get; }

        /// <summary>
        ///     The status of the event.
        /// </summary>
        EventStatus? Status { get; }

        /// <summary>
        ///     If the status is FAILED, a failure code will be provided.
        ///     For numbers provisioning to SMS platform, there won't be any extra failureCode, as the result is binary.
        /// </summary>
        FailureCode? FailureCode { get; }
    }
}

