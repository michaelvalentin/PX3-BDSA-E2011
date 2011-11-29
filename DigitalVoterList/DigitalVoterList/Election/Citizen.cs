// -----------------------------------------------------------------------
// <copyright file="Citizen.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
using System.Diagnostics.Contracts;

namespace DigitalVoterList.Election
{

    /// <summary>
    /// A person with a valid and unique ID-number (CPR-number)
    /// </summary>
    public class Citizen : Person
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
            Contract.Invariant(ValidCpr(_cpr));
        }

        private bool ValidCpr(int cpr)
        {
            //Todo: Write method! 
            return true;
        }
    }
}
