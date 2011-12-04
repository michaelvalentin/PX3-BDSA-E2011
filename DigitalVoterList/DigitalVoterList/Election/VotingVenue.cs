namespace DigitalVoterList.Election
{

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
            _address = address;
            _name = name;
            _dbid = dbid;
        }

        public string Address { get; private set; }

        public string Name { get; private set; }

        public int DbId { get; private set; }

        public static bool operator ==(VotingVenue a, VotingVenue b)
        {
            a.DbId = b.DbId;
        }

        public static bool operator !=(VotingVenue a, VotingVenue b)
        {
            return !(a == b);
        }
    }
}
