namespace DigitalVoterList.Utilities
{

    /// <summary>
    /// A brief assessment used as an authenticator
    /// </summary>
    public class Quiz
    {
        /// <summary>
        /// "A brief assessment used as an authenticator"
        /// </summary>
        /// <param name="question"></param>
        /// <param name="answer"></param>
        public Quiz(string question, string answer)
        {
            Question = question;
            Answer = answer;
        }

        /// <summary>
        /// The quiz' question
        /// </summary>
        public string Question { get; private set; }

        /// <summary>
        /// The answer to the question
        /// </summary>
        public string Answer { get; private set; }
    }
}
