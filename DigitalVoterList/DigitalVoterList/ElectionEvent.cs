using System;
using System.Diagnostics.Contracts;

namespace DigitalVoterList.Election
{

    /// <summary>
    /// An actual election, that runs at a specific time, in a specific area and with a specific set of elegible voters.
    /// </summary>
    public class ElectionEvent
    {
        private DateTime _date;
        private string _name;

        public ElectionEvent(DateTime date, string name)
        {
            Contract.Requires(!date.Equals(null));
            Contract.Requires(!string.IsNullOrEmpty(name));
            _date = date;
            _name = name;
        }

        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                Contract.Ensures(!_date.Equals(null));
                _date = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                Contract.Ensures(!string.IsNullOrEmpty(_name));
                _name = value;
            }
        }
    }
}
