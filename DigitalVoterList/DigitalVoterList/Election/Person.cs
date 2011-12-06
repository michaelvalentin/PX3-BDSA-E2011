
namespace DigitalVoterList.Election
{

    /// <summary>
    /// A humanbeing with a name
    /// </summary>
    public class Person
    {
        /// <summary>
        /// A human being 
        /// </summary>
        /// <param name="id">A database id. Set to 0 to create new person.</param>
        public Person(int id)
        {
            DbId = id;
            Name = "";
            Address = "";
            PlaceOfBirth = "";
        }

        /// <summary>
        /// A human being
        /// </summary>
        public Person()
            : this(0)
        {

        }

        /// <summary>
        /// The persons CPR-number
        /// </summary>
        public string Cpr { get; set; }

        /// <summary>
        /// The persons passport number
        /// </summary>
        public string PassportNumber { get; set; }

        /// <summary>
        /// The persons full name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The persons address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Where this person was born
        /// </summary>
        public string PlaceOfBirth { get; set; }

        /// <summary>
        /// The database id of the person
        /// </summary>
        public int DbId { get; private set; }

        public new string ToString()
        {
            return "PERSON( navn : " + Name + " , cpr : " + Cpr + " )";
        }
    }
}
