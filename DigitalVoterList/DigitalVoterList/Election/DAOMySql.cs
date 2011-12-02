using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Diagnostics.Contracts;

namespace DigitalVoterList.Election
{
    using System.Diagnostics;
    using System.Windows.Documents;

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
        
        public User LoadUser(string username, string pass)
        {
            Connect();
            string query = "SELECT * FROM user INNER JOIN person ON person_id=person.id AND user_name='" + username + "'";
            MySqlCommand loadUser = new MySqlCommand(query, this._sqlConnection);

            try
            {
                MySqlDataReader reader = loadUser.ExecuteReader();
                if (!reader.Read()) throw new DataAccessException("No person with the username and password specified.");
                Debug.Assert(reader.GetString("password_hash") == pass);
                User user = new User(reader.GetInt32("id"));
                user.Username = reader.GetString("user_name");
                user.Title = reader.GetString("title");
                user.PassportNumber = reader.GetInt32("passport_number");
                user.Name = reader.GetString("name");
                user.PlaceOfBirth = reader.GetString("place_of_birth");

                return user;
                //TODO VERIFY USERNAME AND PASSWORD
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public User LoadUser(int id)
        {
            Connect();
            string query = "SELECT * FROM user INNER JOIN person ON person_id=person.id AND user.id='" + id + "'";
            MySqlCommand loadUser = new MySqlCommand(query, this._sqlConnection);

            try
            {
                MySqlDataReader reader = loadUser.ExecuteReader();
                if (!reader.Read()) throw new DataAccessException("No person with the id specified.");
                User user = new User(reader.GetInt32("id"));
                user.Username = reader.GetString("user_name");
                user.Title = reader.GetString("title");
                user.PassportNumber = reader.GetInt32("passport_number");
                user.Name = reader.GetString("name");
                user.PlaceOfBirth = reader.GetString("place_of_birth");

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /*
        public VoterCard LoadVoterCard(int id)
        {
            Connect();
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
        */
        
        public List<Person> Find(Person p)
        {
            Connect();
            List<Person> persons = new List<Person>();
            string query = "SELECT * FROM person WHERE cpr='" + p.Cpr + "' OR (name='" + p.Name + "' AND address='" + p.Address + "') OR COALESCE(name='" + p.Name + "', address='" + p.Address + "') IS NOT NULL";
            MySqlCommand find = new MySqlCommand(query, this._sqlConnection);

            try
            {
                MySqlDataReader reader = find.ExecuteReader();
                while (reader.Read())
                {
                    Person pers = new Person();
                    pers.Cpr = reader.GetInt32("cpr");
                    DoIfNotDbNull(reader, "name", lbl => pers.Name = reader.GetString(lbl));
                    DoIfNotDbNull(reader, "address", lbl => pers.Address = reader.GetString(lbl));
                    DoIfNotDbNull(reader, "place_of_birth", lbl => pers.PlaceOfBirth = reader.GetString(lbl));
                    DoIfNotDbNull(reader, "passport_number", lbl => pers.PassportNumber = reader.GetInt32(lbl));
                    persons.Add(pers);
                }
                
                return persons;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public List<User> Find(User u)
        {
            Connect();
            List<User> users = new List<User>();
            string query = "SELECT * FROM user INNER JOIN person ON person_id = person.id AND (title='" + u.Title + "' AND user_name='" + u.Username + "') OR COALESCE(title='" + u.Title + "' AND user_name='" + u.Username + "') IS NOT NULL";
            MySqlCommand find = new MySqlCommand(query, this._sqlConnection);
            //TODO MAKE THE QUERY BETTER!!
            try
            {
                MySqlDataReader reader = find.ExecuteReader();
                while (reader.Read())
                {
                    User user = new User();
                    user.Username = reader.GetString("user_name");
                    user.Title = reader.GetString("title");
                    user.Cpr = reader.GetInt32("cpr");
                    DoIfNotDbNull(reader, "name", lbl => user.Name = reader.GetString(lbl));
                    DoIfNotDbNull(reader, "address", lbl => user.Address = reader.GetString(lbl));
                    DoIfNotDbNull(reader, "place_of_birth", lbl => user.PlaceOfBirth = reader.GetString(lbl));
                    DoIfNotDbNull(reader, "passport_number", lbl => user.PassportNumber = reader.GetInt32(lbl));
                    users.Add(user);
                }
                
                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<VoterCard> Find(VoterCard voterCard)
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

        public List<Citizen> FindElegibleVoters()
        {
            Connect();
            List<Citizen> citizens = new List<Citizen>();
            string query = "SELECT * FROM person INNER JOIN quiz ON person.id=person_id AND eligible_to_vote = '1'";
            MySqlCommand findEligibleVoters = new MySqlCommand(query, this._sqlConnection);

            try
            {
                MySqlDataReader reader = findEligibleVoters.ExecuteReader();
                while (reader.Read())
                {
                    Citizen citizen = new Citizen(reader.GetInt32("id"),reader.GetInt32("cpr"));
                    citizen.EligibleToVote = reader.GetBoolean("eligible_to_vote");
                    //DoIfNotDbNull(reader, "name", lbl => citizen.Name = reader.GetString(lbl));
                    //DoIfNotDbNull(reader, "address", lbl => citizen.Address = reader.GetString(lbl));
                    //DoIfNotDbNull(reader, "place_of_birth", lbl => citizen.PlaceOfBirth = reader.GetString(lbl));
                    //DoIfNotDbNull(reader, "passport_number", lbl => citizen.PassportNumber = reader.GetInt32(lbl));
                    //TODO SET HASVOTED
                    citizens.Add(citizen);
                }

                return citizens;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
