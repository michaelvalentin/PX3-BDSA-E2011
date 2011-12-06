// -----------------------------------------------------------------------
// <copyright file="DataCollector.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DigitalVoterList.Election
{
    using DigitalVoterList.Utilities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DataTransformer
    {
        public void TransformData()
        {
            var da = DAOFactory.getDAO(u);

            var people = da.LoadRawPeople();

            foreach (var rawPerson in people)
            {
                this.UpdatePerson(rawPerson);
                this.UpdateQuiz(rawPerson);
            }
        }

        private void UpdatePerson(RawPerson rawPerson)
        {   
            Person realPerson;

            var realPersonList = DAOFactory.GlobalDAO.Find(new Person() { Cpr = rawPerson.CPR });

            realPerson = (realPersonList.Count > 0) ? realPersonList[0] : realPerson = new Person();

            realPerson.Name = rawPerson.Name;
            realPerson.Cpr = rawPerson.CPR;
            realPerson.Address = rawPerson.Address;
            realPerson.PassportNumber = rawPerson.PassportNumber;
            realPerson.PlaceOfBirth = rawPerson.Birthplace;
            realPerson.EligibleToVote = CalculateEligibleToVote(realPerson);
            realPerson.VotingVenueId = CalculateVotingVenueId(realPerson);

            DAOFactory.GlobalDAO.Save(realPerson);
        }

        private void UpdateQuiz(RawPerson rawperson)
        {
            Quiz quiz;

            var realPersonList = DAOFactory.GlobalDAO.Find(new Person() { Cpr = rawPerson.CPR });

            if (realPersonList.Count > 0)? realPerson = realPersonList[0] : realPerson = new Person();

            realPerson.Name = rawPerson.Name;
            realPerson.Cpr = rawPerson.CPR;
            realPerson.Address = rawPerson.Address;
            realPerson.PassportNumber = rawPerson.PassportNumber;
            realPerson.PlaceOfBirth = rawPerson.Birthplace;


            DAOFactory.GlobalDAO.Save(realPerson);
        }

        //todo: Calculate voting venue Id
        private int CalculateVotingVenueId(Person person)
        {
            return 0;
        }

        //todo: Calculate eligible to vote
        private bool CalculateEligibleToVote(Person person)
        {
            return true;
        }



    }
}
