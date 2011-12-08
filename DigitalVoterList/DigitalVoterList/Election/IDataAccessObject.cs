﻿using System.Collections.Generic;


namespace DigitalVoterList.Election
{

    using System;


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
        /// What user has this username?
        /// </summary>
        /// <param name="username">The username to search for</param>
        /// <returns>A user object</returns>
        User LoadUser(string username);

        /// <summary>
        /// What authenticated user exists with this username and password?
        /// </summary>
        /// <param name="username">The username to search for</param>
        /// <param name="password">The password to validate with</param>
        /// <returns>An authenticated user object, null if no user matched the details</returns>
        User LoadUser(string username, string password);

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
        /// May I have all eligible voters in the database?
        /// </summary>
        /// <returns>A list of eligible voters</returns>
        List<Citizen> FindElegibleVoters();

        /// <summary>
        /// Create this person with this data!
        /// </summary>
        /// <param name="person">The person to register</param>
        /// <returns>Was the attempt successful?</returns>
        void Save(Person person);

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
        void SetHasVoted(Citizen citizen, int cprKey);

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
        /// Mark this voter card as invalid!
        /// </summary>
        /// <param name="voterCard">The voter card which should be marked as invalid</param>
        /// <returns>Was the attempt succesful?</returns>
        void MarkVoterCardInvalid(VoterCard voterCard);

        /// <summary>
        /// Update all persons in the dataset with this update
        /// </summary>
        /// <param name="voterCard"></param>
        /// <param name="update">The update function</param>
        void UpdatePeople(Func<Person, RawPerson, Person> update);

        /// <summary>
        /// What voting venue should this citizen use?
        /// </summary>
        /// <param name="citizen"></param>
        /// <returns></returns>
        VotingVenue FindVotingVenue(Citizen citizen);
    }
}
