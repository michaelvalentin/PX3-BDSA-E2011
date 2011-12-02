using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

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
                throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
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
                throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
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
                throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
            }
        }

        public int ValidateUser(string username, string passwordHash)
        {
            Connect();
            MySqlCommand validate = new MySqlCommand("SELECT id FROM user WHERE password_hash=@pwd_hash, username=@uname LIMIT 1", _sqlConnection);
            validate.Prepare();
            validate.Parameters.AddWithValue("@pwd_hash", passwordHash);
            validate.Parameters.AddWithValue("@uname", username);
            if (validate.ExecuteNonQuery() != -1)
            {
                return (int)(validate.ExecuteScalar() ?? 0);
            }
        }

        public HashSet<SystemAction> GetPermissions(User u)
        {
            MySqlCommand getPermissions = new MySqlCommand("SELECT label FROM user u INNER JOIN permission p ON u.id = p.user_id INNER JOIN action a ON a.id = p.action_id WHERE u.id=" + u.DbId, _sqlConnection);
            MySqlDataReader rdr = getPermissions.ExecuteReader();
            HashSet<SystemAction> output = new HashSet<SystemAction>();
            while (rdr.Read())
            {
                output.Add(SystemActions.getSystemAction(rdr.GetString(0)));
            }
            return output;
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
                throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
            }
        }

        public List<User> Find(User u)
        {
            Connect();
            List<User> users = new List<User>();
            string query = "SELECT * FROM user INNER JOIN person ON person_id = person.id AND (title='" + u.Title + "' AND user_name='" + u.Username + "') OR COALESCE(title='" + u.Title + "' AND user_name='" + u.Username + "') IS NOT NULL";
            MySqlCommand find = new MySqlCommand(query, this._sqlConnection);
            //TODO USE PREPARED STATEMENT
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
                throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
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
                    Citizen citizen = new Citizen(reader.GetInt32("id"), reader.GetInt32("cpr"));
                    citizen.EligibleToVote = reader.GetBoolean("eligible_to_vote");
                    DoIfNotDbNull(reader, "name", lbl => citizen.Name = reader.GetString(lbl));
                    DoIfNotDbNull(reader, "address", lbl => citizen.Address = reader.GetString(lbl));
                    DoIfNotDbNull(reader, "place_of_birth", lbl => citizen.PlaceOfBirth = reader.GetString(lbl));
                    DoIfNotDbNull(reader, "passport_number", lbl => citizen.PassportNumber = reader.GetInt32(lbl));
                    //TODO SET HASVOTED
                    citizens.Add(citizen);
                }

                return citizens;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
            }
        }

        public bool Save(Person per)
        {
            Connect();
            int id = per.DbId;
            if (per.DbId != 0)
            {
                try
                {
                    MySqlCommand getPerson = new MySqlCommand(
                        "SELECT id FROM person WHERE id=" + per.DbId, _sqlConnection);
                    if (id != (int)getPerson.ExecuteScalar())
                    {
                        throw new DataAccessException(
                            "Invalid id. The person you are trying to update has an ID that does not exist in the database.");
                    }
                }
                catch (Exception ex)
                {
                    throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
                }
            }

            MySqlCommand savePerson;

            if (id == 0)
            {
                savePerson = new MySqlCommand("INSERT INTO person " +
                    "(name,address,cpr,place_of_birth,passport_number) VALUES(@name,@address,@cpr,@place_of_birth,@passport_number)", _sqlConnection);
            }
            else
            {
                savePerson = new MySqlCommand("UPDATE person SET name=@name, address=@address, place_of_birth=@place_of_birth, passport_number=@passport_number WHERE id=@id LIMIT 1", _sqlConnection);
            }
            savePerson.Prepare();
            savePerson.Parameters.AddWithValue("@name", per.Name ?? "");
            savePerson.Parameters.AddWithValue("@address", per.Address ?? "");
            savePerson.Parameters.AddWithValue("@cpr", per.Cpr);
            savePerson.Parameters.AddWithValue("@place_of_birth", per.PlaceOfBirth ?? "");
            savePerson.Parameters.AddWithValue("@passport_number", per.PassportNumber);
            if (id != 0) savePerson.Parameters.AddWithValue("@id", per.DbId);
            return savePerson.ExecuteNonQuery() == 1;
        }

        public bool Save(User u)
        {//TODO CREATE IT!
            Connect();
            int id = u.DbId;
            if (u.DbId != 0)
            {
                try
                {
                    MySqlCommand getUser = new MySqlCommand(
                        "SELECT id FROM user WHERE id=" + u.DbId, _sqlConnection);
                    if (id != (int)getUser.ExecuteScalar())
                    {
                        throw new DataAccessException(
                            "Invalid id. The person you are trying to update has an ID that does not exist in the database.");
                    }
                }
                catch (Exception ex)
                {
                    throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
                }
            }

            MySqlCommand saveUser;

            if (id == 0)
            {
                saveUser = new MySqlCommand("INSERT INTO user " +
                    "(user_name, title, person_id) VALUES(@user_name, @title, )", _sqlConnection);
            }
            else
            {
                saveUser = new MySqlCommand("UPDATE person SET name=@name, address=@address, place_of_birth=@place_of_birth, passport_number=@passport_number WHERE id=@id LIMIT 1", _sqlConnection);
            }
            saveUser.Prepare();
            saveUser.Parameters.AddWithValue("@name", u.Name ?? "");
            saveUser.Parameters.AddWithValue("@address", u.Address ?? "");
            saveUser.Parameters.AddWithValue("@cpr", u.Cpr);
            saveUser.Parameters.AddWithValue("@place_of_birth", u.PlaceOfBirth ?? "");
            saveUser.Parameters.AddWithValue("@passport_number", u.PassportNumber);
            if (id != 0) saveUser.Parameters.AddWithValue("@id", u.DbId);
            return saveUser.ExecuteNonQuery() == 1;
        }

        public bool Save(VoterCard voterCard)
        {
            throw new NotImplementedException();
        }

        public bool SetHasVoted(Citizen citizen, int keyPhrase)
        {
            Connect();
            try
            {
                MySqlCommand getCpr = new MySqlCommand(
                    "SELECT cpr FROM person WHERE id='" + citizen.DbId + "'", _sqlConnection);
                int citizenKeyPhrase = Convert.ToInt32(getCpr.ToString().Substring(7, 4));
                if (keyPhrase == citizenKeyPhrase)
                {
                    MySqlCommand setHasVoted = new MySqlCommand("SELECT person SET has_voted = '1' WHERE id='" + citizen.DbId + "'", _sqlConnection);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
            }
        }

        public bool SetHasVoted(Citizen citizen)
        {
            Connect();
            try
            {
                MySqlCommand setHasVoted = new MySqlCommand("SELECT person SET has_voted = '1' WHERE id='" + citizen.DbId + "'", _sqlConnection);
                if (setHasVoted.ExecuteNonQuery() == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
            }
        }

        public bool ChangePassword(User user, string newPassword)
        {
            throw new NotImplementedException();
        }

        public bool ChangePassword(User user, string newPassword, String oldPassword)
        {

        }

        public bool MarkUserInvalid(User user)
        {
            throw new NotImplementedException();
        }

        public bool RestoreUser(User user)
        {
            throw new NotImplementedException();
        }

        public bool MarkVoterCardInvalid(VoterCard vc)
        {
            Connect();
            try
            {
                MySqlCommand setInvalid = new MySqlCommand("SELECT voter_card SET valid = '0' WHERE id='" + vc.Id + "'", _sqlConnection);
                if (setInvalid.ExecuteNonQuery() == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
            }
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
