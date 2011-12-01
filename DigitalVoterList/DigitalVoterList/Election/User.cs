using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DigitalVoterList.Election
{

    /// <summary>
    /// A person responsible of helping out at an election
    /// </summary>
    public class User : Person
    {
        private string _title;
        private HashSet<Action> _permissions;
        private DateTime? _lastSuccessfullValidationTime;

        /// <summary>
        /// The users at the election venue and people adminitrating the electing, who have different priviledges.
        /// </summary>
        /// <param name="id">The database id of the user</param>
        public User(int id)
            : base(id)
        {
            _title = "";
            _permissions = new HashSet<Action>();
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
            //todo: Make validation..!
            _lastSuccessfullValidationTime = null;
            return false;
        }

        public string Username { get; set; }

        /// <summary>
        /// The users jobtitle
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The users permission. Is an empty set if validation has expired, or has not been performed yet.
        /// </summary>
        public HashSet<Action> Permissions
        {
            get
            {
                Contract.Requires(_permissions != null);
                if (!Validated)
                {
                    return new HashSet<Action>();
                }
                else
                {
                    return new HashSet<Action>(_permissions);
                }
            }
        }

        /// <summary>
        /// Has the user got permission to perform this action?
        /// </summary>
        /// <param name="a">The action to check for permission</param>
        /// <returns>True if the user has the permission. False if not.</returns>
        public bool HasPermission(Action a)
        {
            return Validated && _permissions.Contains(a);
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
                    return true;
                }
            }
        }
    }
}
