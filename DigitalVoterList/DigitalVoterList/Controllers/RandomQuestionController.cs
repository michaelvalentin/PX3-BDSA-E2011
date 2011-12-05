using System;
using System.Windows.Media;
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
        private Quiz[] _questions;
        private HashSet<Quiz> _used;
        private SecurityQuesitonView _view;

        public RandomQuestionController(SecurityQuesitonView view, Citizen voter)
        {
            _view = view;
            voter.SecurityQuestions.CopyTo(_questions);
            _used = new HashSet<Quiz>();
            RequestQuestion(null, null);
            _view.QuestionRequest += RequestQuestion;
        }

        public void RequestQuestion(object caller, EventArgs e)
        {
            if (_questions.Length == 0)
            {
                if (_questions.Length == 0) _view.StatusText.Text = "No security quesitons could be found for this citizen.";
                _view.StatusText.Foreground = new SolidColorBrush(Color.FromRgb(220, 0, 0));
                return;
            }
            Random r = new Random();
            Quiz q = _questions[r.Next(0, _questions.Length - 1)];
            if (_questions.Length <= _used.Count)
            {
                _view.StatusText.Text = "This question has already been showed.";
                _view.StatusText.Foreground = new SolidColorBrush(Color.FromRgb(220, 0, 0));
            }
            else
            {
                while (_used.Contains(q))
                {
                    q = _questions[r.Next(0, _questions.Length - 1)];
                }
            }
            _view.SetQuestion(q);
        }
    }
}
