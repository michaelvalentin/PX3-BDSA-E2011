using System.Collections.Generic;

namespace DigitalVoterList.Election
{

    /// <summary>
    /// An object to access the database in a standardized way.
    /// </summary>
    public interface IDataAccessObject
    {
        /// <summary>
        /// What person has this id?
        /// </summary>
        /// <param name="id">The database id of the person to load</param>
        /// <returns>The Person object loaded from the database</returns>
        Person LoadPerson(int id);

        /// <summary>
        /// What user has this username and password?
        /// </summary>
        /// <param name="username">The username to search for</param>
        /// <param name="password">The password to authenticate with</param>
        /// <returns>An authenticated User object</returns>
        User LoadUser(string username, string password);

        /// <summary>
        /// What user has this id?
        /// </summary>
        /// <param name="id">The database id of the user to load</param>
        /// <returns>An unauthenticated user obejct.</returns>
        User LoadUser(int id);

        /// <summary>
        /// What voter card has this id?
        /// </summary>
        /// <param name="id">The database id of the voter card to load</param>
        /// <returns>A voter card</returns>
        VoterCard LoadVoterCard(int id);

        /// <summary>
        /// What voter card has this id-key?
        /// </summary>
        /// <param name="id_key">The id-key to search for</param>
        /// <returns>A voter card</returns>
        VoterCard LoadVoterCard(string id_key);

        /// <summary>
        /// What persons exists with data similiar to this person?
        /// </summary>
        /// <param name="person">The person to use</param>
        /// <returns>A list of persons that are similair.</returns>
        List<Person> Find(Person person);

        /// <summary>
        /// What users exists with data similiar to this user?
        /// </summary>
        /// <param name="user">The user to use</param>
        /// <returns>A list of users that are similair</returns>
        List<User> Find(User user);

        /// <summary>
        /// What votercards exists with data similiar to this votercard?
        /// </summary>
        /// <param name="voterCard">The voter card to use</param>
        /// <returns>A list of voter cards with similair data</returns>
        List<VoterCard> Find(VoterCard voterCard);

        /// <summary>
        /// Atempt to register this person!
        /// </summary>
        /// <param name="person">The person to register</param>
        /// <returns>Was the atempt successful?</returns>
        bool Save(Person person);

        /// <summary>
        /// Atempt to register this user!
        /// </summary>
        /// <param name="user">The user to register</param>
        /// <returns>Was the atempt successful?</returns>
        bool Save(User user);

        /// <summary>
        /// Atempt to register this voter card!
        /// </summary>
        /// <param name="voterCard">The voter card to register</param>
        /// <returns>Was the atempt successful?</returns>
        bool Save(VoterCard voterCard);
    }
}
