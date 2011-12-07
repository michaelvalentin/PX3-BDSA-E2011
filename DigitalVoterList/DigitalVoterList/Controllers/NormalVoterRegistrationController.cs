// -----------------------------------------------------------------------
// <copyright file="NormalVoterRegistrationController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Windows;
using DigitalVoterList.Election;
using DigitalVoterList.Views;

namespace DigitalVoterList.Controllers
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class NormalVoterRegistrationController : VoterRegistrationController
    {
        private VoterRegistrationView _view;

        public NormalVoterRegistrationController(VoterRegistrationView view)
            : base(view)
        {
            _view = view;
            Disable(_view.VoterIdentification.VoterCprBirthday);
            _view.VoterIdentification.VoterCprBirthday.Text = "XXXXXX";

            _view.SearchVoterButton.Visibility = Visibility.Hidden;

            _view.VoterValidation.Children.Add(new SecurityQuesitonView());
            _view.Height = 305;
        }

        protected override void LoadVoterValidation(Citizen c)
        {
            _view.VoterValidation.Children.Clear();
            SecurityQuesitonView questionView = new SecurityQuesitonView();
            _view.VoterValidation.Children.Add(questionView);
            if (c != null)
            {
                new RandomQuestionController(questionView, c);
            }
        }

        protected override void CheckCpr(object sender, EventArgs e)
        {
            //TODO: Write??
        }

        protected override void RegisterVoter(object sender, EventArgs e)
        {
            //TODO: Make sure we meet pre-conditions..
            try
            {
                DAOFactory.CurrentUserDAO.SetHasVoted(_citizen);
                //_view.StatusImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/DigitalVoterList;component/Resources/Icons/success.png"));
            }
            catch (Exception ex)
            {
                //_view.StatusImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/DigitalVoterList;component/Resources/Icons/error.png"));
                _view.StatusText.Text = ex.Message;
            }
        }
    }
}
