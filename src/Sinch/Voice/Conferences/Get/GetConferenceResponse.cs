using System.Collections.Generic;
using System.Text;

namespace Sinch.Voice.Conferences.Get
{
    public sealed class GetConferenceResponse
    {
        public List<Participant> Participants { get; set; }
    }

    public sealed class Participant
    {
        /// <summary>
        ///     The phone number of the PSTN participant that was connected in the conference, or whatever was passed as CLI for data originated/terminated calls.
        /// </summary>
        public string Cli { get; set; }


        /// <summary>
        ///     The callId of the call leg that the participant joined the conference.
        /// </summary>
        public string Id { get; set; }


        /// <summary>
        ///     The number of seconds that the participant has been connected to the conference.
        /// </summary>
        public int Duration { get; set; }


        /// <summary>
        ///     Gets or Sets Muted
        /// </summary>
        public bool Muted { get; set; }


        /// <summary>
        ///     Gets or Sets Onhold
        /// </summary>
        public bool Onhold { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Participant {\n");
            sb.Append("  Cli: ").Append(Cli).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Duration: ").Append(Duration).Append("\n");
            sb.Append("  Muted: ").Append(Muted).Append("\n");
            sb.Append("  Onhold: ").Append(Onhold).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
