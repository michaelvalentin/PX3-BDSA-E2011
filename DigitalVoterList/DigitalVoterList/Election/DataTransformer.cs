// -----------------------------------------------------------------------
// <copyright file="DataCollector.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DigitalVoterList.Election
{
    using System.Collections.Generic;

    using DigitalVoterList.Utilities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DataTransformer
    {
        public void TransformData()
        {
            DAOFactory.CurrentUserDAO.UpdatePeople(this.UpdateCitizen);
        }

        /// <summary>
        /// What citizen would I get if I gave him/her this rawPersons information?
        /// </summary>
        /// <param name="person"></param>
        /// <param name="rawPerson"></param>
        /// <returns></returns>
        private Citizen UpdateCitizen(Citizen citizen, RawPerson rawPerson)
        {
            citizen.Name = rawPerson.Name;
            citizen.Cpr = rawPerson.CPR;
            citizen.Address = rawPerson.Address;
            citizen.PassportNumber = rawPerson.PassportNumber;
            citizen.PlaceOfBirth = rawPerson.Birthplace;
            citizen.EligibleToVote = Settings.Election.CitizenEligibleToVote(rawPerson);
            citizen.SecurityQuestions = this.GenerateSecurityQuestions(rawPerson);
            citizen.VotingPlace = Settings.Election.VotingVenueForCitizen(rawPerson); //
            return citizen;
        }

        private HashSet<Quiz> GenerateSecurityQuestions(RawPerson rawPerson)
        {
            var quizzes = new HashSet<Quiz>();

            if (rawPerson.Birthplace != null) quizzes.Add(new Quiz("Where were you born?", rawPerson.Birthplace));
            if (rawPerson.Education != null) quizzes.Add(new Quiz("What is your education?", rawPerson.Education));

            return quizzes;
        }
    }
}
