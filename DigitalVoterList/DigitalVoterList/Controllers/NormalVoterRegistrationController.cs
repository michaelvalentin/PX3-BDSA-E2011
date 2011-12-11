using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
        private int _cprTries;
        private Citizen _cprTrier;

        public NormalVoterRegistrationController(VoterRegistrationView view)
            : base(view)
        {
            _view = view;
            Disable(_view.VoterIdentification.VoterCprBirthday);
            _view.VoterIdentification.VoterCprBirthday.Text = "XXXXXX";

            _view.SearchVoterButton.Visibility = Visibility.Hidden;
            _cprTries = 0;

            _view.VoterValidation.Children.Clear();
            _view.VoterValidation.Children.Add(new SecurityQuesitonView());
            _view.Height = 314;


            _view.VoterIdentification.VoterCprDigits.LostFocus += CheckCpr;
            _view.VoterIdentification.VoterCprDigits.PasswordChanged += (o, e) =>
                                                                            {
                                                                                if (!((PasswordBox)o).Password.Equals(""))
                                                                                {
                                                                                    ClearStatusMessage();
                                                                                }
                                                                            };
            _view.RegisterVoterButton.Click += RegisterVoter;
            //_view.VoterIdentification.PreviewKeyDown += HideImages;
        }

        private void CheckCpr(object sender, EventArgs e)
        {
            if (_view.VoterIdentification.VoterCprDigits.Password.Length != 4 || Citizen == null) return;
            if (_cprTrier != null && _cprTrier.Equals(Citizen))
            {
                _cprTries = 0;
                _cprTrier = Citizen;
            }
            if (_cprTries < 3)
            {
                string cprDigits = _view.VoterIdentification.VoterCprDigits.Password;
                string checkCprDigits = Citizen.Cpr.Substring(6, 4);
                if (cprDigits.Equals(checkCprDigits))
                {
                    _view.StatusImageSucces.Visibility = Visibility.Visible;
                    _view.StatusText.Text = "The four last digits in the cpr number are correct!";
                    _view.StatusText.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    _cprTries = -1;
                }
                else if (_cprTries == 2)
                {
                    _view.StatusText.Text = "The maximum number of tries exceeded. Go to manual validation";
                    _view.StatusImageError.Visibility = Visibility.Visible;
                }
                else
                {
                    _view.StatusText.Text = "The last four digits of the cpr number are wrong. Try again";
                    _view.StatusImageWarning.Visibility = Visibility.Visible;
                    _view.VoterIdentification.VoterCprDigits.Password = "";
                }
                _cprTries++;
            }
            else
            {
                _view.StatusText.Text = "The maximum number of tries exceeded. Go to manual validation";
                _view.StatusImageError.Visibility = Visibility.Visible;
            }
        }


        protected override void RegisterVoter(object sender, EventArgs e)
        {
            throw new NotImplementedException();
            /*if (VoterCard != null)
            {
                try
                {
                    DAOFactory.CurrentUserDAO.SetHasVoted(this.Citizen, this._view.VoterIdentification.VoterCprDigits.Password);
                    _view.StatusImageSucces.Visibility = Visibility.Visible;
                    _view.StatusText.Text = "Citizen registered!";
                }
                catch (Exception ex)
                {
                    //TODO: throw ex;
                    _view.StatusImageError.Visibility = Visibility.Visible;
                    _view.StatusText.Text = ex.Message;
                }
            }
            else
            {
                _view.StatusText.Text = "No person found with the inserted information";
                _view.StatusImageWarning.Visibility = Visibility.Visible;
            }*/
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
    }
}
