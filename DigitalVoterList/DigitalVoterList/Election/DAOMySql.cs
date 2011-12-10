

using System.Data;
using System.Diagnostics.Contracts;
using MySql.Data.MySqlClient;

namespace DigitalVoterList.Election
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class DAOMySql : IDataAccessObject
    {
        #region Constructor and factory
        private DAOMySql(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static IDataAccessObject GetDAO(User u, string connectionString)
        {
            return new DAOPermissionProxy(u, new DAOMySql(connectionString));
        }
        #endregion

        #region Implementation of IDataAccessObject

        public Citizen LoadCitizen(string cpr)
        {
            Contract.Requires(cpr != null);
            return (Citizen)LoadWithTransaction(() => PriLoadCitizen(cpr));
        }

        private Citizen PriLoadCitizen(string cpr)
        {
            Contract.Requires(_transaction != null, "This method must be performed in a transaction.");
            Contract.Requires(cpr != null);
            Contract.Requires(Find(new Person() { Cpr = cpr }).Count == 1);
            Contract.Requires(_transaction != null);
            Contract.Ensures(Contract.Result<Person>() != null);
            MySqlCommand command = Prepare("SELECT id FROM person WHERE cpr=@cpr");
            command.Parameters.AddWithValue("@cpr", cpr);
            int id = (int)ScalarQuery(command);
            return PriLoadCitizen(id);
        }

        public Citizen LoadCitizen(int id)
        {
            return (Citizen)LoadWithTransaction(() => PriLoadCitizen(id));
        }

        private Citizen PriLoadCitizen(int id)
        {
            Contract.Requires(_transaction != null, "This method must be performed in a transaction.");
            Contract.Requires(ExistsWithId("person", id), "Person must exist in the database to be loaded.");
            Contract.Requires(HasValidCpr(id), "A citizen must have a valid CPR number");
            Contract.Ensures(Contract.Result<Person>() != null);
            MySqlCommand command = Prepare("SELECT *, v.name venue_name, v.address venue_address FROM person p LEFT JOIN voting_venue v ON v.id=p.voting_venue_id WHERE p.id=@id");
            command.Parameters.AddWithValue("@id", id);
            Citizen c = null;
            Query(command, (MySqlDataReader rdr) =>
            {
                rdr.Read();
                c = new Citizen(id, rdr.GetString("cpr"), rdr.GetInt32("has_voted") != 0);
                c.EligibleToVote = rdr.GetInt16("eligible_to_vote") == 1;
                DoIfNotDbNull(rdr, "voting_venue_id", label =>
                {
                    c.VotingPlace = new VotingVenue(
                        rdr.GetInt32(label),
                        rdr.GetString("venue_name"),
                        rdr.GetString("venue_address"));
                });
                DoIfNotDbNull(rdr, "name", lbl => { c.Name = rdr.GetString(lbl); });
                DoIfNotDbNull(rdr, "address", lbl => { c.Address = rdr.GetString(lbl); });
                DoIfNotDbNull(rdr, "place_of_birth", lbl => { c.PlaceOfBirth = rdr.GetString(lbl); });
                DoIfNotDbNull(rdr, "passport_number", lbl => { c.PassportNumber = rdr.GetString(lbl); });
            });
            return c;
        }

        //Does this id exist in this database table?
        private bool ExistsWithId(string tableName, int id)
        {
            Contract.Requires(_transaction != null, "This method must be performed in a transaction.");
            Contract.Requires(tableName != null);
            Contract.Requires(id > 0);
            MySqlCommand cmd = Prepare("SELECT id FROM @tableName WHERE id=@id");
            cmd.Parameters.AddWithValue("@tableName", tableName);
            cmd.Parameters.AddWithValue("@id", id);
            object found = ScalarQuery(cmd);
            return found != null;
        }

        private bool HasValidCpr(int citizenId)
        {
            Contract.Requires(_transaction != null, "This method must be performed in a transaction.");
            Contract.Requires(citizenId > 0, "A valid database id must be greater than zero.");
            MySqlCommand cmd = Prepare("SELECT cpr FROM person WHERE id=@id");
            cmd.Parameters.AddWithValue("@id", citizenId);
            object result = ScalarQuery(cmd);
            string cpr = (string)(result ?? "");
            return Citizen.ValidCpr(cpr);
        }

        /// <summary>
        /// What user has this username?
        /// </summary>
        /// <param name="username">The username to search for</param>
        /// <returns>A user object, null if no user could be found</returns>
        public User LoadUser(string username)
        {
            User user = null;
            DoTransaction(() => { user = PriLoadUser(username); });
            return user;
        }

        private User PriLoadUser(string username)
        {
            Contract.Requires(_transaction != null, "This method must be performed in a transaction.");
            Contract.Requires(username != null, "Input username must not be null!");
            MySqlCommand cmd = Prepare("SELECT id FROM user WHERE user_name=@username");
            cmd.Parameters.AddWithValue("@username", username);
            object maybeId = ScalarQuery(cmd);
            if (maybeId == null) return null;
            int id = (int)maybeId;
            return PriLoadUser(id);
        }

        /// <summary>
        /// What authenticated user exists with this username and password?
        /// </summary>
        /// <param name="username">The username to search for</param>
        /// <param name="password">The passwordHash to validate with</param>
        /// <returns>An authenticated user object, null if no user matched the details</returns>
        public User LoadUser(string username, string passwordHash)
        {
            Contract.Requires(username != null, "The input username must not be null!");
            Contract.Requires(passwordHash != null, "The input password must not be null!");
            User u = null;
            DoTransaction(() =>
                              {
                                  bool valid = PriValidateUser(username, passwordHash);
                                  if (valid)
                                  {
                                      u = PriLoadUser(username);
                                  }
                              });
            if (u != null) u.FetchPermissions(username, passwordHash);
            return u;
        }

        /// <summary>
        /// What user has this id?
        /// </summary>
        /// <param name="id">The database id of the user to load</param>
        /// <returns>An unauthenticated user obejct.</returns>
        public User LoadUser(int id)
        {
            Contract.Requires(id > 0, "The input id must be larger than zero.");
            Contract.Requires(ExistsWithId("user", id));
            User u = null;
            DoTransaction(() => { u = PriLoadUser(id); });
            return u;
        }

        private User PriLoadUser(int id)
        {
            Contract.Requires(_transaction != null, "This method must be performed in a transaction.");
            Contract.Requires(ExistsWithId("user", id), "User must exist in the database to be loaded.");
            Contract.Requires(id > 0, "The input id must be larger than zero.");
            Contract.Ensures(Contract.Result<User>() != null);
            MySqlCommand cmd = Prepare("SELECT * FROM user u INNER JOIN person p ON u.person_id=p.id WHERE u.id=@id");
            cmd.Parameters.AddWithValue("@id", id);
            User u = new User(id);
            Query(cmd, rdr =>
                           {
                               rdr.Read();
                               DoIfNotDbNull(rdr, "name", lbl => { u.Name = rdr.GetString(lbl); });
                               DoIfNotDbNull(rdr, "address", lbl => { u.Address = rdr.GetString(lbl); });
                               DoIfNotDbNull(rdr, "cpr", lbl => { u.Cpr = rdr.GetString(lbl); });
                               DoIfNotDbNull(rdr, "place_of_birth", lbl => { u.PlaceOfBirth = rdr.GetString(lbl); });
                               DoIfNotDbNull(rdr, "passport_number", lbl => { u.PassportNumber = rdr.GetString(lbl); });
                               u.Username = rdr.GetString("user_name");
                               u.Title = rdr.GetString("title");
                               u.UserSalt = rdr.GetString("user_salt");
                           });
            return u;
        }

        /// <summary>
        /// Is this username and hashed password combination valid?
        /// </summary>
        /// <param name="username">The username to validate</param>
        /// <param name="passwordHash">The passwordHash to validate with</param>
        /// <returns>The id of a validated user. 0 if no user can be found.</returns>
        public bool ValidateUser(string username, string passwordHash)
        {
            Contract.Requires(username != null, "The username must not be null!");
            Contract.Requires(passwordHash != null, "The password hash must not be null!");
            object result = LoadWithTransaction(() => PriValidateUser(username, passwordHash));
            return (bool)result;
        }

        private bool PriValidateUser(string username, string passwordHash)
        {
            Contract.Requires(_transaction != null, "This method must be performed in a transaction.");
            Contract.Requires(username != null, "The username must not be null!");
            Contract.Requires(passwordHash != null, "The password hash must not be null!");
            MySqlCommand cmd = Prepare("SELECT id FROM user WHERE user_name=@username AND password_hash=@passwordHash");
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@passwordHash", passwordHash);
            object result = ScalarQuery(cmd);
            if (result == null) return false;
            int output = (int)result;
            return output == 1;
        }

        /// <summary>
        /// Get the permissions for the supplied user
        /// </summary>
        /// <param name="u">The user to get permissions for</param>
        /// <returns>A set of allowed actions</returns>
        public HashSet<SystemAction> GetPermissions(User u)
        {
            Contract.Requires(u != null, "Input user must not be null!");
            Contract.Ensures(Contract.Result<HashSet<SystemAction>>() != null);
            var result = new HashSet<SystemAction>();
            DoTransaction(() => { result = PriGetPermissions(u); });
            return result;
        }

        private HashSet<SystemAction> PriGetPermissions(User user)
        {
            Contract.Requires(_transaction != null, "This method must be performed in a transaction.");
            Contract.Requires(user != null, "The input user must not be null!");
            Contract.Ensures(Contract.Result<HashSet<SystemAction>>() != null);
            var output = new HashSet<SystemAction>();
            if (user.DbId < 1) return output; //The user CAN not exist in the database...
            MySqlCommand cmd =
                Prepare("SELECT a.label FROM action a INNER JOIN permission p ON a.id = p.action_id WHERE p.user_id=@id");
            cmd.Parameters.AddWithValue("@id", user.DbId);
            Query(cmd, rdr =>
                           {
                               while (rdr.Read())
                               {
                                   SystemAction action = SystemActions.getSystemAction(rdr.GetString(0));
                                   output.Add(action);
                               }
                           });
            return output;
        }

        /// <summary>
        /// Get the workplace(s) for the supplied user
        /// </summary>
        /// <param name="u">The user to get workplaces for</param>
        /// <returns>The voting venues where the user works</returns>
        public HashSet<VotingVenue> GetWorkplaces(User u)
        {
            Contract.Requires(u != null, "Input user must not be null!");
            Contract.Ensures(Contract.Result<HashSet<VotingVenue>>() != null);
            var result = new HashSet<VotingVenue>();
            DoTransaction(() => { result = PriGetWorkplaces(u); });
            return result;
        }

        private HashSet<VotingVenue> PriGetWorkplaces(User user)
        {
            Contract.Requires(_transaction != null, "This method must be performed in a transaction.");
            Contract.Requires(user != null, "The input user must not be null!");
            Contract.Ensures(Contract.Result<HashSet<VotingVenue>>() != null);
            var output = new HashSet<VotingVenue>();
            if (user.DbId < 1) return output; //The user CAN not exist in the database...
            MySqlCommand cmd =
                Prepare("SELECT v.id, v.address, v.name FROM user u INNER JOIN workplace w ON u.id = w.user_id INNER JOIN voting_venue v ON v.id = w.voting_venue_id WHERE u.id=@id");
            cmd.Parameters.AddWithValue("@id", user.DbId);
            Query(cmd, rdr =>
            {
                while (rdr.Read())
                {
                    VotingVenue venue = new VotingVenue(
                        rdr.GetInt32("id"),
                        rdr.GetString("name"),
                        rdr.GetString("address"));
                    output.Add(venue);
                }
            });
            return output;
        }

        /// <summary>
        /// What voter card has this id?
        /// </summary>
        /// <param name="id">The database id of the voter card to load</param>
        /// <returns>A voter card</returns>
        public VoterCard LoadVoterCard(int id)
        {
            return (VoterCard)LoadWithTransaction(() => PriLoadVoterCard(id));
        }

        private VoterCard PriLoadVoterCard(int id)
        {
            Contract.Requires(_transaction != null, "This method must be performed in a transaction.");
            Contract.Requires(ExistsWithId("votercard", id), "Votercard must exist in the database to be loaded.");
            Contract.Ensures(Contract.Result<VoterCard>() != null);
            MySqlCommand command = Prepare("SELECT * FROM votercard v LEFT JOIN person p ON p.id=v.person_id WHERE v.id=@id");
            command.Parameters.AddWithValue("@id", id);
            VoterCard v = null;
            Query(command, (MySqlDataReader rdr) =>
            {
                rdr.Read();
                //v.Citizen = PriLoadPerson(rdr.GetInt32("person_id"));
                v.ElectionEvent = Settings.Election;
                v.Id = rdr.GetInt32("id");
                v.IdKey = rdr.GetString("id_key");
                v.Valid = (rdr.GetUInt32("valid") == 1);
            });
            return v;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private MySqlCommand PrepareWithValues(string tableName, Dictionary<string, string> info)
        {
            Contract.Requires(tableName != null);
            Contract.Requires(info != null);

            var queryBuilder = new StringBuilder("SELECT * FROM " + tableName + " WHERE ");
            var insertAnd = false;

            foreach (KeyValuePair<String, String> entry in info)
            {
                if (string.IsNullOrEmpty(entry.Value)) continue;
                if (insertAnd) queryBuilder.Append(" AND ");
                queryBuilder.Append(entry.Key);
                queryBuilder.Append(" = @'");
                queryBuilder.Append(entry.Key);
                queryBuilder.Append("'");
                insertAnd = true;
            }
            queryBuilder.Append(";");

            var cmd = this.Prepare(queryBuilder.ToString());

            foreach (KeyValuePair<String, String> entry in info)
            {
                if (string.IsNullOrEmpty(entry.Value)) continue;
                cmd.Parameters.AddWithValue("@'" + entry.Key, entry.Value);
                insertAnd = true;
            }

            return cmd;
        }


        /// <summary>
        /// What voter card has this id-key?
        /// </summary>
        /// <param name="idKey">The id-key to search for</param>
        /// <returns>A voter card</returns>
        public VoterCard LoadVoterCard(string idKey)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// What persons exists with data similiar to this person?
        /// </summary>
        /// <param name="person">The person to use</param>
        /// <returns>A list of persons that are similair.</returns>
        public List<Citizen> Find(Person person)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// What users exists with data similiar to this user?
        /// </summary>
        /// <param name="user">The user to use</param>
        /// <returns>A list of users that are similair</returns>
        public List<User> Find(User user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// What votercards exists with data similiar to this votercard?
        /// </summary>
        /// <param name="voterCard">The voter card to use</param>
        /// <returns>A list of voter cards with similair data</returns>
        public List<VoterCard> Find(VoterCard voterCard)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// May I have all eligible voters in the database?
        /// </summary>
        /// <returns>A list of eligible voters</returns>
        public List<Citizen> FindElegibleVoters()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create this person with this data!
        /// </summary>
        /// <param name="citizen">The person to register</param>
        /// <returns>Was the attempt successful?</returns>
        public void Save(Citizen citizen)
        {
            Contract.Requires(citizen != null, "Input person must not be null!");
            Contract.Requires(citizen.DbId >= 0, "DbId must be greater than or equal to zero");
            Contract.Requires(!(citizen.DbId > 0) || ExistsWithId("person", citizen.DbId));
            Contract.Requires(Citizen.ValidCpr(citizen.Cpr));
            if (citizen.DbId > 0)
            {
                DoTransaction(() => PriSave(citizen));
            }
            else
            {
                DoTransaction(() => PriSaveNew(citizen));
            }

        }

        private void PriSave(Citizen citizen)
        {
            Contract.Requires(_transaction != null, "This method must be performed in a transaction.");
            Contract.Requires(citizen != null, "Input citizen must not be null!");
            Contract.Requires(citizen.DbId > 0, "DbId must be larger than zero to update");
            Contract.Requires(ExistsWithId("citizen", citizen.DbId), "DbId must be present in database in order to update anything");
            Contract.Requires(citizen.Cpr != null && Citizen.ValidCpr(citizen.Cpr), "A citizen must be saved with a valid CPR number");
            Contract.Requires(citizen.VotingPlace == null || ExistsWithId("voting_venue", citizen.VotingPlace.DbId), "If Citizen has a VotingPlace, it must exist in the database prior to saving.");
            Contract.Ensures(LoadCitizen(citizen.DbId).Equals(citizen), "All changes must be saved");
            MySqlCommand cmd = Prepare("UPDATE person SET name=@name, address=@address, cpr=@cpr, eligible_to_vote=@eligibleToVote, place_of_birth=@placeOfBirth, passport_number=@passportNumber, voting_venue_id=@votingVenueId WHERE id=@id");

            var mapping = new Dictionary<string, string>()
                              {
                                  {"id",citizen.DbId.ToString()},
                                  {"name",citizen.Name},
                                  {"address",citizen.Address},
                                  {"cpr",citizen.Cpr},
                                  {"eligibleToVote",citizen.EligibleToVote ? "1" : "0"},
                                  {"placeOfBirth",citizen.PlaceOfBirth},
                                  {"passportNumber",citizen.PassportNumber},
                                  {"votingVenueId",citizen.VotingPlace!=null? citizen.VotingPlace.DbId.ToString() : null} //Avoid null-pointer
                              };

            foreach (var kv in mapping)
            {
                if (!String.IsNullOrEmpty(kv.Value))
                {
                    cmd.Parameters.AddWithValue("@" + kv.Key, kv.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@" + kv.Key, null);
                }
            }
            Execute(cmd);
        }

        private void PriSaveNew(Citizen citizen)
        {
            Contract.Requires(_transaction != null, "This method must be performed in a transaction.");
            Contract.Requires(citizen != null, "Input citizen must not be null!");
            Contract.Requires(citizen.DbId == 0, "DbId must be equal to zero");
            Contract.Requires(ExistsWithId("citizen", citizen.DbId), "DbId must be present in database in order to update anything");
            Contract.Requires(citizen.Cpr != null && Citizen.ValidCpr(citizen.Cpr), "A citizen must be saved with a valid CPR number");
            Contract.Requires(citizen.VotingPlace == null || ExistsWithId("voting_venue", citizen.VotingPlace.DbId), "If Citizen has a VotingPlace, it must exist in the database prior to saving.");
            Contract.Ensures(LoadCitizen(citizen.DbId).Equals(citizen), "All changes must be saved");
            MySqlCommand cmd = Prepare("INSERT INTO person (name,address,cpr,eligible_to_vote,place_of_birth,passport_number,voting_venue_id) VALUES (@name, @address, @cpr, @eligibleToVote, @placeOfBirth, @passportNumber, @votingVenueId)");
            var mapping = new Dictionary<string, string>()
                              {
                                  {"name",citizen.Name},
                                  {"address",citizen.Address},
                                  {"cpr",citizen.Cpr},
                                  {"eligibleToVote",citizen.EligibleToVote ? "1" : "0"},
                                  {"placeOfBirth",citizen.PlaceOfBirth},
                                  {"passportNumber",citizen.PassportNumber},
                                  {"votingVenueId",citizen.VotingPlace != null ? citizen.VotingPlace.DbId.ToString() : null} //Avoid null-pointer
                              };
            foreach (var kv in mapping)
            {
                if (!String.IsNullOrEmpty(kv.Value))
                {
                    cmd.Parameters.AddWithValue("@" + kv.Key, kv.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@" + kv.Key, null);
                }
            }
            Execute(cmd);
        }

        /// <summary>
        /// Create this user with this data!
        /// </summary>
        /// <param name="user">The user to register</param>
        /// <returns>Was the attempt successful?</returns>
        public void Save(User user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create this voter card with this data!
        /// </summary>
        /// <param name="voterCard">The voter card to register</param>
        /// <returns>Was the attempt successful?</returns>
        public void Save(VoterCard voterCard)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Mark that a voter has voted with standard validation!
        /// </summary>
        /// <param name="citizen">The citizen who should be marked as voted</param>
        /// <param name="cprKey">The last four digits of the citizen's CPR-Number</param>
        /// <returns>Was the attempt successful?</returns>
        public void SetHasVoted(Citizen citizen, int cprKey)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Mark that a voter has voted with standard validation!
        /// </summary>
        /// <param name="citizen">The citizen who should be marked as voted</param>
        /// <param name="cprKey">The last four digits of the citizen's CPR-Number</param>
        /// <returns>Was the attempt successful?</returns>
        public void SetHasVoted(Citizen citizen, string cprKey)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Mark that a voter has voted with manual validation!
        /// </summary>
        /// <param name="citizen">The citizen who should be marked as voted</param>
        /// <returns>Was the attempt successful?</returns>
        public void SetHasVoted(Citizen citizen)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Change this users pasword to this!
        /// </summary>
        /// <param name="user">The user whose password should be changed</param>
        /// <param name="newPasswordHash">The hash of the new password to use</param>
        /// <param name="oldPasswordHash">The hash of the old password for this user.</param>
        /// <returns>Was the attempt succesful?</returns>
        public void ChangePassword(User user, string newPasswordHash, string oldPasswordHash)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Change this users password to this!
        /// </summary>
        /// <param name="user">The user whose password should be changed</param>
        /// <param name="newPasswordHash">The hash of the new password to use</param>
        /// <returns>Was th attempt succesful?</returns>
        public void ChangePassword(User user, string newPasswordHash)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Mark this user as invalid!
        /// </summary>
        /// <param name="user">The user who should be marked as invalid</param>
        /// <returns>Was the attempt succesful?</returns>
        public void MarkUserInvalid(User user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Mark this invalid user as valid again.
        /// </summary>
        /// <param name="user">The user to mark as valid</param>
        /// <returns>Was the attempt succesful</returns>
        public void RestoreUser(User user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Mark this voter card as invalid!
        /// </summary>
        /// <param name="voterCard">The voter card which should be marked as invalid</param>
        /// <returns>Was the attempt succesful?</returns>
        public void MarkVoterCardInvalid(VoterCard voterCard)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update all persons in the dataset with this update
        /// </summary>
        /// <param name="voterCard"></param>
        /// <param name="update">The update function</param>
        public void UpdatePeople(Func<Person, RawPerson, Person> update)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// What voting venue should this citizen use?
        /// </summary>
        /// <param name="citizen"></param>
        /// <returns></returns>
        public VotingVenue FindVotingVenue(Citizen citizen)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region private SQL features
        //We keep track of all open connections, to enable later maintainance of eventual onclosed connections..
        private MySqlConnection _connection; //Current connection
        private MySqlTransaction _transaction; //Current transaction
        private readonly string _connectionString; //The string to use when connecting to the database
        private Dictionary<string, MySqlCommand> _preparedStatements = new Dictionary<string, MySqlCommand>();

        /// <summary>
        /// An open connection for the database
        /// </summary>
        private MySqlConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = new MySqlConnection(_connectionString);
                    _connection.Open();
                }
                return _connection;
                /*if (_connection != null && _connection.State.Equals("Open"))
                {
                    return _connection;
                }
                else
                {
                    RetryUtility.RetryAction(() => Reconnect(), 3, 500);
                    return Connection;
                }*/
            }
        }

        /// <summary>
        /// Try to reconnect to the database
        /// </summary>
        private void Reconnect()
        {
            try
            {
                if (_connection != null) _connection.Close();
            }
            catch { }
            _connection = new MySqlConnection(_connectionString);
            _connection.Open();
            _preparedStatements = new Dictionary<string, MySqlCommand>();
        }

        /// <summary>
        /// Do this in a transaction, and handle all transaction and connection issues that might occur
        /// </summary>
        /// <param name="act">What to do...</param>
        private void DoTransaction(Action act, IsolationLevel isolationLevel)
        {
            try
            {
                _transaction = Connection.BeginTransaction(isolationLevel);
                act();
                _transaction.Commit();
            }
            catch (Exception ex)
            {
                throw;
                try
                {
                    _transaction.Rollback();
                    //todo: And retry? We can't just rollback the function, we need to try again..
                }
                catch (MySqlException excep)
                {
                    // TODO: Make a logging function and maybe a security alert...
                    throw;
                }
            }
            _transaction = null;
        }

        /// <summary>
        /// Do this in a transaction, and handle all transaction and connection issues that might occur
        /// </summary>
        /// <param name="act">What to do...</param>
        private void DoTransaction(Action act)
        {
            DoTransaction(act, IsolationLevel.Serializable);
        }

        /// <summary>
        /// Load this in a transaction, and handle all transaction and connection issues that might occur
        /// </summary>
        /// <param name="func">How to load this?</param>
        /// <returns></returns>
        private object LoadWithTransaction(Func<object> func)
        {
            object o = null;
            DoTransaction(() => { o = func(); });
            return o;
        }

        /// <summary>
        /// Prepare this string for query, on this 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private MySqlCommand Prepare(string query)
        {
            if (_preparedStatements.ContainsKey(query))
            {
                var ps = _preparedStatements[query];
                ps.Parameters.Clear(); //todo: I think we need this
                return ps;
            }
            MySqlCommand cmd = new MySqlCommand(query);
            cmd.Connection = Connection;
            if (_transaction != null) cmd.Transaction = _transaction;
            cmd.Prepare();
            _preparedStatements.Add(query, cmd);
            return cmd;
        }

        private void Execute(MySqlCommand cmd)
        {
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //TODO: Write some catch shit...!!!!
                throw;
            }
        }

        //Brug en scalar
        private void ScalarQuery(MySqlCommand cmd, Action<object> func)
        {
            try
            {
                object o = cmd.ExecuteScalar();
                func(o);
            }
            catch (Exception ex)
            {
                //TODO: Write some catch shit...!!!!
                throw;
            }
        }

        //Returner scalar
        private object ScalarQuery(MySqlCommand cmd)
        {
            object o = null;
            ScalarQuery(cmd, obj => o = obj);
            return o;
        }

        //Brug en reader
        private void Query(MySqlCommand cmd, Action<MySqlDataReader> func)
        {
            MySqlDataReader rdr = null;
            try
            {
                rdr = cmd.ExecuteReader();
                func(rdr);
            }
            catch (Exception ex)
            {
                //TODO: Write some catch shit...!!!!
                throw;
            }
            finally
            {
                if (rdr != null) rdr.Close();
            }
        }

        //Do this only if the supplied lable on the supplied reader is not db-null
        private void DoIfNotDbNull(MySqlDataReader rdr, string label, Action<string> act)
        {
            int ordinal = rdr.GetOrdinal(label);
            if (!rdr.IsDBNull(ordinal))
            {
                act(label);
            }
        }
        #endregion
    }
}
