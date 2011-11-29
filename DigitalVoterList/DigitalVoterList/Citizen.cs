using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace DigitalVoterList
{

    class Citizen : Person
    {
        private int _cpr;

        public Citizen(int cpr)
        {
            _cpr = cpr;
        }

        public int Cpr
        {
            get
            {
                return _cpr;
            }
            set
            {
                Contract.Ensures(_cpr.ToString().Length == 10);
                _cpr = value;
            }
        }

        private void ObjectInvariant()
        {
            Contract.Invariant(Cpr != null);
        }
    }
}
