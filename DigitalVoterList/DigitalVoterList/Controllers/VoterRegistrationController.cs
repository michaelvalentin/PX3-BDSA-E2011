using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DigitalVoterList.Election;
using DigitalVoterList.Views;
using System.Windows;
using DigitalVoterList.Utilities;

namespace DigitalVoterList.Controllers
{

    /// <summary>
    /// A controller for handling the registration of voters
    /// </summary>
    public abstract class VoterRegistrationController : ContentController
    {

        private VoterRegistrationView _view;
        private Citizen _citizen;
        public Citizen Citizen
        {
            get { return _citizen; }
            set
            {
                if (_citizen != value)
                {
                    _citizen = value;
                    OnCitizenChanged();
                }
            }
        }
        public event EventHandler CitizenChanged;

        private void OnCitizenChanged()
        {
            if (CitizenChanged != null)
            {
                CitizenChanged.Invoke(this, new EventArgs());
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

            _view.StatusImageSucces.Visibility = Visibility.Hidden;
            _view.StatusImageError.Visibility = Visibility.Hidden;
            _view.StatusImageWarning.Visibility = Visibility.Hidden;

            _view.VoterIdentification.VoterCardNumber.TextChanged += VoterCardNumberChanged;
            _view.RegisterVoterButton.Click += RegisterVoterWrapper;
            _view.RegisterVoterButton.KeyDown += RegisterVoterWrapper;
            _view.RegisterVoterButton.Click += RegisterVoter;
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

        private void RegisterVoterWrapper(object sender, EventArgs e)
        {
            if (e is KeyEventArgs && ((KeyEventArgs)e).Key != Key.Enter) return;
            RegisterVoter(sender, e);
        }

        protected void RegisterVoter(object sender, EventArgs e)
        {
            if (Citizen != null)
            {
                try
                {
                    //TODO: TEST IF REGISTERED
                    DAOFactory.CurrentUserDAO.SetHasVoted(Citizen, _view.VoterIdentification.VoterCprDigits.Password);
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
            Citizen = null;
            if (voterCardNumberBox.Text.Length == 8)
            {
                IDataAccessObject dao = DAOFactory.CurrentUserDAO;
                VoterCard voterCard = dao.LoadVoterCard(voterCardNumberBox.Text);
                if (voterCard != null)
                {
                    Citizen = voterCard.Citizen;
                    _view.VoterIdentification.VoterCprDigits.Focus();
                }
            }
            voterCardNumberBox.Text = voterCardNumberBox.Text.ToUpper();
            voterCardNumberBox.CaretIndex = 8;
            LoadVoterData();
        }

        private void LoadVoterData()
        {
            if (Citizen == null)
            {
                _view.VoterIdentification.VoterName.Text = "";
                _view.VoterIdentification.VoterAddress.Text = "";
                _view.VoterIdentification.VoterCprDigits.Password = "";
                LoadVoterValidation(null);
                Citizen = null;
            }
            else
            {
                _view.VoterIdentification.VoterName.Text = Citizen.Name;
                _view.VoterIdentification.VoterAddress.Text = Citizen.Address;
            }
            ClearStatusMessage();
            LoadVoterValidation(Citizen);
        }

        protected void ClearStatusMessage()
        {
            _view.StatusText.Text = "";
            HideImages();
        }

        protected abstract void LoadVoterValidation(Citizen c);
    }
}
