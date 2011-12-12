﻿/*
 * Authors:
 * Team: PX3
 * Date: 12-12-2011
 */

namespace DigitalVoterList.Election
{
    using System;

    /// <summary>
    /// An example of an election implementation. This contains rules regarding a specific kind of election.
    /// </summary>
    public class GeneralElection : ElectionEvent
    {
        private VotingVenue _globalVotingVenue;

        public GeneralElection(DateTime date, string name)
            : base(date, name)
        {
        }

        /// <summary>
        /// What votingVenue should be used for this citizen
        /// </summary>
        /// <returns></returns>
        public override VotingVenue VotingVenueForCitizen(RawPerson rawPerson)
        {
            /*
                This method should calculate there a citizen should vote from his data. This might be calculated from a list of zipcodes in the database,
             *  by looking at GPS coordinates or other stuff.
            */

            return new VotingVenue(1, "Dyssegårdskolens aula", "Dyssegårdsvej 34");
        }

        /// <summary>
        /// What votingVenue should be used for this citizen
        /// </summary>
        /// <returns></returns>
        public override bool CitizenEligibleToVote(RawPerson rawPerson)
        {
            /*
             * This method should calculate whether a citizen is eligible to vote or not. An example of such calculation is given here:
             */

            //Voter is disempowered to vote
            if (rawPerson.Disempowered) return false;

            //Person is too young
            if (rawPerson.Age < 18) return false;

            //Person is not danish
            if (rawPerson.Nationality != "DNK") return false;

            //Person is dead
            if (rawPerson.Alive == false) return false;

            return true;
        }
    }
}
