using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace DigitalVoterList.Election
{

    class DAOMySql : IDataAccessObject
    {
        private MySqlConnection _sqlConnection;
        private string _connectionString;
        private bool _isConnected;

        public DAOMySql()
        {
            this._connectionString = "Server=" + Settings.DbHost + ";" +
                                     "Database=" + Settings.DbName + ";" +
                                     "Uid=" + Settings.DbUser + ";" +
                                     "Pwd=" + Settings.DbPassword + ";";

            try
            {
                _sqlConnection = new MySqlConnection(this._connectionString);
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Error connecting you to " +
                    "the my sql server. Internal error message: " + excp.Message, excp);
                throw myExcp;
            }

            this._isConnected = false;
        }

        ~DAOMySql()
        {
            Disconnect();
        }

        private void Connect()
        {
            bool success = true;

            if (this._isConnected == false)
            {
                try
                {
                    this._sqlConnection.Open();
                }
                catch (Exception excp)
                {
                    this._isConnected = false;
                    success = false;
                    Exception myException = new Exception("Error opening connection" +
                        " to the sql server. Error: " + excp.Message, excp);

                    throw myException;
                }

                if (success)
                {
                    this._isConnected = true;
                }
            }
        }

        public void Disconnect()
        {
            if (this._isConnected)
            {
                this._sqlConnection.Close();
            }
        }

        public Person LoadPerson(int id)
        {
            Connect();
            string query = "SELECT * FROM person WHERE id=" + id + " LIMIT 1";
            MySqlCommand loadPerson = new MySqlCommand(query, this._sqlConnection);

            try
            {
                MySqlDataReader reader = loadPerson.ExecuteReader();
                if (!reader.Read()) throw new DataAccessException("No person with the supplied id could be found.");
                Person person = new Person(id);
                DoIfNotDbNull(reader, "name", lbl => person.Name = reader.GetString(lbl));
                person.Cpr = reader.GetInt32("cpr");
                DoIfNotDbNull(reader, "address", lbl => person.Address = reader.GetString(lbl));
                DoIfNotDbNull(reader, "place_of_birth", lbl => person.PlaceOfBirth = reader.GetString(lbl));
                DoIfNotDbNull(reader, "passport_number", lbl => person.PassportNumber = reader.GetInt32(lbl));
                return person;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /*
        public User LoadUser(string username, string pass)
        {
            Connect();
            string query = "SELECT * FROM user WHERE user_name='"+username+"' LIMIT 1";
            MySqlCommand loadUser = new MySqlCommand(query, this._sqlConnection);

            try
            {
                MySqlDataReader myReader = loadUser.ExecuteReader();
                myReader.Read();
                myReader.GetString("user_name");
                Debug.Assert(!myReader.Read());
                User user = new User();
                //TODO VERIFY USERNAME AND PASSWORD


            }
            catch (Exception e)
            {
                Exception myEx = new Exception("Could not load user. Error: " + myEx.Message, myEx);
                throw (myEx);
            }
        }

        public User LoadUser(int id)
        {
            string query = "SELECT * FROM user WHERE User.Id="+id+" INNER JOIN person ON person.id=user.person_id";
            MySqlCommand loadUser = new MySqlCommand(query, this._sqlConnection);

            try
            {
                loadUser.ExecuteReader();
                User user = new User(id);
                user.Username = user_name;
                user.Name = name;
                user.Title = title;
                user.PassportNumber = passport_number;
                user.PlaceOfBirth = place_of_birth;
                //TODO SET HasVoted
            }
            catch (Exception e)
            {
                Exception myEx = new Exception("Could not load user. Error: " + myEx.Message, myEx);
                throw (myEx);
            }
        }

        public VoterCard LoadVoterCard(int id)
        {
            string query = "SELECT * FROM voter_card WHERE id="+id;
            MySqlCommand loadUser = new MySqlCommand(query, this._sqlConnection);

            try
            {
                loadUser.ExecuteNonQuery();
                VoterCard voterCard = new VoterCard(ElectionEvent, );
                //TODO WRITE CLASS TO DEFINE ELECTIONEVENT
            }
            catch (Exception e)
            {
                Exception myEx = new Exception("Could not load user. Error: " + myEx.Message, myEx);
                throw (myEx);
            }
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

        public bool MarkUserInvalid(User user)
        {
            throw new NotImplementedException();
        }

        public bool MarkVoterCardInvalid(VoterCard voterCard)
        {
            throw new NotImplementedException();
        }*/

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

        public List<Citizen> FindElegibleVoters()
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

        public bool SetHasVoted(Citizen citizen, int keyPhrase)
        {
            throw new NotImplementedException();
        }

        public bool SetHasVoted(Citizen citizen)
        {
            throw new NotImplementedException();
        }

        public bool ChangePassword(User user, string newPassword)
        {
            throw new NotImplementedException();
        }

        public bool MarkUserInvalid(User user)
        {
            throw new NotImplementedException();
        }

        public bool RestoreUser(User user)
        {
            throw new NotImplementedException();
        }

        public bool MarkVoterCardInvalid(VoterCard voterCard)
        {
            throw new NotImplementedException();
        }

        private void DoIfNotDbNull(MySqlDataReader rdr, string label, Action<string> action)
        {
            if (!rdr.IsDBNull(rdr.GetOrdinal(label)))
            {
                action.Invoke(label);
            }
        }
    }
}
