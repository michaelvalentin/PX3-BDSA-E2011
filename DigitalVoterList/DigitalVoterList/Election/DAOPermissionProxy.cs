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

        private bool ActionPermitted(SystemAction a, string msg = "You don't have permission to perform this SystemAction.")
        {
            return true;

            Contract.Ensures(
                (!_user.HasPermission(a) && Contract.Result<bool>() == false)
                || (_user.HasPermission(a) && Contract.Result<bool>() == true));


            if (!_user.HasPermission(a))
            {
                //todo: Enten skal denne metode retunere en bool eller kaste en exception, ikke begge?
                throw new PermissionException(a, _user, msg);
                return false;
            }

            return true;
        }

        private bool ActionPermittedForThisUser(User user, SystemAction a, string msg = "You don't have permission to perform this SystemAction.")
        {
            if (user.Equals(_user)) return this.ActionPermitted(a, msg);
            return false;
        }

        private bool WorksHere(VotingVenue v, string msg = "You can't perform this action, as you don't work in the right voting venue")
        {
            return _user.Workplaces.Contains(v) || ActionPermitted(SystemAction.AllVotingPlaces);
        }

        public Citizen LoadCitizen(int id)
        {
            if (ActionPermitted(SystemAction.LoadPerson))
            {
                return _dao.LoadCitizen(id);
            }
            return null;
        }

        public Citizen LoadCitizen(string cpr)
        {
            if (ActionPermitted(SystemAction.LoadPerson))
            {
                return _dao.LoadCitizen(cpr);
            }
            return null;
        }

        public User LoadUser(string username)
        {
            return _dao.LoadUser(username);
        }

        public User LoadUser(int id)
        {
            if (ActionPermitted(SystemAction.LoadUser))
            {
                return _dao.LoadUser(id);
            }
            return null;
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
            if (ActionPermitted(SystemAction.LoadVoterCard))
            {
                return _dao.LoadVoterCard(id);
            }
            return null;
        }

        public VoterCard LoadVoterCard(string idKey)
        {
            if (ActionPermitted(SystemAction.ScanVoterCard))
            {
                return _dao.LoadVoterCard(idKey);
            }
            return null;
        }

        public List<Citizen> FindCitizens(Dictionary<CitizenSearchParam, object> data, SearchMatching matching)
        {
            if (ActionPermitted(SystemAction.FindPerson))
            {
                return _dao.FindCitizens(data, matching);
            }
            return null;
        }

        public List<User> FindUsers(Dictionary<UserSearchParam, object> data, SearchMatching matching)
        {
            if (ActionPermitted(SystemAction.FindUser))
            {
                return _dao.FindUsers(data, matching);
            }
            return null;
        }

        public List<VoterCard> FindVoterCards(Dictionary<VoterCardSearchParam, object> data, SearchMatching matching)
        {
            if (ActionPermitted(SystemAction.FindVoterCard))
            {
                return _dao.FindVoterCards(data, matching);
            }
            return null;
        }

        public List<Citizen> FindCitizens(Dictionary<CitizenSearchParam, object> data)
        {
            if (ActionPermitted(SystemAction.FindPerson))
            {
                return _dao.FindCitizens(data, SearchMatching.Similair);
            }
            return null;
        }

        public List<User> FindUsers(Dictionary<UserSearchParam, object> data)
        {
            if (ActionPermitted(SystemAction.FindUser))
            {
                return _dao.FindUsers(data, SearchMatching.Similair);
            }
            return null;
        }

        public List<VoterCard> FindVoterCards(Dictionary<VoterCardSearchParam, object> data)
        {
            if (ActionPermitted(SystemAction.FindVoterCard))
            {
                return _dao.FindVoterCards(data, SearchMatching.Similair);
            }
            return null;
        }

        public void Save(User user)
        {
            if (ActionPermitted(SystemAction.SaveUser))
            {
                _dao.Save(user);
            }
        }

        public void Save(VoterCard voterCard)
        {
            if (ActionPermitted(SystemAction.SaveVoterCard))
            {
                _dao.Save(voterCard);
            }
        }

        public void SetHasVoted(Citizen citizen, string cprKey)
        {
            if (ActionPermitted(SystemAction.SetHasVoted) && WorksHere(citizen.VotingPlace))
            {
                _dao.SetHasVoted(citizen, cprKey);
            }
        }

        public void SetHasVoted(Citizen citizen)
        {
            if (ActionPermitted(SystemAction.SetHasVotedManually))
            {
                _dao.SetHasVoted(citizen);
            }
        }

        public void ChangePassword(User user, string newPasswordHash, string oldPasswordHash)
        {
            if (ActionPermittedForThisUser(user, SystemAction.ChangeOwnPassword))
            {
                _dao.ChangePassword(user, newPasswordHash, oldPasswordHash);
            }
        }

        public void ChangePassword(User user, string newPasswordHash)
        {
            if (ActionPermitted(SystemAction.ChangeOthersPassword))
            {
                _dao.ChangePassword(user, newPasswordHash);
            }
        }

        public void MarkUserInvalid(User user)
        {
            if (ActionPermitted(SystemAction.MarkUserInvalid))
            {
                _dao.MarkUserInvalid(user);
            }
        }

        public void RestoreUser(User user)
        {
            if (ActionPermitted(SystemAction.RestoreUser))
            {
                _dao.RestoreUser(user);
            }
        }

        public void UpdatePeople(Func<Citizen, RawPerson, Citizen> update)
        {
            if (ActionPermitted(SystemAction.UpdatePeople))
            {
                _dao.UpdatePeople(update);
            }
        }
    }
}
