using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using System.Text;

namespace DigitalVoterList.Election
{

    /// <summary>
    /// A person responsible of helping out at an election
    /// </summary>
    public class User : Person
    {
        private HashSet<SystemAction> _permissions;
        private HashSet<VotingVenue> _workplaces;
        private DateTime? _lastSuccessfullValidationTime;

        /// <summary>
        /// What user has this login?
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>A validated user obejct, null if the login is not found.</returns>
        public static User GetUser(string username, string password)
        {
            IDataAccessObject dao = DAOFactory.CurrentUserDAO;
            User u = dao.LoadUser(username);
            if (u == null) return null;
            u.FetchPermissions(username, password);
            if (!u.Validated) return null;
            return u;
        }

        /// <summary>
        /// The users at the election venue and people adminitrating the electing, who have different priviledges.
        /// </summary>
        /// <param name="id">The database id of the user</param>
        public User(int id)
            : base(id)
        {
            _permissions = new HashSet<SystemAction>();
            _workplaces = new HashSet<VotingVenue>();
            _lastSuccessfullValidationTime = null;
        }

        /// <summary>
        /// The users at the election venue and people adminitrating the electing, who have different priviledges.
        /// </summary>
        public User()
            : this(0)
        {
        }

        /// <summary>
        /// Validate the user and load according permissions into the user object.
        /// </summary>
        /// <param name="uname">The username to validate</param>
        /// <param name="pwd">The password to validate</param>
        /// <returns>True on success. False otherwise.</returns>
        public bool FetchPermissions(string uname, string pwd)
        {
            IDataAccessObject dao = DAOFactory.getDAO(this);
            string pwdHash = HashPassword(pwd);
            Debug.WriteLine("PwdHash: " + pwdHash);
            Debug.WriteLine("UserSalt: " + UserSalt);
            if (dao.ValidateUser(uname, pwdHash))
            {
                _lastSuccessfullValidationTime = new DateTime();
                _permissions = dao.GetPermissions(this);
                _workplaces = dao.GetWorkplaces(this);
                return true;
            }
            return false;
        }

        /// <summary>
        /// The user's username
        /// </summary>
        public string Username { get; set; }

        public string UserSalt { get; set; }

        public bool Valid { get; set; }

        /// <summary>
        /// Changes the password of this user
        /// </summary>
        /// <param name="oldPwd">The old password</param>
        /// <param name="newPwd">The new password</param>
        /// <returns>Was it succesful?</returns>
        public void ChangePassword(string oldPwd, string newPwd)
        {
            IDataAccessObject dao = DAOFactory.getDAO(this);
            dao.ChangePassword(this, HashPassword(newPwd), HashPassword(oldPwd));
        }

        /// <summary>
        /// Changes the password of this user
        /// </summary>
        /// <param name="newPwd">The new password</param>
        /// <returns>Was it succesful?</returns>
        public void ChangePassword(string newPwd)
        {
            IDataAccessObject dao = DAOFactory.CurrentUserDAO;
            dao.ChangePassword(this, HashPassword(newPwd));
        }

        /// <summary>
        /// The user's id in the database
        /// </summary>
        public int DBId { get; private set; }

        /// <summary>
        /// The user's jobtitle
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The users permission. Is an empty set if validation has expired, or has not been performed yet.
        /// </summary>
        public HashSet<SystemAction> Permissions
        {
            get
            {
                Contract.Requires(_permissions != null);
                if (!Validated)
                {
                    return new HashSet<SystemAction>();
                }
                else
                {
                    if (_permissions != null)
                    {
                        return new HashSet<SystemAction>(_permissions);
                    }
                }
                return new HashSet<SystemAction>();
            }
        }

        /// <summary>
        /// The voting venue(s) where the user works.
        /// </summary>
        public HashSet<VotingVenue> Workplaces
        {
            get
            {
                Contract.Requires(_permissions != null);
                if (!Validated)
                {
                    return new HashSet<VotingVenue>();
                }
                else
                {
                    return new HashSet<VotingVenue>(_workplaces);
                }
            }
        }

        /// <summary>
        /// Has the user got permission to perform this SystemAction?
        /// </summary>
        /// <param name="a">The SystemAction to check for permission</param>
        /// <returns>True if the user has the permission. False if not.</returns>
        public bool HasPermission(SystemAction a)
        {
            return Validated && _permissions.Contains(a);
        }

        /// <summary>
        /// Checks if the user works at this specific voting venue
        /// </summary>
        /// <param name="v">The voting venue to check for</param>
        /// <returns>True if the user works here. False if not</returns>
        public bool WorksHere(VotingVenue v)
        {
            return Validated && _workplaces.Contains(v);
        }

        public bool Validated
        {
            get
            {
                DateTime now = new DateTime();
                if (_lastSuccessfullValidationTime == null || now.Subtract((DateTime)_lastSuccessfullValidationTime).Minutes > 15)
                {
                    return false;
                }
                else
                {
                    _lastSuccessfullValidationTime = new DateTime();
                    return true;
                }
            }
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <returns>
        /// true if the specified object is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object.</param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            return Equals(obj as User);
        }

        private string HashPassword(string password)
        {
            string salted = UserSalt + password + "AX7530G7FR";
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.UTF32.GetBytes(salted);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder output = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                output.Append(hash[i].ToString("X2"));
            }
            return output.ToString();
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(Username != null);
            Contract.Invariant(Title != null);
            Contract.Invariant(UserSalt != null);
        }

        public bool Equals(User other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return base.Equals(other) && Equals(other._permissions, this._permissions) && Equals(other._workplaces, this._workplaces) && other._lastSuccessfullValidationTime.Equals(this._lastSuccessfullValidationTime) && Equals(other.UserSalt, this.UserSalt) && other.Valid.Equals(this.Valid) && other.DBId == this.DBId;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = base.GetHashCode();
                result = (result * 397) ^ (this._permissions != null ? this._permissions.GetHashCode() : 0);
                result = (result * 397) ^ (this._workplaces != null ? this._workplaces.GetHashCode() : 0);
                result = (result * 397) ^ (this._lastSuccessfullValidationTime.HasValue ? this._lastSuccessfullValidationTime.Value.GetHashCode() : 0);
                result = (result * 397) ^ (this.UserSalt != null ? this.UserSalt.GetHashCode() : 0);
                result = (result * 397) ^ this.Valid.GetHashCode();
                result = (result * 397) ^ this.DBId;
                return result;
            }
        }
    }
}
