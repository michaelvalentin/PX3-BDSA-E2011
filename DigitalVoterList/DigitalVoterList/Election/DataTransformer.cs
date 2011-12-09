// -----------------------------------------------------------------------
// <copyright file="DataCollector.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DigitalVoterList.Election
{
    using System;
    using System.Collections.Generic;

    using DigitalVoterList.Utilities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DataTransformer
    {
        private ElectionEvent _electionEvent; //todo: Can we refer to the current electionEvent somehow?

        public void TransformData(ElectionEvent electionEvent)
        {
            _electionEvent = electionEvent;

            DAOFactory.CurrentUserDAO.UpdatePeople(new Func<Person, RawPerson, Person>(UpdatePerson));
        }

        /// <summary>
        /// Change this Person with this raw person data, and return the changed person.
        /// </summary>
        /// <param name="person"></param>
        /// <param name="rawPerson"></param>
        /// <returns></returns>
        private Person UpdatePerson(Person person, RawPerson rawPerson)
        {
            person.Name = rawPerson.Name;
            person.Cpr = rawPerson.CPR;
            person.Address = rawPerson.Address;
            person.PassportNumber = rawPerson.PassportNumber;
            person.PlaceOfBirth = rawPerson.Birthplace;

            if (person is Citizen)
            {
                var citizen = (Citizen)person;
                //citizen.EligibleToVote = CalculateEligibleToVote(rawPerson); //TODO: FIX!
                citizen.SecurityQuestions = this.GenerateSecurityQuestions(rawPerson);
                citizen.VotingPlace = _electionEvent.VotingVenueForCitizen(citizen);
                return citizen;
            }
            return person;
        }

        private HashSet<Quiz> GenerateSecurityQuestions(RawPerson rawPerson)
        {
            var quizzes = new HashSet<Quiz>();

            if (rawPerson.Birthplace != null) quizzes.Add(new Quiz("Where were you born?", rawPerson.Birthplace));
            if (rawPerson.Education != null) quizzes.Add(new Quiz("What is your education?", rawPerson.Education));

            return quizzes;
        }

        //todo: Calculate eligible to vote better
        private bool CalculateEligibleToVote(RawPerson rawPerson)
        {
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
