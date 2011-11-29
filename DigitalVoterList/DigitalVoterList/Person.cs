using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace DigitalVoterList
{
    class Person
    {
        private string _name;
        private string _address;
        private bool _eligibleVoter;
        private bool _ballotHanded;

        protected Person()
        {
        }

        public Person(string name, string address, bool eligibleVoter, bool ballotHanded)
        {
            _name = name;
            _address = address;
            _eligibleVoter = eligibleVoter;
            _ballotHanded = ballotHanded;
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public string Address
        {
            get
            {
                return _address;
            }
        }

        public bool EligibleVoter { get; set; }

        public bool BallotHanded { get; set; }
    }
}
