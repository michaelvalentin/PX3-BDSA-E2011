using DigitalVoterList.Election;
using DigitalVoterList.Utilities;
using DigitalVoterList.Views;

namespace DigitalVoterList.Controllers
{
    using System.Collections.Generic;

    /// <summary>
    /// A controller for asking random security questions for a given citizen
    /// </summary>
    public class RandomQuestionController
    {
        private HashSet<Quiz> _questions;
        private HashSet<Quiz> _used;
        private SecurityQuesitonView _view;

        public RandomQuestionController(SecurityQuesitonView view, Citizen voter)
        {
            _view = view;
            _questions = voter.SecurityQuestions;
            _used = new HashSet<Quiz>();
        }
    }
}
