using System.Collections.Generic;
using System.Diagnostics;

namespace DigitalVoterList.Election
{
    using System;

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
            if (!_user.HasPermission(a))
            {
                foreach (SystemAction ac in _user.Permissions)
                {
                    Debug.WriteLine("User permission: " + ac.ToString());
                }
                throw new PermissionException(a, _user, msg);
            }
            else
            {
                return true;
            }
        }

        private bool ActionPermittedForThisUser(User user, SystemAction a, string msg = "You don't have permission to perform this SystemAction.")
        {
            if (!user.Equals(_user) || !_user.HasPermission(a))
            {
                throw new PermissionException(a, _user, msg);
            }
            else
            {
                return true;
            }
        }

        private bool WorksHere(VotingVenue v, string msg = "You can't perform this action, as you don't work in the right voting venue")
        {
            return _user.Workplaces.Contains(v) || ActionPermitted(SystemAction.AllVotingPlaces);
        }

        public Person LoadPerson(int id)
        {
            if (ActionPermitted(SystemAction.LoadPerson))
            {
                return _dao.LoadPerson(id);
            }
            return null;
        }

        public User LoadUser(string username)
        {
            return _dao.LoadUser(username);
        }

        public User LoadUser(string username, string password)
        {
            return _dao.LoadUser(username, password);
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

        public List<Person> Find(Person person)
        {
            if (ActionPermitted(SystemAction.FindPerson))
            {
                return _dao.Find(person);
            }
            return null;
        }

        public List<User> Find(User user)
        {
            if (ActionPermitted(SystemAction.FindUser))
            {
                return _dao.Find(user);
            }
            return null;
        }

        public List<VoterCard> Find(VoterCard voterCard)
        {
            if (ActionPermitted(SystemAction.FindVoterCard))
            {
                return _dao.Find(voterCard);
            }
            return null;
        }

        public List<Citizen> FindElegibleVoters()
        {
            if (ActionPermitted(SystemAction.FindElegibleVoters))
            {
                return _dao.FindElegibleVoters();
            }
            return null;
        }

        public IEnumerable<RawPerson> LoadRawPeople()
        {
            throw new System.NotImplementedException();
        }

        public void Save(Person person)
        {
            if (ActionPermitted(SystemAction.SavePerson))
            {
                _dao.Save(person);
            }
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

        public void SetHasVoted(Citizen citizen, int cprKey)
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
                _dao.Save(citizen);
            }
        }

        public void ChangePassword(User user, string newPasswordHash, string oldPasswordHash)
        {
            if (ActionPermittedForThisUser(user, SystemAction.ChangeOwnPassword))
            {

                _dao.ChangePassword(user, newPasswordHash, oldPasswordHash);

                if (ActionPermitted(SystemAction.ChangeOwnPassword))
                {
                    _dao.ChangePassword(user, newPasswordHash, oldPasswordHash);
                }

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

        public void MarkVoterCardInvalid(VoterCard voterCard)
        {
            if (ActionPermitted(SystemAction.MarkVoteCardInvalid))
            {
                _dao.MarkVoterCardInvalid(voterCard);
            }
        }

        public void MarkPeopleNotInRawDataUneligibleToVote()
        {
            if (ActionPermitted(SystemAction.MarkPeopleNotInRawDataUneligibleToVote))
            {
                _dao.MarkPeopleNotInRawDataUneligibleToVote();
            }
        }

        public void UpdatePeople(Func<Person, RawPerson, Person> update)
        {
            if (ActionPermitted(SystemAction.UpdatePeople))
            {
                _dao.UpdatePeople(update);
            }
        }

        public VotingVenue FindVotingVenue(Citizen citizen)
        {
            if (ActionPermitted(SystemAction.FindVotingVenue))
            {
                return _dao.FindVotingVenue(citizen);
            }
            return null;
        }
    }
}
