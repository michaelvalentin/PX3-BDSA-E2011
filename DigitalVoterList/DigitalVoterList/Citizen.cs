// -----------------------------------------------------------------------
// <copyright file="Citizen.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
using System.Diagnostics.Contracts

namespace DigitalVoterList.Election
{

    /// <summary>
    /// A person with a valid and unique ID-number (CPR-number)
    /// </summary>
    public class Citizen : Person
    {
        private int _cpr;

        /// <summary>
        /// "A human being with a cpr-number"
        /// </summary>
        /// <param name="cpr"></param>
        public Citizen(int cpr)
        {
            _cpr = cpr;
        }

        /// <summary>
        /// "What is your CPR-number?"
        /// </summary>
        public int Cpr
        {
            get
            {
                return _cpr;
            }
        }

        private void ObjectInvariant()
        {
            Contract.Invariant(Cpr != null);
        }
    }
}
