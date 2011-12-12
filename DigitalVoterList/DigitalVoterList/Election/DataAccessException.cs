/*
 * Authors:
 * Team: PX3
 * Date: 12-12-2011
 */

using System;

namespace DigitalVoterList.Election
{
    class DataAccessException : Exception
    {
        public DataAccessException(string msg) : base(msg) { }
    }
}
