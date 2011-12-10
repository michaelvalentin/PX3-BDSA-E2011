// -----------------------------------------------------------------------
// <copyright file="VoterCard.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DigitalVoterList.Election
{

    /// <summary>
    /// A ticket to be exchanged for a ballot, that is send out to the voter as a part of the validation process.
    /// </summary>
    public class VoterCard
    {
        /// <summary>
        /// The ElectionEvent that the Voter Card is attached to.
        /// </summary>
        public ElectionEvent ElectionEvent { get; set; }

        /// <summary>
        /// The citizen, who is the owner of the Voter Card.
        /// </summary>
        public Citizen Citizen { get; set; }

        /// <summary>
        /// The database id of the Voter Card.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The id-key, corresponding to the barcode on the physical votercard
        /// </summary>
        public string IdKey { get; set; }

        public bool MarkAsInvalid()
        {
            Valid = false;
            return true;
        }

        /// <summary>
        /// A getter and setter for _valid. This says if the Voter Card is valid or not.
        /// </summary>
        public bool Valid { get; set; }
    }
}
