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

        /// <summary>
        /// A getter and setter for _date. The date is the scheduled date of the ElectionEvent.
        /// </summary>
        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                Contract.Ensures(!value.Equals(null));
                _date = value;
            }
        }

        /// <summary>
        /// A getter and setter for _name. The name is the name of the election.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                Contract.Ensures(!string.IsNullOrEmpty(value));
                _name = value;
            }
        }

        /// <summary>
        /// What votingVenue should be used for this citizen
        /// </summary>
        /// <returns></returns>
        public VotingVenue VotingVenueForCitizen(Citizen citizen)
        {
            //return DAOFactory.CurrentUserDAO.FindVotingVenue(citizen);
            throw new NotImplementedException();
        }

    }
}
