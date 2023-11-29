namespace Sinch.Voice.Calls.Instructions
{
    /// <summary>
    ///     Starts a recording of the call.
    /// </summary>
    public sealed class StartRecording : IInstruction
    {
        public string Name { get; } = "startRecording";

        /// <summary>
        ///     An object that specifies details about the recording. For more details, see
        ///     https://developers.sinch.com/docs/in-app-calling/voice-recording/#recording-options
        /// </summary>
        // TODO: check if class is correct
        public StartRecordingOptions Options { get; set; }
    }

    /// <summary>
    ///     An object that specifies details about the recording. For more details, see
    ///     https://developers.sinch.com/docs/in-app-calling/voice-recording/#recording-options
    /// </summary>
    public sealed class StartRecordingOptions
    {
        /// <summary>
        ///     Where the recording file should be stored. Sinch supports the following destinations:
        ///     <list type="bullet">
        ///         <item>Amazon S3</item>
        ///         <item>Microsoft Azure Blob Storage</item>
        ///     </list>
        ///     To read more about how to configure each of these destinations, see the
        ///     DestinationURL(https://developers.sinch.com/docs/in-app-calling/voice-recording/#destinationurl) section.
        /// </summary>
        public string DestinationUrl { get; set; }

        /// <summary>
        ///     Specifies the information required for the Sinch platform to authenticate and/or authorize in the destination
        ///     service in order to be able to store the file.
        /// </summary>
        public string Credentials { get; set; }

        /// <summary>
        ///     An optional property that specifies the format of the recording file. Default value is mp3.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        ///     An optional property that specifies if Sinch should send your backend events when the call recording is finished
        ///     and when the file is available in the destination URL specified. For more information about notification events,
        ///     see the Notification Events (https://developers.sinch.com/docs/in-app-calling/voice-recording/#notification-events)
        ///     section. Default value is “true”
        /// </summary>
        public bool? NotificationEvents { get; set; }
    }
}
