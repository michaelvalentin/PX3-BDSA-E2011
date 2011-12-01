// -----------------------------------------------------------------------
// <copyright file="Citizen.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
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
        private bool _eligibleToVote;
        private bool _hasVoted;
        private HashSet<VoterCard> _voterCards = null;
        private HashSet<Quiz> _securityQuestions = null;

        public Citizen(int id)
            : base(id)
        {
        }

        //TODO: Make this... :-)
        public bool EligibleToVote { get; set; }

        //TODO: Make this... :-)
        public bool HasVoted { get; private set; }

        //TODO: Make this... :-)
        public HashSet<VoterCard> VoterCards { get; set; }

        public HashSet<Quiz> SecurityQuestions { get; set; }

        private void ObjectInvariant()
        {
            Contract.Invariant(ValidCpr(Cpr));
        }

        private bool ValidCpr(int cpr)
        {
            string tempCpr = cpr.ToString();
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
