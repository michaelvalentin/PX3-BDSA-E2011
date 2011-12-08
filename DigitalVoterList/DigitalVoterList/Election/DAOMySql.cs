<<<<<<< HEAD
﻿
=======
﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

>>>>>>> 9435b06c1c4e53a1ee3c752186644408d37cc68d
namespace DigitalVoterList.Election
{
    using System;
    using System.Collections.Generic;

    public class DAOMySql : IDataAccessObject
    {
        public DAOMySql()
        {

        }

        #region Implementation of IDataAccessObject

        /// <summary>
        /// What person has this id?
        /// </summary>
        /// <param name="id">The database id of the person to load</param>
        /// <returns>The Person object loaded from the database</returns>
        public Person LoadPerson(int id)
        {
            throw new NotImplementedException();
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

<<<<<<< HEAD
        /// <summary>
        /// Mark that a voter has voted with standard validation!
        /// </summary>
        /// <param name="citizen">The citizen who should be marked as voted</param>
        /// <param name="cprKey">The last four digits of the citizen's CPR-Number</param>
        /// <returns>Was the attempt successful?</returns>
        public void SetHasVoted(Citizen citizen, int cprKey)
        {
            throw new NotImplementedException();
=======
        public void SetHasVoted(Citizen citizen, string cprKey)
        {
            var connection = GetSqlConnection();
            try
            {
                MySqlCommand getCpr = new MySqlCommand(
                    "SELECT cpr FROM person WHERE id='" + citizen.DbId + "'", connection);
                string cpr = (string)getCpr.ExecuteScalar();
                Regex cprKeyPattern = new Regex(".{6}" + cprKey);
                if (cprKeyPattern.IsMatch(cpr))
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
                else
                {
                    throw new DataAccessException("Wrong CPR-key");
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Unable to connect to database. Error message: " + ex.Message);
            }
>>>>>>> 9435b06c1c4e53a1ee3c752186644408d37cc68d
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
    }
}
