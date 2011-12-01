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

        private bool ActionPermitted(Action a, string msg = "You don't have permission to perform this action.")
        {
            if (!_user.HasPermission(a))
            {
                throw new PermissionException(a, _user);
            }
            else
            {
                return true;
            }
        }

        public Person LoadPerson(int id)
        {
            if (ActionPermitted(Action.LoadPerson))
            {
                return _dao.LoadPerson(id);
            }
            return null;
        }

        public User LoadUser(string username, string password)
        {
            return _dao.LoadUser(username, password);
        }

        public User LoadUser(int id)
        {
            if (ActionPermitted(Action.LoadUser))
            {
                return _dao.LoadUser(id);
            }
            return null;
        }

        public VoterCard LoadVoterCard(int id)
        {
            if (ActionPermitted(Action.LoadVoterCard))
            {
                return _dao.LoadVoterCard(id);
            }
            return null;
        }

        public VoterCard LoadVoterCard(string idKey)
        {
            if (ActionPermitted(Action.ScanVoterCard))
            {
                return _dao.LoadVoterCard(idKey);
            }
            return null;
        }

        public List<Person> Find(Person person)
        {
            if (ActionPermitted(Action.FindPerson))
            {
                return _dao.Find(person);
            }
            return null;
        }

        public List<User> Find(User user)
        {
            if (ActionPermitted(Action.FindUser))
            {
                return _dao.Find(user);
            }
            return null;
        }

        public List<VoterCard> Find(VoterCard voterCard)
        {
            if (ActionPermitted(Action.FindVoterCard))
            {
                return _dao.Find(voterCard);
            }
            return null;
        }

        public List<Citizen> FindElegibleVoters()
        {
            if (ActionPermitted(Action.FindElegibleVoters))
            {
                return _dao.FindElegibleVoters();
            }
            return null;
        }

        public bool Save(Person person)
        {
            if (ActionPermitted(Action.SavePerson))
            {
                return _dao.Save(person);
            }
            return false;
        }

        public bool Save(User user)
        {
            if (ActionPermitted(Action.SaveUser))
            {
                return _dao.Save(user);
            }
            return false;
        }

        public bool Save(VoterCard voterCard)
        {
            if (ActionPermitted(Action.SaveVoterCard))
            {
                return _dao.Save(voterCard);
            }
            return false;
        }

        public bool SetHasVoted(Citizen citizen, int keyPhrase)
        {
            if (ActionPermitted(Action.SetHasVoted))
            {
                return _dao.SetHasVoted(citizen, keyPhrase);
            }
            return false;
        }

        public bool SetHasVoted(Citizen citizen)
        {
            if (ActionPermitted(Action.SetHasVotedManually))
            {
                return _dao.Save(citizen);
            }
            return false;
        }

        public bool ChangePassword(User user, string newPassword)
        {
            if (user.Equals(_user))
            {
                if (ActionPermitted(Action.ChangeOwnPassword))
                {
                    return _dao.ChangePassword(user, newPassword);
                }
                return false;
            }
            else
            {
                if (ActionPermitted(Action.ChangeOthersPassword))
                {
                    return _dao.ChangePassword(user, newPassword);
                }
                return false;
            }
        }

        public bool MarkUserInvalid(User user)
        {
            if (ActionPermitted(Action.MarkUserInvalid))
            {
                return _dao.MarkUserInvalid(user);
            }
            return false;
        }

        public bool RestoreUser(User user)
        {
            if (ActionPermitted(Action.RestoreUser))
            {
                return _dao.RestoreUser(user);
            }
            return false;
        }

        public bool MarkVoterCardInvalid(VoterCard voterCard)
        {
            if (ActionPermitted(Action.MarkVoteCardInvalid))
            {
                return _dao.Save(voterCard);
            }
            return false;
        }
    }
}