namespace DigitalVoterList.Election
{

    /// <summary>
    /// A specific venue for voting, typically a school or similair public place.
    /// </summary>
    public class VotingVenue
    {
        private string _address;
        private string _name;

        public VotingVenue(string name, string address)
        {
            _address = address;
            _name = name;
        }

        public string Address { get; private set; }

        public string Name { get; private set; }
    }
}
