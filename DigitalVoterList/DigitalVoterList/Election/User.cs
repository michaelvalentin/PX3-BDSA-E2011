﻿using System;
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
        private HashSet<SystemAction> _permissions;
        private HashSet<VotingVenue> _workplaces;
        private DateTime? _lastSuccessfullValidationTime;

        /// <summary>
        /// The users at the election venue and people adminitrating the electing, who have different priviledges.
        /// </summary>
        /// <param name="id">The database id of the user</param>
        public User(int id)
            : base(id)
        {
            _title = "";
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
            if (dao.ValidateUser(uname, pwdHash) != 0)
            {
                _lastSuccessfullValidationTime = new DateTime();
                _permissions = dao.GetPermissions(this);
            }
            return false;
        }

        public string Username { get; set; }

        public bool CangePassword(string oldPwd, string newPwd)
        {
            IDataAccessObject dao = DAOFactory.getDAO(this);
        }

        public int dBId { get; private set; }

        /// <summary>
        /// The users jobtitle
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
                    return new HashSet<SystemAction>(_permissions);
                }
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

        public new string ToString()
        {
            return "USER( username : " + Username + " , title : " + Title + " )";
        }

        private string HashPassword(string password)
        {
            return password;
        }
    }
}
