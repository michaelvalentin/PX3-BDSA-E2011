// -----------------------------------------------------------------------
// <copyright file="VoterCard.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
using System.Diagnostics.Contracts;

namespace DigitalVoterList.Election
{

    /// <summary>
    ///     A ticket to be exchanged for a ballot, that is send out to the voter as a part of the validation process.
    /// </summary>
    public class VoterCard
    {
        private readonly ElectionEvent _electionEvent;
        private readonly Citizen _citizen;
        private readonly string _id;

        public VoterCard(ElectionEvent electionEvent, Citizen citizen, string id)
        {
            _electionEvent = electionEvent;
            _citizen = citizen;
            _id = id;
        }

        public ElectionEvent ElectionEvent
        {
            get
            {
                return _electionEvent;
            }
        }

        public Citizen Citizen
        {
            get
            {
                return _citizen;
            }
        }

        public string Id
        {
            get
            {
                return _id;
            }
        }
    }
}
