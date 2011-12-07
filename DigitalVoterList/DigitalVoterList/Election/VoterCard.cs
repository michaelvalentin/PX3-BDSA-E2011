// -----------------------------------------------------------------------
// <copyright file="VoterCard.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
using System.Diagnostics.Contracts;

namespace DigitalVoterList.Election
{

    /// <summary>
    /// A ticket to be exchanged for a ballot, that is send out to the voter as a part of the validation process.
    /// </summary>
    public class VoterCard
    {
        private string _idKey;
        private int _id; //The database id

        public VoterCard(ElectionEvent electionEvent, Citizen citizen)
        {
            Contract.Requires(!electionEvent.Equals(null));
            Contract.Requires(!citizen.Equals(null) && citizen.EligibleToVote);
            ElectionEvent = electionEvent;
            Citizen = citizen;
            Valid = true;
        }

        /// <summary>
        /// The ElectionEvent that the Voter Card is attached to.
        /// </summary>
        public ElectionEvent ElectionEvent { get; private set; }

        /// <summary>
        /// The citizen, who is the owner of the Voter Card.
        /// </summary>
        public Citizen Citizen { get; private set; }

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
        // TODO: Make setter..
        public bool Valid { get; private set; }
    }
}
