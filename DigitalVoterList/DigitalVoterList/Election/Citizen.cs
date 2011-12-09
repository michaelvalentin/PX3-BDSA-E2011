using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using DigitalVoterList.Utilities;

namespace DigitalVoterList.Election
{

    /// <summary>
    /// A person with a valid and unique ID-number (CPR-number)
    /// </summary>
    public class Citizen : Person
    {
        private short _eligibleToVote = 0;
        private VotingVenue _votingPlace = null;

        public Citizen(int id, string cpr)
            : base(id)
        {
            Cpr = cpr;
        }

        private HashSet<VoterCard> _voterCards;

        private HashSet<Quiz> _securityQuestions;

        public bool HasVoted { get; private set; }

        public bool EligibleToVote
        {
            get
            {
                if(_eligibleToVote == 0)
            }
        }

        public VotingVenue VotingPlace { get; set; }

        public HashSet<VoterCard> VoterCards
        {
            get { return _voterCards ?? new HashSet<VoterCard>(); }
            set { _voterCards = value ?? new HashSet<VoterCard>(); }
        }

        public HashSet<Quiz> SecurityQuestions
        {
            get { return _securityQuestions ?? new HashSet<Quiz>(); }
            set { _securityQuestions = value ?? new HashSet<Quiz>(); }
        }


        public void SetHasVoted()
        {
            IDataAccessObject dao = DAOFactory.CurrentUserDAO;
            dao.SetHasVoted(this);
            HasVoted = true;
        }

        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant()
        {
            Contract.Invariant(ValidCpr(Cpr));
        }

        private bool ValidCpr(string cpr)
        {
            string tempCpr = cpr;
            if (tempCpr.Length == 10)
            {
                int day = Int32.Parse(tempCpr.Substring(0, 1));
                int month = Int32.Parse(tempCpr.Substring(2, 3));
                if (!(day > 0 && day <= 31)) return false;
                if (!(month > 0 && month <= 12)) return false;

                return true;
            }
            return false;
        }
    }
}
