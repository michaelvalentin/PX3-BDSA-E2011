using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DigitalVoterList.Election;
using DigitalVoterList.Views;

namespace DigitalVoterList.Controllers
{

    /// <summary>
    /// A controller for handling the registration of voters
    /// </summary>
    public abstract class VoterRegistrationController : ContentController
    {

        private readonly VoterRegistrationView _view;
        private Citizen _citizen;
        public Action CitizenChanged;
        public Citizen Citizen
        {
            get { return _citizen; }
            set
            {
                SetCitizen(value);
            }
        }

        protected VoterRegistrationController(VoterRegistrationView view)
        {
            _neededPermissions.Add(SystemAction.ScanVoterCard);
            _neededPermissions.Add(SystemAction.LoadPerson);
            _neededPermissions.Add(SystemAction.SetHasVoted);

            _view = view;
            View = _view;

            Disable(_view.VoterIdentification.VoterName);
            Disable(_view.VoterIdentification.VoterAddress);
            Disable(_view.RegisterVoterButton);

            _view.StatusImageSucces.Visibility = Visibility.Hidden;
            _view.StatusImageError.Visibility = Visibility.Hidden;
            _view.StatusImageWarning.Visibility = Visibility.Hidden;

            _view.VoterIdentification.VoterCardNumber.TextChanged += VoterCardNumberChanged;
            _view.RegisterVoterButton.Click += RegisterVoterWrapper;
            _view.RegisterVoterButton.KeyDown += RegisterVoterWrapper;
        }

        public void SetCitizen(Citizen c)
        {
            if (c == _citizen) return;
            if (c != null && c.Equals(_citizen)) return;
            _citizen = c;
            CitizenChanged.Invoke();
            CheckAbilityToVote();
            LoadVoterData();
        }

        private void CheckAbilityToVote()
        {
            if (Citizen != null)
            {
                if (!Citizen.EligibleToVote)
                {
                    ShowError("This citizen is not eligible to vote!");
                }
                if (Citizen.HasVoted)
                {
                    ShowError("This voter has already voted!");
                }
            }
        }


        private void RegisterVoterWrapper(object sender, EventArgs e)
        {
            if (e is KeyEventArgs && ((KeyEventArgs)e).Key != Key.Enter) return;
            RegisterVoter(sender, e);
        }

        protected abstract void RegisterVoter(object sender, EventArgs e);

        private void VoterCardNumberChanged(object sender, EventArgs e)
        {
            var voterCardNumberBox = (TextBox)sender;
            if (voterCardNumberBox.Text.Length == 8)
            {
                VoterCard voterCard = DAOFactory.CurrentUserDAO.LoadVoterCard(voterCardNumberBox.Text);
                if (voterCard != null && voterCard.Valid)
                {
                    Citizen = voterCard.Citizen;
                }
                else if (voterCard != null && !voterCard.Valid)
                {
                    ShowError("Voter card is invalid!");
                }
            }
            voterCardNumberBox.Text = voterCardNumberBox.Text.ToUpper();
            voterCardNumberBox.CaretIndex = 8;
        }

        private void LoadVoterData()
        {
            if (Citizen == null)
            {
                _view.VoterIdentification.VoterName.Text = "";
                _view.VoterIdentification.VoterAddress.Text = "";
                _view.VoterIdentification.VoterCprDigits.Password = "";
                Citizen = null;
            }
            else
            {
                _view.VoterIdentification.VoterName.Text = Citizen.Name;
                _view.VoterIdentification.VoterAddress.Text = Citizen.Address;
            }
            ClearStatusMessage();
        }

        protected void ClearStatusMessage()
        {
            _view.StatusText.Text = "";
            HideImages();
        }

        protected void Disable(TextBox t)
        {
            t.Background = new SolidColorBrush(Color.FromRgb(210, 210, 210));
            t.IsEnabled = false;
            t.IsTabStop = false;
        }

        protected void Disable(Button b)
        {
            b.Background = new SolidColorBrush(Color.FromRgb(210, 210, 210));
            b.IsEnabled = false;
            b.IsTabStop = false;
        }

        protected void Enable(Button b)
        {
            //TODO: Set standard button color
            //b.Background = new SolidColorBrush(Color.FromRgb(210, 210, 210));
            b.IsEnabled = true;
            b.IsTabStop = true;
        }

        protected void HideImages()
        {
            _view.StatusImageSucces.Visibility = Visibility.Hidden;
            _view.StatusImageError.Visibility = Visibility.Hidden;
            _view.StatusImageWarning.Visibility = Visibility.Hidden;
        }

        protected void ShowSuccess(string msg)
        {
            _view.StatusText.Text = msg;
            _view.StatusText.Foreground = new SolidColorBrush(Color.FromRgb(0, 140, 0));
            _view.StatusImageSucces.Visibility = Visibility.Visible;
            SetRegBtnState();
        }

        protected void ShowWarning(string msg)
        {
            _view.StatusText.Text = msg;
            _view.StatusText.Foreground = new SolidColorBrush(Color.FromRgb(190, 0, 0));
            _view.StatusImageWarning.Visibility = Visibility.Visible;
            SetRegBtnState();
        }

        protected void ShowError(string msg)
        {
            _view.StatusText.Text = msg;
            _view.StatusText.Foreground = new SolidColorBrush(Color.FromRgb(190, 0, 0));
            _view.StatusImageError.Visibility = Visibility.Visible;
            SetRegBtnState();
        }

        protected void SetRegBtnState()
        {
            Disable(_view.RegisterVoterButton);
            if (Citizen == null) return;
            if (Citizen.HasVoted) return;
            if (!Citizen.EligibleToVote) return;
            Enable(_view.RegisterVoterButton);
        }
    }
}
