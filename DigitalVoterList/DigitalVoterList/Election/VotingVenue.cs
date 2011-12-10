
namespace DigitalVoterList.Election
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// A specific venue for voting, typically a school or similair public place.
    /// </summary>
    public class VotingVenue
    {
        private string _address;
        private string _name;
        private int _dbid;

        public VotingVenue(int dbid, string name, string address)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Requires(!string.IsNullOrEmpty(address));
            _address = address;
            _name = name;
            _dbid = dbid;
        }

        public string Address { get; private set; }

        public string Name { get; private set; }

        public int DbId { get; private set; }

        public static bool operator ==(VotingVenue a, VotingVenue b) //todo: do this nicer
        {
            var c = a ?? new VotingVenue(0, null, null);
            var d = b ?? new VotingVenue(0, null, null);
            return c.DbId == d.DbId;
        }

        public static bool operator !=(VotingVenue a, VotingVenue b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <returns>
        /// true if the specified object is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object.</param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != typeof(VotingVenue))
            {
                return false;
            }
            return Equals((VotingVenue)obj);
        }
        
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_name != null);
            Contract.Invariant(_address != null);
        }

        public bool Equals(VotingVenue other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(other._address, this._address) && Equals(other._name, this._name) && other._dbid == this._dbid && Equals(other.Address, this.Address) && Equals(other.Name, this.Name) && other.DbId == this.DbId;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (this._address != null ? this._address.GetHashCode() : 0);
                result = (result * 397) ^ (this._name != null ? this._name.GetHashCode() : 0);
                result = (result * 397) ^ this._dbid;
                result = (result * 397) ^ (this.Address != null ? this.Address.GetHashCode() : 0);
                result = (result * 397) ^ (this.Name != null ? this.Name.GetHashCode() : 0);
                result = (result * 397) ^ this.DbId;
                return result;
            }
        }
    }
}
