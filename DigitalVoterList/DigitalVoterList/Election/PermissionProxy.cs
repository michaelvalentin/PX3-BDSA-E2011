using System.Collections.Generic;

namespace DigitalVoterList.Election
{

    /// <summary>
    /// A proxy to handle permissions for data access actions
    /// </summary>
    // TODO: Consider voting-venues?
    public class PermissionProxy : IDataAccessObject
    {
        private readonly User _user;
        private readonly IDataAccessObject _dao;

        public PermissionProxy(User u, IDataAccessObject dao)
        {
            _user = u;
            _dao = dao;
        }

        private bool ActionPermitted(SystemAction a, string msg = "You don't have permission to perform this SystemAction.")
        {
            if (!_user.HasPermission(a))
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

        public bool Save(Person person)
        {
            if (ActionPermitted(SystemAction.SavePerson))
            {
                return _dao.Save(person);
            }
            return false;
        }

        public bool Save(User user)
        {
            if (ActionPermitted(SystemAction.SaveUser))
            {
                return _dao.Save(user);
            }
            return false;
        }

        public bool Save(VoterCard voterCard)
        {
            if (ActionPermitted(SystemAction.SaveVoterCard))
            {
                return _dao.Save(voterCard);
            }
            return false;
        }

        public bool SetHasVoted(Citizen citizen, int keyPhrase)
        {
            if (ActionPermitted(SystemAction.SetHasVoted) && WorksHere(citizen.VotingPlace))
            {
                return _dao.SetHasVoted(citizen, keyPhrase);
            }
            return false;
        }

        public bool SetHasVoted(Citizen citizen)
        {
            if (ActionPermitted(SystemAction.SetHasVotedManually))
            {
                return _dao.Save(citizen);
            }
            return false;
        }

        public bool ChangePassword(User user, string newPassword, string oldPassword)
        {
            if (user.Equals(_user))
            {
                if (ActionPermitted(SystemAction.ChangeOwnPassword))
                {
                    return _dao.ChangePassword(user, newPassword, oldPassword);
                }
            }
            return false;
        }

        public bool ChangePassword(User user, string newPassword)
        {
            if (ActionPermitted(SystemAction.ChangeOthersPassword))
            {
                return _dao.ChangePassword(user, newPassword);
            }
            return false;
        }

        public bool MarkUserInvalid(User user)
        {
            if (ActionPermitted(SystemAction.MarkUserInvalid))
            {
                return _dao.MarkUserInvalid(user);
            }
            return false;
        }

        public bool RestoreUser(User user)
        {
            if (ActionPermitted(SystemAction.RestoreUser))
            {
                return _dao.RestoreUser(user);
            }
            return false;
        }

        public bool MarkVoterCardInvalid(VoterCard voterCard)
        {
            if (ActionPermitted(SystemAction.MarkVoteCardInvalid))
            {
                return _dao.Save(voterCard);
            }
            return false;
        }
    }
}