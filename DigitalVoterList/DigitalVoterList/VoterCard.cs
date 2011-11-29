using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace DigitalVoterList
{
    class VoterCard
    {
        private ElectionEvent _electionEvent;
        private Citizen _citizen;
        private string _id;

        public VoterCard(ElectionEvent electionEvent, Citizen citizen, string id)
        {
            _electionEvent = electionEvent;
            _citizen = citizen;
            _id = id;
        }

        public ElectionEvent ElectionEvent
        {
            get
            {
                return _electionEvent;
            }
        }

        public Citizen Citizen
        {
            get
            {
                return _citizen;
            }
        }

        public string Id
        {
            get
            {
                return _id;
            }
        }
    }
}
