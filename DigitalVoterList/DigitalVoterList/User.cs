// -----------------------------------------------------------------------
// <copyright file="User.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
using System.Diagnostics.Contracts;

namespace DigitalVoterList.Election
{

    /// <summary>
    /// A person responsible of helping out at an election
    /// </summary>
    public class User : Person
    {
        private readonly string _title;
        private readonly string _username;
        private string _password;

        /// <summary>
        /// "The users at the election venue and people adminitrating the electing, whom have different priviledges."
        /// </summary>
        /// <param name="title"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public User(string title, string username, string password)
        {
            Contract.Requires(!string.IsNullOrEmpty(title));
            Contract.Requires(!string.IsNullOrEmpty(username));
            Contract.Requires(!string.IsNullOrEmpty(password));
            _title = title;
            _username = username;
            _password = password;
        }

        /// <summary>
        /// "The password of the user"
        /// </summary>
        public string Password
        {
            // "What is your password?"
            get
            {
                return _password;
            }
            // "Change your password!"
            set
            {
                Contract.Ensures(!string.IsNullOrEmpty(_password));
                _password = value;
            }
        }

        /// <summary>
        /// "What is your title?"
        /// </summary>
        public string Title
        {
            get
            {
                return _title;
            }
        }

        /// <summary>
        /// "What is your username?"
        /// </summary>
        public string Username
        {
            get
            {
                return _username;
            }
        }
    }
}
