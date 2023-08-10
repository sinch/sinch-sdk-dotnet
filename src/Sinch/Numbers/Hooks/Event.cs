using System;

namespace Sinch.Numbers.Hooks
{
    /// <summary>
    ///     A notification of an event sent to your configured callback URL.
    /// </summary>
    public class Event
    {
        /// <summary>
        ///     The ID of the event.
        /// </summary>
        public string EventId { get; set; }
        
        /// <summary>
        ///     The date and time when the callback was created and added to the callbacks queue.
        /// </summary>
        public DateTime Timestamp { get; set; }
        
        /// <summary>
        ///     The ID of the project to which the event belongs.
        /// </summary>
        public string ProjectId { get; set; }
        
        /// <summary>
        ///     The unique identifier of the resource, depending on the resource type.
        ///     For example, a phone number, a hosting order ID, or a brand ID.
        /// </summary>
        public string ResourceId { get; set; }

        /// <summary>
        ///     The type of the resource. 
        /// </summary>
        public ResourceType ResourceType { get; set; }
    
        /// <summary>
        ///     The type of the event.
        /// </summary>
        public EventType EventType { get; set; }
        
        /// <summary>
        ///     The status of the event
        /// </summary>
        public EventStatus Status { get; set; }
        
        /// <summary>
        ///     If the status is FAILED, a failure code will be provided.
        ///     For numbers provisioning to SMS platform, there won't be any extra failureCode, as the result is binary.
        /// </summary>
        public FailureCode? FailureCode { get; set; }
    }
}
