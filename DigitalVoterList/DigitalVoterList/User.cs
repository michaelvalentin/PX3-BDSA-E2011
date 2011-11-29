using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace DigitalVoterList
{
    class User : Person
    {
        private readonly string _title;
        private readonly string _username;
        private string _password;

        public User(string title, string username, string password)
        {
            Contract.Requires(!string.IsNullOrEmpty(title));
            Contract.Requires(!string.IsNullOrEmpty(username));
            Contract.Requires(!string.IsNullOrEmpty(password));
            _title = title;
            _username = username;
            _password = password;
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                Contract.Ensures(!string.IsNullOrEmpty(_password));
                _password = value;
            }
        }
        
        public string Title
        {
            get
            {
                return _title;
            }
        }

        public string Username
        {
            get
            {
                return _username;
            }
        }
    }
}
