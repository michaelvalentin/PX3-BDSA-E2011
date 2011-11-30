using System;
using System.Diagnostics.Contracts;

namespace DigitalVoterList.Election
{

    /// <summary>
    /// "An event containing data of an election."
    /// </summary>
    public class ElectionEvent
    {
        private DateTime _date;
        private string _name;

        /// <summary>
        /// "Can I have a new election with this name and date?"
        /// </summary>
        /// <param name="date">The date of the election</param>
        /// <param name="name">The name of the election</param>
        public ElectionEvent(DateTime date, string name)
        {
            Contract.Requires(!date.Equals(null));
            Contract.Requires(!string.IsNullOrEmpty(name));
            _date = date;
            _name = name;
        }

        /// <summary>
        /// "The date of the election"
        /// </summary>
        public DateTime Date
        {
            //"When are you scheduled to be?"
            get
            {
                return _date;
            }
            //"Change your scheduled election date!"
            set
            {
                Contract.Ensures(!_date.Equals(null));
                _date = value;
            }
        }

        /// <summary>
        /// "The name of the election"
        /// </summary>
        public string Name
        {
            //"What is your name?"
            get
            {
                return _name;
            }
            // "Change your name!"
            set
            {
                Contract.Ensures(!string.IsNullOrEmpty(_name));
                _name = value;
            }
        }
    }
}
