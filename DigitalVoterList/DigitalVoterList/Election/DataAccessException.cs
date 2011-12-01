using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalVoterList.Election
{
    class DataAccessException : Exception
    {
        public DataAccessException(string msg) : base(msg){}
    }
}
