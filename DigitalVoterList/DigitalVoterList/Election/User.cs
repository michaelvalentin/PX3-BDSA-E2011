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
        private _userName;
        private HashSet<Permission> _permissions;
        private DateTime? _lastSuccessfullValidationTime;

        /// <summary>
        /// The users at the election venue and people adminitrating the electing, who have different priviledges.
        /// </summary>
        /// <param name="id">The database id of the user</param>
        public User(int id)
            : base(id)
        {
            _title = "";
            _permissions = new HashSet<Permission>();
            _lastSuccessfullValidationTime = null;
        }

        /// <summary>
        /// The users at the election venue and people adminitrating the electing, who have different priviledges.
        /// </summary>
        public User()
            : this(0)
        {
        }


        public bool FetchPermissions(string uname, string pwd)
        {
            //todo: Make validation..!
            _lastSuccessfullValidationTime = null;
            return false;
        }

        public string Title { get; set; }

        public string Username { get; set; }

        public Permission[] Permissions
        {
            get
            {
                Contract.Requires(_permissions != null);
                if (!Validated)
                {
                    return new Permission[0];
                }
                else
                {
                    Permission[] output = new Permission[_permissions.Count];
                    _permissions.CopyTo(output);
                    return output;
                }
            }
        }

        public bool HasPermission(Permission p)
        {
            return Validated && _permissions.Contains(p);
        }

        public bool Validated
        {
            get
            {
                if (_lastSuccessfullValidationTime == null) return false;

                DateTime now = new DateTime();
                if (now.Subtract((DateTime)_lastSuccessfullValidationTime).Minutes > 15)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        private bool CheckValidation()
        {
            if()
            DateTime now = new DateTime();
            if (now.Subtract(_lastSuccessfullValidationTime).Minutes > 15)
            {
                _validated = false;
                _permissions = new HashSet<Permission>();
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
