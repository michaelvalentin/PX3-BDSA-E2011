// -----------------------------------------------------------------------
// <copyright file="VoterCard.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DigitalVoterList.Election
{

    /// <summary>
    ///     "The voter card being sent to citizens who are eligibe to vote."
    /// </summary>
    public class VoterCard
    {
        private readonly ElectionEvent _electionEvent;
        private readonly Citizen _citizen;
        private readonly string _id;

        /// <summary>
        /// "Can I have a new voter card for this citizen?"
        /// </summary>
        /// <param name="electionEvent"></param>
        /// <param name="citizen"></param>
        /// <param name="id"></param>
        public VoterCard(Citizen citizen)
        {
            _citizen = citizen;

            //Get election event

            //Generate id

        }

        /// <summary>
        /// "Which election event have you been made for?"
        /// </summary>
        public ElectionEvent ElectionEvent
        {
            get
            {
                return _electionEvent;
            }
        }

        /// <summary>
        /// "Which election event have you been made for?"
        /// </summary>
        public Citizen Citizen
        {
            get
            {
                return _citizen;
            }
        }

        /// <summary>
        /// "What is your ID?"
        /// </summary>
        public string Id
        {
            get
            {
                return _id;
            }
        }
    }
}
