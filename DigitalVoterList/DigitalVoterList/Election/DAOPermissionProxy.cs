using System.Collections.Generic;

namespace DigitalVoterList.Election
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// A proxy to handle permissions for data access actions
    /// </summary>
    // TODO: Consider voting-venues?
    public class DAOPermissionProxy : IDataAccessObject
    {
        private readonly User _user;
        private readonly IDataAccessObject _dao;

        public DAOPermissionProxy(User u, IDataAccessObject dao)
        {
            _user = u;
            _dao = dao;
        }

        public bool ActionPermitted(SystemAction a)
        {
            Contract.Ensures(
                (!_user.HasPermission(a) && Contract.Result<bool>() == false)
                || (_user.HasPermission(a) && Contract.Result<bool>() == true));

            return _user.HasPermission(a);
        }

        private void TestPermission(SystemAction a, string msg)
        {
            if (!this.ActionPermitted(a)) throw new PermissionException(a, _user, msg);
        }

        private void TestWorksHere(VotingVenue votingVenue)
        {
            if (!this.WorksHere(votingVenue)) throw new PermissionException(_user, "You don't work at this voting venue");
        }

        public bool CorrectUser(User user)
        {
            Contract.Requires(user != null);
            return user.Equals(_user);
        }

        private void TestCorrectUser(User user)
        {
            Contract.Requires(user != null);
            if (!this.CorrectUser(user)) throw new PermissionException(_user, "You must be logged as this user");
        }

        private bool WorksHere(VotingVenue v, string msg = "You can't perform this action, as you don't work in the right voting venue")
        {
            return _user.Workplaces.Contains(v);
        }

        public Citizen LoadCitizen(int id)
        {
            Contract.Requires(this.ActionPermitted(SystemAction.LoadPerson));
            this.TestPermission(SystemAction.LoadPerson, "You dont have permission to load information about citizens");
            return _dao.LoadCitizen(id);
        }

        public Citizen LoadCitizen(string cpr)
        {
            Contract.Requires(this.ActionPermitted(SystemAction.LoadPerson));
            this.TestPermission(SystemAction.LoadPerson, "You dont have permission to load information about citizens");
            return _dao.LoadCitizen(cpr);
        }

        public User LoadUser(string username)
        {
            return _dao.LoadUser(username);
        }

        public User LoadUser(int id)
        {
            Contract.Requires(this.ActionPermitted(SystemAction.LoadUser));
            this.TestPermission(SystemAction.LoadUser, "You dont have permission to load information about users");
            return _dao.LoadUser(id);
        }

        public bool ValidateUser(string username, string passwordHash)
        {
            return _dao.ValidateUser(username, passwordHash);
        }

        public HashSet<SystemAction> GetPermissions(User u)
        {
            return _dao.GetPermissions(u);
        }

        public HashSet<VotingVenue> GetWorkplaces(User u)
        {
            return _dao.GetWorkplaces(u);
        }

        public HashSet<VotingVenue> Workplaces(User u)
        {
            return _dao.GetWorkplaces(u);
        }

        public VoterCard LoadVoterCard(int id)
        {
            Contract.Requires(this.ActionPermitted(SystemAction.LoadVoterCard));
            this.TestPermission(SystemAction.LoadVoterCard, "You dont have permission to load information about votercards");
            return _dao.LoadVoterCard(id);
        }

        public VoterCard LoadVoterCard(string idKey)
        {
            Contract.Requires(this.ActionPermitted(SystemAction.ScanVoterCard));
            this.TestPermission(SystemAction.ScanVoterCard, "You dont have permission to scan votercards");
            return _dao.LoadVoterCard(idKey);
        }

        /// <summary>
        /// Update votercards
        /// </summary>
        public void UpdateVoterCards()
        {
            Contract.Requires(this.ActionPermitted(SystemAction.UpdateVoterCards));
            this.TestPermission(SystemAction.UpdateVoterCards, "You dont have permission to update votercards");
            _dao.UpdateVoterCards();
        }

        public List<Citizen> FindCitizens(Dictionary<CitizenSearchParam, object> data, SearchMatching matching)
        {
            Contract.Requires(this.ActionPermitted(SystemAction.FindPerson));
            this.TestPermission(SystemAction.FindPerson, "you don't have permission to search for citizens");
            return _dao.FindCitizens(data, matching);
        }

        public List<Citizen> FindCitizens(Dictionary<CitizenSearchParam, object> data)
        {
            return FindCitizens(data, SearchMatching.Similair);
        }

        public List<User> FindUsers(Dictionary<UserSearchParam, object> data, SearchMatching matching)
        {
            Contract.Requires(this.ActionPermitted(SystemAction.FindUser));
            this.TestPermission(SystemAction.FindUser, "You don't have permission to search for users");
            return _dao.FindUsers(data, matching);
        }

        public List<User> FindUsers(Dictionary<UserSearchParam, object> data)
        {
            return FindUsers(data, SearchMatching.Similair);
        }

        public List<VoterCard> FindVoterCards(Dictionary<VoterCardSearchParam, object> data, SearchMatching matching)
        {
            Contract.Requires(this.ActionPermitted(SystemAction.FindVoterCard));
            this.TestPermission(SystemAction.FindVoterCard, "You don't have permission to search for votercards");
            return _dao.FindVoterCards(data, matching);
        }

        public List<VoterCard> FindVoterCards(Dictionary<VoterCardSearchParam, object> data)
        {
            return FindVoterCards(data, SearchMatching.Similair);
        }

        public void Save(User user)
        {
            Contract.Requires(this.ActionPermitted(SystemAction.SaveUser));
            this.TestPermission(SystemAction.SaveUser, "You don't have permission to save users");
            _dao.Save(user);
        }

        public void Save(VoterCard voterCard)
        {
            Contract.Requires(this.ActionPermitted(SystemAction.SaveVoterCard));
            this.TestPermission(SystemAction.SaveVoterCard, "You don't have permission to save votercards");
            _dao.Save(voterCard);
        }

        public void SetHasVoted(Citizen citizen, string cprKey)
        {
            Contract.Requires(this.ActionPermitted(SystemAction.SetHasVoted));
            Contract.Requires(this.WorksHere(citizen.VotingPlace));
            this.TestPermission(SystemAction.SetHasVoted, "You don't have permission register voting");
            this.TestWorksHere(citizen.VotingPlace);
            _dao.SetHasVoted(citizen, cprKey);
        }

        public void SetHasVoted(Citizen citizen)
        {
            Contract.Requires(this.ActionPermitted(SystemAction.SetHasVotedManually));
            this.TestPermission(SystemAction.SetHasVotedManually, "You don't have permission to register voting without a key");

            _dao.SetHasVoted(citizen);
        }

        public void ChangePassword(User user, string newPasswordHash, string oldPasswordHash)
        {
            Contract.Requires(ActionPermitted(SystemAction.ChangeOwnPassword));
            Contract.Requires(this.CorrectUser(user));
            this.TestPermission(SystemAction.ChangeOwnPassword, "You don't have permission to change your own password");
            this.TestCorrectUser(user);
            _dao.ChangePassword(user, newPasswordHash, oldPasswordHash);
        }

        public void ChangePassword(User user, string newPasswordHash)
        {
            Contract.Requires(this.ActionPermitted(SystemAction.ChangeOthersPassword));
            this.TestPermission(SystemAction.ChangeOthersPassword, "You don't have permission to changed users passwords");
            _dao.ChangePassword(user, newPasswordHash);
        }

        public void MarkUserInvalid(User user)
        {
            Contract.Requires(this.ActionPermitted(SystemAction.MarkUserInvalid));
            this.TestPermission(SystemAction.MarkUserInvalid, "You don't have permission to mark this user invalid");
            _dao.MarkUserInvalid(user);
        }

        public void RestoreUser(User user)
        {
            Contract.Requires(this.ActionPermitted(SystemAction.RestoreUser));
            this.TestPermission(SystemAction.RestoreUser, "You don't have permission to restore this user");
            _dao.RestoreUser(user);
        }

        public void UpdatePeople(Func<Citizen, RawPerson, Citizen> update)
        {
            Contract.Requires(this.ActionPermitted(SystemAction.UpdateCitizens));
            this.TestPermission(SystemAction.UpdateCitizens, "You don't have permission to update citizen data.");
            _dao.UpdatePeople(update);
        }



    }
}
