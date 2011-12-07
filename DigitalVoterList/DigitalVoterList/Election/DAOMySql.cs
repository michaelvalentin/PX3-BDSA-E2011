using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace DigitalVoterList.Election
{
    using System.Diagnostics.Contracts;

    using DigitalVoterList.Utilities;

    public class DAOMySql : IDataAccessObject
    {
        private int _readers = 0;

        private Stack<MySqlConnection> _freeConnections = new Stack<MySqlConnection>();
        private HashSet<MySqlConnection> _workingConnections = new HashSet<MySqlConnection>();
        private string _connectionString;

        private MySqlConnection con;

        private DAOMySql()
        {
            this._connectionString = "Server=" + Settings.DbHost + ";" +
                                     "Database=" + Settings.DbName + ";" +
                                     "Uid=" + Settings.DbUser + ";" +
                                     "Pwd=" + Settings.DbPassword + ";";
        }

        public static IDataAccessObject GetDao(User u)
        {
            return new DAOPermissionProxy(u, new DAOMySql());
        }

        private MySqlConnection GetSqlConnection()
        {
            Contract.Ensures(Contract.Result<MySqlConnection>() != null);

            MySqlConnection workingConnection = null;

            if (_freeConnections.Count != 0)
            {
                workingConnection = _freeConnections.Pop();
                this._workingConnections.Add(workingConnection);
            }
            else
            {
                workingConnection = this.NewSqlConnection();
                workingConnection.Open();
                this._workingConnections.Add(workingConnection);
            }

            return workingConnection;
        }

        private void ReleaseSqlConnection(MySqlConnection connection)
        {
            if (_freeConnections.Count != 0)
            {
                connection.Close();
                this._workingConnections.Remove(connection);
            }
            else
            {
                this._freeConnections.Push(connection);
                this._workingConnections.Remove(connection);
            }
        }

        private MySqlConnection NewSqlConnection()
        {
            try
            {
                return new MySqlConnection(this._connectionString);
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Error connecting you to " +
                    "the my sql server. Internal error message: " + excp.Message, excp);
                throw myExcp;
            }
        }

        ~DAOMySql()
        {
            Disconnect();
        }

        private void Disconnect()
        {
            foreach (var mySqlConnection in _freeConnections)
            {
                mySqlConnection.Close();
            }

            foreach (var mySqlConnection in _workingConnections)
            {
                mySqlConnection.Close();
            }
        }

        public Person LoadPerson(int id)
        {
            var connection = this.GetSqlConnection();
            string query = "SELECT * FROM person WHERE id=" + id + " LIMIT 1";
            MySqlCommand loadPerson = new MySqlCommand(query, connection);
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
                    DoIfNotDbNull(reader, "passport_number", lbl => person.PassportNumber = reader.GetString(lbl));
                    return person;
                }
                else
                {
                    Citizen citizen = new Citizen(id, reader.GetString("cpr"));
                    DoIfNotDbNull(reader, "name", lbl => citizen.Name = reader.GetString(lbl));
                    DoIfNotDbNull(reader, "address", lbl => citizen.Address = reader.GetString(lbl));
                    DoIfNotDbNull(reader, "place_of_birth", lbl => citizen.PlaceOfBirth = reader.GetString(lbl));
                    DoIfNotDbNull(reader, "passport_number", lbl => citizen.PassportNumber = reader.GetString(lbl));
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
                this.ReleaseSqlConnection(connection);
            }
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

        public User LoadUser(string username)
        {
            var connection = this.GetSqlConnection();
            var loadUser = new MySqlCommand(
                "SELECT * FROM user INNER JOIN person ON person_id=person.id AND user_name=@uname", connection);
            loadUser.Prepare();
            loadUser.Parameters.AddWithValue("@uname", username);
            return LoadUser(loadUser);
        }

        public User LoadUser(int id)
        {
            string query = "SELECT * FROM user u LEFT JOIN person p ON u.person_id=p.id WHERE u.id=" + id;
            var connection = this.GetSqlConnection();
            return LoadUser(new MySqlCommand(query, connection));
        }

        private User LoadUser(MySqlCommand loadUser)
        {
            Contract.Requires(loadUser != null & loadUser.Connection != null);

            MySqlDataReader reader = null;
            try
            {
                reader = loadUser.ExecuteReader();
                if (!reader.Read()) return null;
                User user = new User(reader.GetInt32("id"));
                user.Username = reader.GetString("user_name");
                user.Title = reader.GetString("title");
                user.PassportNumber = reader.GetString("passport_number");
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
                this.ReleaseSqlConnection(loadUser.Connection);
            }
        }

        public bool ValidateUser(string username, string passwordHash)
        {
            var connection = this.GetSqlConnection();
            MySqlCommand validate = new MySqlCommand("SELECT id FROM user WHERE password_hash=@pwd_hash AND user_name=@uname LIMIT 1", connection);
            validate.Prepare();
            validate.Parameters.AddWithValue("@pwd_hash", passwordHash);
            validate.Parameters.AddWithValue("@uname", username);

            object result = validate.ExecuteScalar();

            this.ReleaseSqlConnection(connection);
            return result != null && (int)result > 0;
        }

        public HashSet<SystemAction> GetPermissions(User u)
        {
            var connection = this.GetSqlConnection();
            MySqlCommand getPermissions = new MySqlCommand("SELECT label FROM user u INNER JOIN permission p ON u.id = p.user_id INNER JOIN action a ON a.id = p.action_id WHERE u.id=" + u.DbId, connection);
            MySqlDataReader reader = null;
            HashSet<SystemAction> output = new HashSet<SystemAction>();

            try
            {
                reader = getPermissions.ExecuteReader();
                while (reader.Read())
                {
                    output.Add(SystemActions.getSystemAction(reader.GetString(0)));
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
                this.ReleaseSqlConnection(connection);
            }
            return output;
        }

        public HashSet<VotingVenue> GetWorkplaces(User u)
        {
            var connection = this.GetSqlConnection();
            MySqlCommand getWorkplaces = new MySqlCommand("SELECT * FROM user u INNER JOIN workplace w ON u.id = w.user_id INNER JOIN voting_venue v ON v.id = w.voting_venue_id WHERE u.id=" + u.DbId, connection);
            HashSet<VotingVenue> output = new HashSet<VotingVenue>();
            MySqlDataReader reader = null;
            try
            {
                reader = getWorkplaces.ExecuteReader();
                while (reader.Read())
                {
                    output.Add(new VotingVenue(reader.GetInt32("id"), reader.GetString("name"), reader.GetString("address")));
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
                this.ReleaseSqlConnection(connection);
            }
            return output;
        }

        public VoterCard LoadVoterCard(int id)
        {
            var connection = this.GetSqlConnection();
            string query = "SELECT * FROM voter_card INNER JOIN person ON person.id=person_id AND voter_card.id=" + id;
            MySqlCommand loadVoterCard = new MySqlCommand(query, connection);
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
                this.ReleaseSqlConnection(connection);
            }
        }

        public VoterCard LoadVoterCard(string idKey)
        {
            var connection = this.GetSqlConnection();
            MySqlCommand loadVoterCardId = new MySqlCommand("SELECT id FROM voter_card WHERE id_key=@id_key", connection);
            loadVoterCardId.Prepare();
            loadVoterCardId.Parameters.AddWithValue("id_key", idKey);
            int id = (int)(loadVoterCardId.ExecuteScalar() ?? 0);
            if (id == 0) return null;
            return LoadVoterCard(id);
        }

        private MySqlCommand FindByValues(string tableName, Dictionary<string, string> info)
        {
            var connection = this.GetSqlConnection();
            var query = "SELECT * FROM person WHERE id=@1 AND name='Hans Hansen'";
            var findData = new MySqlCommand(query, connection);
            findData.Prepare();
            findData.Parameters.AddWithValue("@1", Person.Name);
        }



        public List<Person> Find(Person p)
        {
            Contract.Requires(p != null);
            Contract.Ensures(Contract.Result<List<Person>>() != null);

            var tableName = "person";

            var information = new Dictionary<string, string>
                {
                    { p.DbId.ToString(), "id" },
                    { p.Name, "name" },
                    { p.Cpr, "cpr" },
                    { p.Address, "address" },
                    { p.PassportNumber, "passport_number" },
                    { p.PlaceOfBirth, "place_of_birth" }
                };

            MySqlCommand find = this.FindByValues(tableName, information);
            MySqlDataReader reader = null;

            var persons = new List<Person>();

            try
            {
                reader = find.ExecuteReader();
                while (reader.Read())
                {
                    Person pers = new Person(reader.GetInt32("id"));
                    pers.Cpr = reader.GetString("cpr");
                    DoIfNotDbNull(reader, "name", lbl => pers.Name = reader.GetString(lbl));
                    DoIfNotDbNull(reader, "address", lbl => pers.Address = reader.GetString(lbl));
                    DoIfNotDbNull(reader, "place_of_birth", lbl => pers.PlaceOfBirth = reader.GetString(lbl));
                    DoIfNotDbNull(reader, "passport_number", lbl => pers.PassportNumber = reader.GetString(lbl));
                    persons.Add(pers);
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
                this.ReleaseSqlConnection(find.Connection);
            }
            return persons;
        }

        public List<User> Find(User u)
        {
            var connection = GetSqlConnection();
            List<User> users = new List<User>();
            string query = "SELECT * FROM user INNER JOIN person ON person_id = person.id WHERE (title='" + u.Title + "' AND user_name='" + u.Username + "') OR COALESCE(title='" + u.Title + "', user_name='" + u.Username + "')";
            MySqlCommand find = new MySqlCommand(query, connection);
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
                    DoIfNotDbNull(reader, "passport_number", lbl => user.PassportNumber = reader.GetString(lbl));
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
                this.ReleaseSqlConnection(connection);
            }
        }

        public List<VoterCard> Find(VoterCard voterCard)
        {
            throw new NotImplementedException();
        }

        public List<Citizen> FindElegibleVoters()
        {
            var connection = GetSqlConnection();
            List<Citizen> citizens = new List<Citizen>();
            string query = "SELECT * FROM person WHERE eligible_to_vote = '1'";
            MySqlCommand findEligibleVoters = new MySqlCommand(query, connection);
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
                    DoIfNotDbNull(reader, "passport_number", lbl => citizen.PassportNumber = reader.GetString(lbl));

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
                this.ReleaseSqlConnection(connection);
            }
        }

        public void UpdatePeople(Func<Person, RawPerson, Person> updateFunc)
        {
            var connection = GetSqlConnection();
            string query = "SELECT * FROM raw_person_data";
            MySqlCommand loadRowPeople = new MySqlCommand(query, connection);
            MySqlDataReader reader = null;

            try
            {
                reader = loadRowPeople.ExecuteReader();

                while (reader.Read())
                {
                    RawPerson rawPerson = new RawPerson();
                    DoIfNotDbNull(reader, "name", lbl => rawPerson.Name = reader.GetString(lbl));
                    DoIfNotDbNull(reader, "address", lbl => rawPerson.Address = reader.GetString(lbl));
                    DoIfNotDbNull(reader, "birthplace", lbl => rawPerson.Birthplace = reader.GetString(lbl));
                    DoIfNotDbNull(reader, "passport_number", lbl => rawPerson.PassportNumber = reader.GetString(lbl));

                    //Make new person or find old in database
                    var realPersonList = Find(new Person() { Cpr = rawPerson.CPR });
                    Person person = (realPersonList.Count > 0) ? realPersonList[0] : person = new Person();

                    //Update data with updatefunction
                    person = updateFunc(person, rawPerson);

                    //Save updated data
                    //Save(person);
                }


            }
            catch (Exception ex)
            {
                throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
                this.ReleaseSqlConnection(connection);
            }

            //Update people that are not in the raw data
            this.MarkPeopleNotInRawDataUneligibleToVote();

        }

        public VotingVenue FindVotingVenue(Citizen citizen)
        {
            throw new NotImplementedException();
        }

        public void MarkPeopleNotInRawDataUneligibleToVote()
        {
            var connection = GetSqlConnection();
            try
            {
                new MySqlCommand("DELETE FROM person WHERE p.cpr NOT IN (SELECT r.cpr FROM raw_person_data);", connection).ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
            }
        }

        public void Save(Person per)
        {
            var connection = GetSqlConnection();
            int id = per.DbId;

            if (id != 0)
            {
                try
                {
                    MySqlCommand getPerson = new MySqlCommand(
                        "SELECT id FROM person WHERE id=" + per.DbId, connection);
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
                    "(name,address,cpr,place_of_birth,passport_number) VALUES(@name,@address,@cpr,@place_of_birth,@passport_number)", connection);
            }
            else
            {
                savePerson = new MySqlCommand("UPDATE person SET name=@name, address=@address, place_of_birth=@place_of_birth, passport_number=@passport_number WHERE id=@id LIMIT 1", connection);
            }
            savePerson.Prepare();
            savePerson.Parameters.AddWithValue("@name", per.Name ?? "");
            savePerson.Parameters.AddWithValue("@address", per.Address ?? "");
            savePerson.Parameters.AddWithValue("@cpr", per.Cpr);
            savePerson.Parameters.AddWithValue("@place_of_birth", per.PlaceOfBirth ?? "");
            savePerson.Parameters.AddWithValue("@passport_number", per.PassportNumber);
            if (id != 0) savePerson.Parameters.AddWithValue("@id", per.DbId);
            if (per is Citizen)
                SaveQuizzes((Citizen)per);
        }

        private void SaveQuizzes(Citizen citizen)
        {
            var connection = GetSqlConnection();
            if (citizen.DbId == 0)
            {
                MySqlCommand deleteQuiz = new MySqlCommand(
                    "DELETE FROM quiz WHERE person_id='" + citizen.DbId + "'", connection);

                MySqlDataReader reader = null;

                try
                {
                    reader = deleteQuiz.ExecuteReader();

                    foreach (var quiz in citizen.SecurityQuestions)
                    {
                        MySqlCommand saveQuiz =
                            new MySqlCommand(
                                "INSERT INTO quiz (question, answer, person_id) VALUES(@question, @answer, @person_id)",
connection);
                        saveQuiz.Prepare();
                        saveQuiz.Parameters.AddWithValue("@question", quiz.Question);
                        saveQuiz.Parameters.AddWithValue("@answer", quiz.Answer);
                        saveQuiz.Parameters.AddWithValue("@person_id", citizen.DbId);
                    }
                }
                catch (Exception ex)
                {
                    throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
                }
                finally
                {
                    if (reader != null) reader.Close();
                    this.ReleaseSqlConnection(connection);
                }
            }
        }

        public void Save(User u)
        {
            var connection = GetSqlConnection();
            int id = u.DbId;
            if (u.DbId != 0)
            {
                try
                {
                    MySqlCommand getUser = new MySqlCommand(
                        "SELECT id FROM user WHERE id=" + u.DbId, connection);
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
                    "(user_name, title) VALUES(@user_name, @title)", connection);
            }
            else
            {
                saveUser = new MySqlCommand("UPDATE user SET user_name=@user_name, title=@title, WHERE id=@id LIMIT 1", connection);
            }
            saveUser.Prepare();
            saveUser.Parameters.AddWithValue("@user_name", u.Username);
            saveUser.Parameters.AddWithValue("@title", u.Title);
            if (id != 0) saveUser.Parameters.AddWithValue("@id", u.DbId);
            saveUser.ExecuteNonQuery();
        }

        public void Save(VoterCard vc)
        {
            var connection = GetSqlConnection();
            int id = vc.Id;
            if (vc.Id != 0)
            {
                try
                {
                    MySqlCommand getVoterCard = new MySqlCommand(
                        "SELECT id FROM voter_card WHERE id=" + vc.Id, connection);
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
                    "(person_id, valid) VALUES(@person_id, @valid, @id_key)", connection);
            }
            else
            {
                saveVoterCard = new MySqlCommand("UPDATE voter_card SET person_id=@person_id, valid=@valid, id_key=@id_key WHERE id=@id LIMIT 1", connection);
            }
            saveVoterCard.Prepare();
            saveVoterCard.Parameters.AddWithValue("@person_id", vc.Citizen.DbId);
            saveVoterCard.Parameters.AddWithValue("@valid", vc.Valid);
            saveVoterCard.Parameters.AddWithValue("@id_key", vc.IdKey);
            if (id != 0) saveVoterCard.Parameters.AddWithValue("@id", vc.Id);
            //return saveVoterCard.ExecuteNonQuery() == 1;
        }

        public bool Save(int citizenId, Quiz q)
        {
            var connection = GetSqlConnection();
            MySqlCommand cId = new MySqlCommand("SELECT id FROM person WHERE id='" + citizenId + "' LIMIT 1");
            MySqlDataReader reader = null;
            try
            {
                reader = cId.ExecuteReader();
                if (reader.Read() && reader.GetInt32("id") != 0)
                {
                    MySqlCommand quiz =
                        new MySqlCommand(
                            "INSERT INTO quiz (question, answer, person_id) VALUES(@question, @answer, @person_id)",
                            connection);
                    quiz.Parameters.AddWithValue("@question", q.Question);
                    quiz.Parameters.AddWithValue("@answer", q.Answer);
                    quiz.Parameters.AddWithValue("@person_id", citizenId);
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
            finally
            {
                if (reader != null) reader.Close();
                this.ReleaseSqlConnection(connection);
            }
        }

        public void SetHasVoted(Citizen citizen, int cprKey)
        {
            var connection = GetSqlConnection();
            try
            {
                MySqlCommand getCpr = new MySqlCommand(
                    "SELECT cpr FROM person WHERE id='" + citizen.DbId + "'", connection);
                int citizenKeyPhrase = Convert.ToInt32(getCpr.ToString().Substring(7, 4));
                if (cprKey == citizenKeyPhrase)
                {
                    try
                    {
                        MySqlCommand setHasVoted = new MySqlCommand("SELECT person SET has_voted = '1' WHERE id='" + citizen.DbId + "'", connection);
                        citizen.SetHasVoted();
                        //return true;
                    }
                    catch (Exception ex)
                    {
                        throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
                    }
                }
                //return false;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
            }
        }

        public void SetHasVoted(Citizen citizen)
        {
            var connection = GetSqlConnection();
            try
            {
                MySqlCommand setHasVoted = new MySqlCommand("SELECT person SET has_voted = '1' WHERE id='" + citizen.DbId + "'", connection);
                if (setHasVoted.ExecuteNonQuery() == 1)
                {
                    //return true;
                }
                else
                {
                    //return false;
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
            }
        }

        public void ChangePassword(User user, string newPassword)
        {
            var connection = GetSqlConnection();

            try
            {
                User u = LoadUser(user.DBId);
                u.ChangePassword(newPassword);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
            }

        }

        public void ChangePassword(User user, string newPasswordHash, string oldPasswordHash)
        {
            var connection = GetSqlConnection();
            try
            {
                User u = LoadUser(user.DBId);
                u.ChangePassword(oldPasswordHash, newPasswordHash);

                MySqlCommand loadUser = new MySqlCommand("SELECT id FROM user WHERE id=@id AND password_hash=@oldPasswordHash", connection);
                loadUser.Prepare();
                loadUser.Parameters.AddWithValue("id", user.DbId);
                loadUser.Parameters.AddWithValue("oldPasswordHash", oldPasswordHash);
                if (loadUser.ExecuteScalar() != null) ChangePassword(user.DbId, newPasswordHash);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
            }
        }

        private bool ChangePassword(int userDbId, string newPasswordHash)
        {
            var connection = this.GetSqlConnection();
            MySqlCommand changePassword = new MySqlCommand("UPDATE user SET password_hash=@newPasswordHash WHERE id=@id", connection);
            changePassword.Prepare();
            changePassword.Parameters.AddWithValue("id", userDbId);
            changePassword.Parameters.AddWithValue("newPasswordHash", newPasswordHash);
            return changePassword.ExecuteNonQuery() == 1;
        }

        public void MarkUserInvalid(User user)
        {

        }

        public void RestoreUser(User user)
        {

        }

        public void MarkVoterCardInvalid(VoterCard vc)
        {
            var connection = this.GetSqlConnection();
            try
            {
                MySqlCommand setInvalid = new MySqlCommand("SELECT voter_card SET valid = '0' WHERE id='" + vc.Id + "'", connection);
                if (setInvalid.ExecuteNonQuery() == 1)
                {
                    //return true;
                }
                //return false;
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
