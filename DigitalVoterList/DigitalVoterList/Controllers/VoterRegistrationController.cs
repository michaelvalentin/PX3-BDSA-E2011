using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DigitalVoterList.Election;
using DigitalVoterList.Views;

namespace DigitalVoterList.Controllers
{
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// A controller for handling the registration of voters
    /// </summary>
    public abstract class VoterRegistrationController : ContentController
    {
        private VoterRegistrationView _view;
        private VoterCard _voterCard;
        protected Citizen Citizen;
        private int cprTries;

        public VoterRegistrationController(VoterRegistrationView view)
        {
            _neededPermissions.Add(SystemAction.ScanVoterCard);
            _neededPermissions.Add(SystemAction.LoadPerson);
            _neededPermissions.Add(SystemAction.SetHasVoted);

            _view = view;
            View = _view;

            Disable(_view.VoterIdentification.VoterName);
            Disable(_view.VoterIdentification.VoterAddress);

            _view.StatusImageSucces.Visibility = Visibility.Hidden;
            _view.StatusImageError.Visibility = Visibility.Hidden;
            _view.StatusImageWarning.Visibility = Visibility.Hidden;

            _voterCard = null;
            cprTries = 0;

            _view.VoterIdentification.VoterCardNumber.TextChanged += VoterCardNumberChanged;
            _view.RegisterVoterButton.Click += RegisterVoterWrapper;
            _view.RegisterVoterButton.KeyDown += RegisterVoterWrapper;
        }

        protected VoterCard VoterCard
        {
            get
            {
                return _voterCard;
            }
            set
            {
                _voterCard = value;
            }
        }

        protected void Disable(TextBox t)
        {
            t.Background = new SolidColorBrush(Color.FromRgb(210, 210, 210));
            t.IsEnabled = false;
            t.IsTabStop = false;
        }

        protected void HideImages(object sender, KeyEventArgs e)
        {
            _view.StatusImageSucces.Visibility = Visibility.Hidden;
            _view.StatusImageError.Visibility = Visibility.Hidden;
            _view.StatusImageWarning.Visibility = Visibility.Hidden;
        }

        private void RegisterVoterWrapper(object sender, EventArgs e)
        {
            if (e is KeyEventArgs && ((KeyEventArgs)e).Key != Key.Enter) return;
            RegisterVoter(sender, e);
        }

        protected void CheckCpr(object sender, EventArgs e)
        {
            if (cprTries < 3)
            {
                if (_view.VoterIdentification.VoterCprDigits.Password.Length == 4)
                {
                    char[] digits = _view.VoterIdentification.VoterCprDigits.Password.ToCharArray();
                    if (digits.All(n => Char.IsDigit(n) == true))
                    {
                        _view.VoterIdentification.VoterCprDigits.Equals(VoterCard.Citizen.Cpr.Substring(5, 4));
                        _view.StatusImageSucces.Visibility = Visibility.Visible;
                        _view.StatusText.Text = "The four last digits in the cpr number is correct!";
                        cprTries = 0;
                    }
                    else
                    {
                        _view.StatusText.Text = "The last four digits of the cpr number is wrong. Try again";
                        _view.StatusImageWarning.Visibility = Visibility.Visible;
                        _view.VoterIdentification.Content = "";
                        cprTries++;
                    }
                }
            }
            else
            {
                _view.StatusText.Text = "The number of maximum tries exceeded. Go to manual validation";
                _view.StatusImageError.Visibility = Visibility.Visible;
            }
        }

        protected void RegisterVoter(object sender, EventArgs e)
        {
            if (VoterCard != null)
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
            }
        }

        private void VoterCardNumberChanged(object sender, EventArgs e)
        {
            TextBox voterCardNumberBox = (TextBox)sender;
            if (voterCardNumberBox.Text.Length == 8)
            {
                IDataAccessObject dao = DAOFactory.CurrentUserDAO;
                _voterCard = dao.LoadVoterCard(voterCardNumberBox.Text);
            }
            voterCardNumberBox.Text = voterCardNumberBox.Text.ToUpper();
            voterCardNumberBox.CaretIndex = 8;
            LoadVoterData();
        }

        protected void LoadVoterData()
        {
            if (_voterCard == null)
            {
                _view.VoterIdentification.VoterName.Text = "";
                _view.VoterIdentification.VoterAddress.Text = "";
                _view.VoterIdentification.VoterCprDigits.Password = "";
                LoadVoterValidation(null);
                Citizen = null;

            }
            else
            {
                _view.VoterIdentification.VoterName.Text = _voterCard.Citizen.Name;
                _view.VoterIdentification.VoterAddress.Text = _voterCard.Citizen.Address;
                Citizen = _voterCard.Citizen;
            }
            _view.StatusText.Text = "";
            LoadVoterValidation(Citizen);
        }

        protected void LoadVoterValidation(Citizen c)
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
