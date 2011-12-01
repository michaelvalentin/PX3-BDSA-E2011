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
        private readonly ElectionEvent _electionEvent;
        private readonly Citizen _citizen;
        private readonly string _idKey;
        private string _id; //The database id
        private bool _valid;

        public VoterCard(ElectionEvent electionEvent, Citizen citizen)
        {
            Contract.Requires(!electionEvent.Equals(null));
            Contract.Requires(!citizen.Equals(null) && citizen.EligibleToVote);
            _electionEvent = electionEvent;
            _citizen = citizen;
            _valid = true;
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
        public string Id { get; set; }

        /// <summary>
        /// The id-key, corresponding to the barcode on the physical votercard
        /// </summary>
        public string IdKey { get; set; }

        /// <summary>
        /// A getter and setter for _valid. This says if the Voter Card is valid or not.
        /// </summary>
        // TODO: Make setter..
        public bool Valid { get; private set; }
    }
}
