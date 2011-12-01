// -----------------------------------------------------------------------
// <copyright file="DAOMysql.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DigitalVoterList.Election
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DAOMysql : IDataAccessObject
    {

        public Person LoadPerson(int id)
        {
            throw new NotImplementedException();
        }

        public User LoadUser(string username, string password)
        {
            throw new NotImplementedException();
        }

        public User LoadUser(int id)
        {
            throw new NotImplementedException();
        }

        public VoterCard LoadVoterCard(int id)
        {
            throw new NotImplementedException();
        }

        public VoterCard LoadVoterCard(string idKey)
        {
            throw new NotImplementedException();
        }

        public List<Person> Find(Person person)
        {
            throw new NotImplementedException();
        }

        public List<User> Find(User user)
        {
            throw new NotImplementedException();
        }

        public List<VoterCard> Find(VoterCard voterCard)
        {
            throw new NotImplementedException();
        }

        public List<Citizen> Find()
        {
            throw new NotImplementedException();
        }

        public bool Save(Person person)
        {
            throw new NotImplementedException();
        }

        public bool Save(User user)
        {
            throw new NotImplementedException();
        }

        public bool Save(VoterCard voterCard)
        {
            throw new NotImplementedException();
        }

        public bool Mark(Citizen citizen, int keyPhrase)
        {
            throw new NotImplementedException();
        }

        public bool Mark(Citizen citizen)
        {
            throw new NotImplementedException();
        }

        public bool ChangePassword(User user)
        {
            throw new NotImplementedException();
        }

        public bool InvalidUser(User user)
        {
            throw new NotImplementedException();
        }

        public bool InvalidVoterCard(VoterCard voterCard)
        {
            throw new NotImplementedException();
        }
    }
}
