using System.Collections.Generic;


namespace DigitalVoterList.Election
{

    using System;


    /// <summary>
    /// An object to access the database in a standardized way.
    /// </summary>
    public interface IDataAccessObject
    {
        /// <summary>
        /// What Citizen has this id?
        /// </summary>
        /// <param name="id">The database id of the person to load</param>
        /// <returns>The Person object loaded from the database</returns>
        Citizen LoadCitizen(int id);

        /// <summary>
        /// What Citizen has this cpr?
        /// </summary>
        /// <param name="cpr">The CPR to search for</param>
        /// <returns>A citizen that matches the given CPR</returns>
        Citizen LoadCitizen(string cpr);

        /// <summary>
        /// What user has this username?
        /// </summary>
        /// <param name="username">The username to search for</param>
        /// <returns>A user object</returns>
        User LoadUser(string username);

        /// <summary>
        /// What user has this id?
        /// </summary>
        /// <param name="id">The database id of the user to load</param>
        /// <returns>An unauthenticated user obejct.</returns>
        User LoadUser(int id);

        /// <summary>
        /// Is this username and hashed password combination valid?
        /// </summary>
        /// <param name="username">The username to validate</param>
        /// <param name="passwordHash">The passwordHash to validate with</param>
        /// <returns>The id of a validated user. 0 if no user can be found.</returns>
        bool ValidateUser(string username, string passwordHash);

        /// <summary>
        /// Get the permissions for the supplied user
        /// </summary>
        /// <param name="u">The user to get permissions for</param>
        /// <returns>A set of allowed actions</returns>
        HashSet<SystemAction> GetPermissions(User u);

        /// <summary>
        /// Get the workplace(s) for the supplied user
        /// </summary>
        /// <param name="u">The user to get workplaces for</param>
        /// <returns>The voting venues where the user works</returns>
        HashSet<VotingVenue> GetWorkplaces(User u);

        /// <summary>
        /// What voter card has this id?
        /// </summary>
        /// <param name="id">The database id of the voter card to load</param>
        /// <returns>A voter card</returns>
        VoterCard LoadVoterCard(int id);

        /// <summary>
        /// What voter card has this id-key?
        /// </summary>
        /// <param name="idKey">The id-key to search for</param>
        /// <returns>A voter card</returns>
        VoterCard LoadVoterCard(string idKey);

        /// <summary>
        /// What persons exists with data similiar to this data?
        /// </summary>
        /// <param name="data">The data to search with</param>
        /// <param name="matching">What type of search matching to use</param>
        /// <returns>A list of ctiizens that have the specified data.</returns>
        List<Citizen> FindCitizens(Dictionary<CitizenSearchParam, object> data, SearchMatching matching);

        /// <summary>
        /// What users exists with data similiar to this data?
        /// </summary>
        /// <param name="data">The data to search with</param>
        /// <param name="matching">What type of search matching to use</param>
        /// <returns>A list of users that have the specified data</returns>
        List<User> FindUsers(Dictionary<UserSearchParam, object> data, SearchMatching matching);

        /// <summary>
        /// What votercards exists with data similiar to this data?
        /// </summary>
        /// <param name="data">The data to search with</param>
        /// <param name="matching">What type of search matching to use</param>
        /// <returns>A list of voter cards that have the specified data</returns>
        List<VoterCard> FindVoterCards(Dictionary<VoterCardSearchParam, object> data, SearchMatching matching);

        /// <summary>
        /// What persons exists with data similiar to this data?
        /// </summary>
        /// <param name="data">The data to search with</param>
        /// <returns>A list of ctizens that have data similair to the specified data.</returns>
        List<Citizen> FindCitizens(Dictionary<CitizenSearchParam, object> data);

        /// <summary>
        /// What users exists with data similiar to this data?
        /// </summary>
        /// <param name="data">The data to search with</param>
        /// <returns>A list of users that have data similair to the specified data</returns>
        List<User> FindUsers(Dictionary<UserSearchParam, object> data);

        /// <summary>
        /// What votercards exists with data similiar to this data?
        /// </summary>
        /// <param name="data">The data to search with</param>
        /// <returns>A list of voter cards that have data similair to the specified data</returns>
        List<VoterCard> FindVoterCards(Dictionary<VoterCardSearchParam, object> data);

        /// <summary>
        /// Create this user with this data!
        /// </summary>
        /// <param name="user">The user to register</param>
        /// <returns>Was the attempt successful?</returns>
        void Save(User user);

        /// <summary>
        /// Create this voter card with this data!
        /// </summary>
        /// <param name="voterCard">The voter card to register</param>
        /// <returns>Was the attempt successful?</returns>
        void Save(VoterCard voterCard);

        /// <summary>
        /// Mark that a voter has voted with standard validation!
        /// </summary>
        /// <param name="citizen">The citizen who should be marked as voted</param>
        /// <param name="cprKey">The last four digits of the citizen's CPR-Number</param>
        /// <returns>Was the attempt successful?</returns>
        void SetHasVoted(Citizen citizen, string cprKey);

        /// <summary>
        /// Mark that a voter has voted with manual validation!
        /// </summary>
        /// <param name="citizen">The citizen who should be marked as voted</param>
        /// <returns>Was the attempt successful?</returns>
        void SetHasVoted(Citizen citizen);

        /// <summary>
        /// Change this users pasword to this!
        /// </summary>
        /// <param name="user">The user whose password should be changed</param>
        /// <param name="newPasswordHash">The hash of the new password to use</param>
        /// <param name="oldPasswordHash">The hash of the old password for this user.</param>
        /// <returns>Was the attempt succesful?</returns>
        void ChangePassword(User user, string newPasswordHash, string oldPasswordHash);

        /// <summary>
        /// Change this users password to this!
        /// </summary>
        /// <param name="user">The user whose password should be changed</param>
        /// <param name="newPasswordHash">The hash of the new password to use</param>
        /// <returns>Was th attempt succesful?</returns>
        void ChangePassword(User user, string newPasswordHash);

        /// <summary>
        /// Mark this user as invalid!
        /// </summary>
        /// <param name="user">The user who should be marked as invalid</param>
        /// <returns>Was the attempt succesful?</returns>
        void MarkUserInvalid(User user);

        /// <summary>
        /// Mark this invalid user as valid again.
        /// </summary>
        /// <param name="user">The user to mark as valid</param>
        /// <returns>Was the attempt succesful</returns>
        void RestoreUser(User user);

        /// <summary>
        /// Update all persons in the dataset with this update
        /// </summary>
        /// <param name="voterCard"></param>
        /// <param name="update">The update function</param>
        void UpdatePeople(Func<Citizen, RawPerson, Citizen> update);
    }
}
