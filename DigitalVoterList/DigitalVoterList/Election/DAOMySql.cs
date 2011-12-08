

using System.Data;
using System.Diagnostics.Contracts;
using MySql.Data.MySqlClient;

namespace DigitalVoterList.Election
{
    using System;
    using System.Collections.Generic;

    public class DAOMySql : IDataAccessObject
    {

        private DAOMySql()
        {
        }

        public static IDataAccessObject GetDAO(User u)
        {
            return new DAOPermissionProxy(u, new DAOMySql());
        }

        #region Implementation of IDataAccessObject

        //DØD BABY!
        /*public Person LoadPerson(string cpr)
        {
            Person p;
            DoTransaction(() => { p = PriLoadPerson(cpr); });
            return p;
        }*/

        public Person LoadPerson(string cpr)
        {
            return (Person)LoadWithTransaction(() => PriLoadPerson(cpr));
        }

        /*private Person PriLoadPerson(string cpr)
        {
            Contract.Requires(CprExists(cpr, trans));
            MySqlCommand command = Prepare("SELECT id FROM person WHERE cpr=@cpr");
            command.Parameters.AddWithValue("@cpr", cpr);
            int personId = 0;
            Query(command, (object id) => personId = (int)id);
            return LoadPerson(personId);
        }*/


        //Other way...
        private Person PriLoadPerson(string cpr)
        {
            Contract.Requires(Find(new Person() { Cpr = cpr }).Count == 1);
            Contract.Ensures(Contract.Result<Person>() != null);
            MySqlCommand command = Prepare("SELECT id FROM person WHERE cpr=@cpr");
            command.Parameters.AddWithValue("@cpr", cpr);
            int id = 0;
            Query(command, (object o) => { id = (int)o; });
            return PriLoadPerson(id);
        }


        /// <summary>
        /// What person has this id?
        /// </summary>
        /// <param name="id">The database id of the person to load</param>
        /// <returns>The Person object loaded from the database</returns>
        public Person LoadPerson(int id)
        {
            return (Person)LoadWithTransaction(() => PriLoadPerson(id));
        }

        private Person PriLoadPerson(int id)
        {
            //Contract.Requires(_transaction != null); //Must be included for all "Pri" methods.. Requirement to even make Contracts!!
            //Contract.Requires(PersonExistsWithId(id, trans));
            MySqlCommand command = Prepare("SELECT * FROM person WHERE id=@id");
            command.Parameters.AddWithValue("id", id);
            Person p = new Person(id);
            Query(command, (MySqlDataReader rdr) =>
            {
                rdr.Read();
                p.Name = rdr.GetString("name");
                p.PassportNumber = rdr.GetString("passport_number");
            });
            return p;
        }

        /// <summary>
        /// What user has this username?
        /// </summary>
        /// <param name="username">The username to search for</param>
        /// <returns>A user object</returns>
        public User LoadUser(string username)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// What authenticated user exists with this username and password?
        /// </summary>
        /// <param name="username">The username to search for</param>
        /// <param name="password">The password to validate with</param>
        /// <returns>An authenticated user object, null if no user matched the details</returns>
        public User LoadUser(string username, string password)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// What user has this id?
        /// </summary>
        /// <param name="id">The database id of the user to load</param>
        /// <returns>An unauthenticated user obejct.</returns>
        public User LoadUser(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Is this username and hashed password combination valid?
        /// </summary>
        /// <param name="username">The username to validate</param>
        /// <param name="passwordHash">The passwordHash to validate with</param>
        /// <returns>The id of a validated user. 0 if no user can be found.</returns>
        public bool ValidateUser(string username, string passwordHash)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the permissions for the supplied user
        /// </summary>
        /// <param name="u">The user to get permissions for</param>
        /// <returns>A set of allowed actions</returns>
        public HashSet<SystemAction> GetPermissions(User u)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the workplace(s) for the supplied user
        /// </summary>
        /// <param name="u">The user to get workplaces for</param>
        /// <returns>The voting venues where the user works</returns>
        public HashSet<VotingVenue> GetWorkplaces(User u)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// What voter card has this id?
        /// </summary>
        /// <param name="id">The database id of the voter card to load</param>
        /// <returns>A voter card</returns>
        public VoterCard LoadVoterCard(int id)
        {
            throw new NotImplementedException();
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
        public List<Person> Find(Person person)
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
        /// <param name="person">The person to register</param>
        /// <returns>Was the attempt successful?</returns>
        public void Save(Person person)
        {
            throw new NotImplementedException();
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
        private string _connectionString = "SERVER=localhost;" +
                "DATABASE=px3;" +
                "UID=root;" +
                "PASSWORD=abcd1234;";
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
        private void DoTransaction(Action act)
        {
            _transaction = Connection.BeginTransaction(IsolationLevel.Serializable);
            try
            {
                act();
                _transaction.Commit();
            }
            catch (Exception ex)
            {
                try
                {
                    _transaction.Rollback();
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
                return _preparedStatements[query];
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
            }
        }

        //Brug en scalar
        private void Query(MySqlCommand cmd, Action<object> func)
        {
            try
            {
                object o = cmd.ExecuteScalar();
                func(o);
            }
            catch (Exception ex)
            {
                //TODO: Write some catch shit...!!!!
            }
        }

        //Returner scalar
        private object Query(MySqlCommand cmd, Func<object, object> func)
        {
            object o = null;
            Query(cmd, (object obj) => { o = func(obj); });
            return o;
        }

        //Brug en reader
        private void Query(MySqlCommand cmd, Action<MySqlDataReader> func)
        {
            try
            {
                MySqlDataReader rdr = cmd.ExecuteReader();
                func(rdr);
                rdr.Close();
            }
            catch (Exception ex)
            {
                //TODO: Write some catch shit...!!!!
            }
        }
        #endregion
    }
}
