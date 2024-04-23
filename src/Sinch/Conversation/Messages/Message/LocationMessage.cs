using System.Text;

namespace Sinch.Conversation.Messages.Message
{
    /// <summary>
    ///     Message containing geographic location.
    /// </summary>
    public sealed class LocationMessage
    {
        /// <summary>
        ///     Gets or Sets Coordinates
        /// </summary>
#if NET7_0_OR_GREATER
        public required Coordinates Coordinates { get; set; }
#else
        public Coordinates Coordinates { get; set; } = null!;
#endif


        /// <summary>
        ///     Label or name for the position.
        /// </summary>
        public string? Label { get; set; }


        /// <summary>
        ///     The title is shown close to the button or link that leads to a map showing the location. The title can be clickable in some cases.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string Title { get; set; }
#else
        public string Title { get; set; } = null!;
#endif


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class LocationMessage {\n");
            sb.Append("  Coordinates: ").Append(Coordinates).Append("\n");
            sb.Append("  Label: ").Append(Label).Append("\n");
            sb.Append("  Title: ").Append(Title).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    /// <summary>
    ///     Coordinates
    /// </summary>
    public record Coordinates(float Latitude, float Longitude)
    {
        /// <summary>
        ///     The latitude.
        /// </summary>
#if NET7_0_OR_GREATER
        public float Latitude { get; init; } = Latitude;
#else
        public float Latitude { get; } = Latitude;
#endif


        /// <summary>
        ///     The longitude.
        /// </summary>
#if NET7_0_OR_GREATER
        public float Longitude { get; init; } = Longitude;
#else
        public float Longitude { get; } = Longitude;
#endif


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class Coordinates {\n");
            sb.Append("  Latitude: ").Append(Latitude).Append("\n");
            sb.Append("  Longitude: ").Append(Longitude).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
