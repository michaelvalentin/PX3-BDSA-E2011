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
        private readonly string _id;
        private bool _valid;

        public VoterCard(ElectionEvent electionEvent, Citizen citizen, string id)
        {
            Contract.Requires(!electionEvent.Equals(null));
            Contract.Requires(!citizen.Equals(null) && citizen.EligibleVoter);
            Contract.Requires(!string.IsNullOrEmpty(id));
            _electionEvent = electionEvent;
            _citizen = citizen;
            _id = id;
            _valid = true;
        }

        /// <summary>
        /// A getter for _electionEvent, which is the ElectionEvent that the Voter Card is attached to.
        /// </summary>
        public ElectionEvent ElectionEvent { get; private set; }

        /// <summary>
        /// A getter for _citizen. The citizen is the owner of the Voter Card.
        /// </summary>
        public Citizen Citizen { get; private set; }

        /// <summary>
        /// A getter for _id. The id is to identify the Voter Card.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// A getter and setter for _valid. This says if the Voter Card is valid or not.
        /// </summary>
        public bool Valid { get; set; }
    }
}
