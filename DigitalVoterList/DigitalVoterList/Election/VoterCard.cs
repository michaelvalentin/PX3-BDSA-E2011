// -----------------------------------------------------------------------
// <copyright file="VoterCard.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DigitalVoterList.Election
{
    using System;

    /// <summary>
    /// A ticket to be exchanged for a ballot, that is send out to the voter as a part of the validation process.
    /// </summary>
    public class VoterCard
    {
        /// <summary>
        /// The ElectionEvent that the Voter Card is attached to.
        /// </summary>
        public ElectionEvent ElectionEvent { get; set; }

        /// <summary>
        /// The citizen, who is the owner of the Voter Card.
        /// </summary>
        public Citizen Citizen { get; set; }

        /// <summary>
        /// The database id of the Voter Card.
        /// </summary>
        public int Id {get; set; }

        /// <summary>
        /// The id-key, corresponding to the barcode on the physical votercard
        /// </summary>
        public string IdKey {
            get
            {
                return IdKey;
            }
            set
            {
                Contract.Requires(value != null);
                IdKey = value;
            } 
        }

        public bool MarkAsInvalid()
        {
            Valid = false;
            return true;
        }

        /// <summary>
        /// A getter and setter for _valid. This says if the Voter Card is valid or not.
        /// </summary>
        // TODO: Make setter..
        public bool Valid { get; private set; }

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
            if (obj.GetType() != typeof(VoterCard))
            {
                return false;
            }
            return Equals((VoterCard)obj);
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(ElectionEvent != null);
            Contract.Invariant(Citizen != null);
            Contract.Invariant(IdKey != null);
        }

        public bool Equals(VoterCard other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(other.ElectionEvent, this.ElectionEvent) && Equals(other.Citizen, this.Citizen) && other.Id == this.Id && other.Valid.Equals(this.Valid);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (this.ElectionEvent != null ? this.ElectionEvent.GetHashCode() : 0);
                result = (result * 397) ^ (this.Citizen != null ? this.Citizen.GetHashCode() : 0);
                result = (result * 397) ^ this.Id;
                result = (result * 397) ^ this.Valid.GetHashCode();
                return result;
            }
        }
        public bool Valid { get; set; }
    }
}
