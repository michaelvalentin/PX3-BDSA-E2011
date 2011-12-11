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
        private readonly VoterRegistrationView _view;
        private int _cprTries = 0;

        public NormalVoterRegistrationController(VoterRegistrationView view)
            : base(view)
        {
            _view = view;
            _view.Height = 314;

            Disable(_view.VoterIdentification.VoterCprBirthday);
            _view.VoterIdentification.VoterCprBirthday.Text = "XXXXXX";
            Disable(_view.RegisterVoterButton);
            _view.SearchVoterButton.Visibility = Visibility.Hidden;

            _view.VoterValidation.Children.Clear();
            _view.VoterValidation.Children.Add(new SecurityQuesitonView());

            _view.VoterIdentification.VoterCprDigits.PasswordChanged += CheckCpr;

            base.CitizenChanged += LoadVoterValidation;
            base.CitizenChanged += () =>
                                       {
                                           _view.VoterIdentification.VoterCprDigits.Password = "";
                                           _cprTries = 0;
                                       };
        }

        private void CheckCpr(object sender, EventArgs e)
        {
            _view.VoterIdentification.CprSuccessImage.Visibility = Visibility.Hidden;
            string cprDigits = _view.VoterIdentification.VoterCprDigits.Password;

            if (cprDigits.Length != 4 || Citizen == null) { return; }

            string checkCprDigits = Citizen.Cpr.Substring(6, 4);
            if (cprDigits.Equals(checkCprDigits) && _cprTries < 3)
            {
                ClearStatusMessage();
                _cprTries = -1;
                _view.VoterIdentification.CprSuccessImage.Visibility = Visibility.Visible;
            }
            else
            {
                ShowWarning("The last four digits of the cpr number are wrong. Try again");
                _view.VoterIdentification.VoterCprDigits.Password = "";
            }

            _cprTries++;

            if (_cprTries > 2)
            {
                ShowError("The maximum number of tries exceeded. Go to manual validation");
            }
        }

        private void LoadVoterValidation()
        {
            _view.VoterValidation.Children.Clear();
            var questionView = new SecurityQuesitonView();
            _view.VoterValidation.Children.Add(questionView);
            if (Citizen != null)
            {
                new RandomQuestionController(questionView, Citizen);
            }
        }

        protected override void RegisterVoter(object sender, EventArgs e)
        {
            if (Citizen != null)
            {
                try
                {
                    IDataAccessObject dao = DAOFactory.CurrentUserDAO;
                    if (Citizen.HasVoted)
                    {
                        ShowError("Voter has allready voted!");
                        return;
                    }
                    if (!Citizen.EligibleToVote)
                    {
                        ShowError("Citizen is not eligible to vote!");
                        return;
                    }
                    string cprDigits = _view.VoterIdentification.VoterCprDigits.Password;
                    if (!Citizen.Cpr.Substring(6, 4).Equals(cprDigits))
                    {
                        ShowError("CPR-Digits are incorrect!");
                        return;
                    }
                    DAOFactory.CurrentUserDAO.SetHasVoted(Citizen, cprDigits);
                    ShowSuccess("Citizen registered!");
                }
                catch (Exception ex)
                {
                    //TODO: Log the exception for security / maintainance...
                    ShowError("An unexpected error occured. Please try again.");
                }
            }
            else
            {
                ShowWarning("No person found with the inserted information");
            }
        }
    }
}