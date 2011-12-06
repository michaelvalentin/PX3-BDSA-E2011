// -----------------------------------------------------------------------
// <copyright file="DataCollector.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DigitalVoterList.Election
{

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DataTransformer
    {



        public void TransformData(User u)
        {
            var da = DAOFactory.getDAO(u);

            var people = da.LoadRawPeople();

            foreach (var rawPerson in people)
            {
                //Find rawPerson.cpr
                da.Find(new Person().Cpr = rawPerson.CPR)
                //Person med cpr i db
                //Hvis person findes, hent ham ud og update,
                //Ellers opret ny person


            }
        }

        private void InsertIntoPerson(RawPerson rawPerson)
        {


        }

        private void InsertIntoQuiz(Person person)
        {

        }
    }
}
