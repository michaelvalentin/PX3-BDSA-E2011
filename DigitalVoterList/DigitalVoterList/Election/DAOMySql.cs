using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace DigitalVoterList.Election
{
    using System.Diagnostics.Contracts;
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
            MySqlDataReader reader = null;
            try
            {
                reader = loadPerson.ExecuteReader();
                if (!reader.Read()) return null;
                if (reader.GetString("cpr").Equals(null))
                {
                    reader.Read();
                    Person person = new Person(id);
                    DoIfNotDbNull(reader, "name", lbl => person.Name = reader.GetString(lbl));
                    DoIfNotDbNull(reader, "cpr", lbl => person.Cpr = reader.GetString(lbl));
                    DoIfNotDbNull(reader, "address", lbl => person.Address = reader.GetString(lbl));
                    DoIfNotDbNull(reader, "place_of_birth", lbl => person.PlaceOfBirth = reader.GetString(lbl));
                    DoIfNotDbNull(reader, "passport_number", lbl => person.PassportNumber = reader.GetInt32(lbl));
                    return person;
                }
                else
                {
                    Citizen citizen = new Citizen(id, reader.GetString("cpr"));
                    DoIfNotDbNull(reader, "name", lbl => citizen.Name = reader.GetString(lbl));
                    DoIfNotDbNull(reader, "address", lbl => citizen.Address = reader.GetString(lbl));
                    DoIfNotDbNull(reader, "place_of_birth", lbl => citizen.PlaceOfBirth = reader.GetString(lbl));
                    DoIfNotDbNull(reader, "passport_number", lbl => citizen.PassportNumber = reader.GetInt32(lbl));
                    citizen.EligibleToVote = reader.GetBoolean("eligible_to_vote");
                    return citizen;
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        public User LoadUser(string username)
        {
            Connect();
            MySqlCommand loadUser = new MySqlCommand(
                "SELECT * FROM user INNER JOIN person ON person_id=person.id AND user_name=@uname",
                 this._sqlConnection
                 );
            loadUser.Prepare();
            loadUser.Parameters.AddWithValue("@uname", username);
            return LoadUser(loadUser);
        }

        public User LoadUser(string username, string password)
        {
            User u = LoadUser(username);
            if (u != null && u.FetchPermissions(username, password))
            {
                return u;
            }
            else
            {
                return null;
            }
        }

        public User LoadUser(int id)
        {
            Connect();
            string query = "SELECT * FROM user u LEFT JOIN person p ON u.person_id=p.id WHERE u.id=" + id;
            return LoadUser(new MySqlCommand(query, this._sqlConnection));
        }

        private User LoadUser(MySqlCommand loadUser)
        {
            Connect();
            MySqlDataReader reader = null;
            try
            {
                reader = loadUser.ExecuteReader();
                if (!reader.Read()) return null;
                User user = new User(reader.GetInt32("id"));
                user.Username = reader.GetString("user_name");
                user.Title = reader.GetString("title");
                user.PassportNumber = reader.GetInt32("passport_number");
                user.Name = reader.GetString("name");
                user.PlaceOfBirth = reader.GetString("place_of_birth");
                user.UserSalt = reader.GetString("user_salt");

                return user;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        public bool ValidateUser(string username, string passwordHash)
        {
            Connect();
            MySqlCommand validate = new MySqlCommand("SELECT id FROM user WHERE password_hash=@pwd_hash AND user_name=@uname LIMIT 1", _sqlConnection);
            validate.Prepare();
            validate.Parameters.AddWithValue("@pwd_hash", passwordHash);
            validate.Parameters.AddWithValue("@uname", username);

            object result = validate.ExecuteScalar();
            return result != null && (int)result > 0;
        }

        public HashSet<SystemAction> GetPermissions(User u)
        {
            MySqlCommand getPermissions = new MySqlCommand("SELECT label FROM user u INNER JOIN permission p ON u.id = p.user_id INNER JOIN action a ON a.id = p.action_id WHERE u.id=" + u.DbId, _sqlConnection);
            MySqlDataReader rdr = null;
            HashSet<SystemAction> output = new HashSet<SystemAction>();

            try
            {
                rdr = getPermissions.ExecuteReader();
                while (rdr.Read())
                {
                    output.Add(SystemActions.getSystemAction(rdr.GetString(0)));
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
            }
            finally
            {
                if (rdr != null) rdr.Close();
            }
            return output;
        }

        public HashSet<VotingVenue> GetWorkplaces(User u)
        {
            MySqlCommand getWorkplaces = new MySqlCommand("SELECT * FROM user u INNER JOIN workplace w ON u.id = w.user_id INNER JOIN voting_venue v ON v.id = w.voting_venue_id WHERE u.id=" + u.DbId, _sqlConnection);
            HashSet<VotingVenue> output = new HashSet<VotingVenue>();
            MySqlDataReader rdr = null;
            try
            {
                rdr = getWorkplaces.ExecuteReader();
                while (rdr.Read())
                {
                    output.Add(new VotingVenue(rdr.GetInt32("id"), rdr.GetString("name"), rdr.GetString("address")));
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
            }
            finally
            {
                if (rdr != null) rdr.Close();
            }
            return output;
        }

        public VoterCard LoadVoterCard(int id)
        {
            Connect();
            string query = "SELECT * FROM voter_card INNER JOIN person ON person.id=person_id AND voter_card.id=" + id;
            MySqlCommand loadVoterCard = new MySqlCommand(query, this._sqlConnection);
            MySqlDataReader reader = null;

            try
            {
                reader = loadVoterCard.ExecuteReader();
                reader.Read();
                int personId = reader.GetInt32("person_id");
                reader.Close();
                Citizen citizen = (Citizen)this.LoadPerson(personId);

                reader = loadVoterCard.ExecuteReader();
                if (!reader.Read()) return null;
                VoterCard voterCard = new VoterCard(Settings.Election, citizen);
                voterCard.Id = id;
                return voterCard;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        public VoterCard LoadVoterCard(string idKey)
        {
            Connect();
            MySqlCommand loadVoterCardId = new MySqlCommand("SELECT id FROM voter_card WHERE id_key=@id_key", _sqlConnection);
            loadVoterCardId.Prepare();
            loadVoterCardId.Parameters.AddWithValue("id_key", idKey);
            int id = (int)(loadVoterCardId.ExecuteScalar() ?? 0);
            if (id == 0) return null;
            return LoadVoterCard(id);
        }


        public List<Person> Find(Person p)
        {
            Contract.Ensures(Contract.Result<List<Person>>() != null);


            Connect();
            List<Person> persons = new List<Person>();
            string query = "SELECT * FROM person WHERE cpr='" + p.Cpr + "' OR (name=@name AND address=@address) OR COALESCE(name=@name, address=@address) IS NOT NULL";
            MySqlCommand find = new MySqlCommand(query, this._sqlConnection);
            MySqlDataReader reader = null;
            try
            {
                reader = find.ExecuteReader();
                while (reader.Read())
                {
                    Person pers = new Person();
                    pers.Cpr = reader.GetString("cpr");
                    DoIfNotDbNull(reader, "name", lbl => pers.Name = reader.GetString(lbl));
                    DoIfNotDbNull(reader, "address", lbl => pers.Address = reader.GetString(lbl));
                    DoIfNotDbNull(reader, "place_of_birth", lbl => pers.PlaceOfBirth = reader.GetString(lbl));
                    DoIfNotDbNull(reader, "passport_number", lbl => pers.PassportNumber = reader.GetInt32(lbl));
                    persons.Add(pers);
                }
                if (persons.ToArray().Length == 0) return null;
                return persons;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        public List<User> Find(User u)
        {
            Connect();
            List<User> users = new List<User>();
            string query = "SELECT * FROM user INNER JOIN person ON person_id = person.id WHERE (title='" + u.Title + "' AND user_name='" + u.Username + "') OR COALESCE(title='" + u.Title + "', user_name='" + u.Username + "')";
            MySqlCommand find = new MySqlCommand(query, this._sqlConnection);
            MySqlDataReader reader = null;

            try
            {
                reader = find.ExecuteReader();
                while (reader.Read())
                {
                    User user = new User();
                    user.Username = reader.GetString("user_name");
                    user.Title = reader.GetString("title");
                    user.Cpr = reader.GetString("cpr");
                    DoIfNotDbNull(reader, "name", lbl => user.Name = reader.GetString(lbl));
                    DoIfNotDbNull(reader, "address", lbl => user.Address = reader.GetString(lbl));
                    DoIfNotDbNull(reader, "place_of_birth", lbl => user.PlaceOfBirth = reader.GetString(lbl));
                    DoIfNotDbNull(reader, "passport_number", lbl => user.PassportNumber = reader.GetInt32(lbl));
                    users.Add(user);
                }
                if (users.ToArray().Length == 0) return null;
                return users;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        public List<VoterCard> Find(VoterCard voterCard)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<RawPerson> LoadRawPersonData()
        {
            Connect();
            string query = "SELECT * FROM raw_person_data";
            MySqlCommand loadRawPeople = new MySqlCommand(query, this._sqlConnection);

            try
            {
                MySqlDataReader reader = loadRawPeople.ExecuteReader();
                return readStuff(reader);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
            }
        }

        // HAD TO MAKE A PRIVATE METHOD FOR TRY CATCH OF YIELD
        private IEnumerator<RawPerson> readStuff(MySqlDataReader reader)
        {
            while (reader.Read())
            {
                var rawPerson = new RawPerson();
                DoIfNotDbNull(reader, "name", lbl => rawPerson.Name = reader.GetString(lbl));
                yield return rawPerson;
            }
        }

        public List<Citizen> FindElegibleVoters()
        {
            Connect();
            List<Citizen> citizens = new List<Citizen>();
            string query = "SELECT * FROM person WHERE eligible_to_vote = '1'";
            MySqlCommand findEligibleVoters = new MySqlCommand(query, this._sqlConnection);
            MySqlDataReader reader = null;

            try
            {
                reader = findEligibleVoters.ExecuteReader();
                while (reader.Read())
                {
                    Citizen citizen = new Citizen(reader.GetInt32("id"), reader.GetString("cpr"));
                    citizen.EligibleToVote = reader.GetBoolean("eligible_to_vote");
                    DoIfNotDbNull(reader, "name", lbl => citizen.Name = reader.GetString(lbl));
                    DoIfNotDbNull(reader, "address", lbl => citizen.Address = reader.GetString(lbl));
                    DoIfNotDbNull(reader, "place_of_birth", lbl => citizen.PlaceOfBirth = reader.GetString(lbl));
                    DoIfNotDbNull(reader, "passport_number", lbl => citizen.PassportNumber = reader.GetInt32(lbl));

                    citizens.Add(citizen);
                }
                if (citizens.ToArray().Length == 0) return null;
                return citizens;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        public IEnumerable<RawPerson> LoadRawPeople()
        {
            throw new NotImplementedException();
        }

        public bool Save(Person per)
        {
            Connect();
            int id = per.DbId;

            if (id != 0)
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
        {
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
                            "Invalid id. The user you are trying to update has an ID that does not exist in the database.");
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
                    "(user_name, title, password) VALUES(@user_name, @title, @password)", _sqlConnection);
            }
            else
            {
                saveUser = new MySqlCommand("UPDATE user SET user_name=@user_name, title=@title, password=@password WHERE id=@id LIMIT 1", _sqlConnection);
            }
            saveUser.Prepare();
            saveUser.Parameters.AddWithValue("@user_name", u.Username);
            saveUser.Parameters.AddWithValue("@title", u.Title);
            saveUser.Parameters.AddWithValue("@password", u.ChangePassword("@password"));
            if (id != 0) saveUser.Parameters.AddWithValue("@id", u.DbId);
            return saveUser.ExecuteNonQuery() == 1;
        }

        public bool Save(VoterCard vc)
        {
            Connect();
            int id = vc.Id;
            if (vc.Id != 0)
            {
                try
                {
                    MySqlCommand getVoterCard = new MySqlCommand(
                        "SELECT id FROM voter_card WHERE id=" + vc.Id, _sqlConnection);
                    if (id != (int)getVoterCard.ExecuteScalar())
                    {
                        throw new DataAccessException(
                            "Invalid id. The votercard you are trying to update has an ID that does not exist in the database.");
                    }
                }
                catch (Exception ex)
                {
                    throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
                }
            }

            MySqlCommand saveVoterCard;

            if (id == 0)
            {
                saveVoterCard = new MySqlCommand("INSERT INTO voter_card " +
                    "(person_id, valid) VALUES(@person_id, @valid, @id_key)", _sqlConnection);
            }
            else
            {
                saveVoterCard = new MySqlCommand("UPDATE voter_card SET person_id=@person_id, valid=@valid, id_key=@id_key WHERE id=@id LIMIT 1", _sqlConnection);
            }
            saveVoterCard.Prepare();
            saveVoterCard.Parameters.AddWithValue("@person_id", vc.Citizen.DbId);
            saveVoterCard.Parameters.AddWithValue("@valid", vc.Valid);
            saveVoterCard.Parameters.AddWithValue("@id_key", vc.IdKey);
            if (id != 0) saveVoterCard.Parameters.AddWithValue("@id", vc.Id);
            return saveVoterCard.ExecuteNonQuery() == 1;
        }

        public bool SetHasVoted(Citizen citizen, int cprKey)
        {
            Connect();
            try
            {
                MySqlCommand getCpr = new MySqlCommand(
                    "SELECT cpr FROM person WHERE id='" + citizen.DbId + "'", _sqlConnection);
                int citizenKeyPhrase = Convert.ToInt32(getCpr.ToString().Substring(7, 4));
                if (cprKey == citizenKeyPhrase)
                {
                    try
                    {
                        MySqlCommand setHasVoted = new MySqlCommand("SELECT person SET has_voted = '1' WHERE id='" + citizen.DbId + "'", _sqlConnection);
                        citizen.SetHasVoted();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
                    }
                }
                return false;
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
            Connect();

            try
            {
                User u = LoadUser(user.DBId);
                return u.ChangePassword(newPassword);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
            }

        }

        public bool ChangePassword(User user, string newPassword, string oldPassword)
        {
            Connect();
            try
            {
                User u = LoadUser(user.DBId);
                return u.ChangePassword(oldPassword, newPassword);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
            }
        }

        public bool MarkUserInvalid(User user)
        {
            DAOMySql dms = new DAOMySql();
            if (dms.MarkUserInvalid(user))
            {
                User u = LoadUser(user.Username);
                u.Valid = false;
                return true;
            }
            return false;
        }

        public bool RestoreUser(User user)
        {
            DAOMySql dms = new DAOMySql();
            if (dms.RestoreUser(user))
            {
                try
                {
                    User u = LoadUser(user.Username);
                    u.Valid = true;
                    return true;
                }
                catch (Exception ex)
                {
                    throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
                }
            }
            return false;
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
                return false;
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
