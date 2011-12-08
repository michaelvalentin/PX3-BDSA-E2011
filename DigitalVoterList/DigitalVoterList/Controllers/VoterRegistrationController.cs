using System;
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
        private VoterRegistrationView _view;
        private VoterCard _voterCard;
        protected Citizen Citizen;

        public VoterRegistrationController(VoterRegistrationView view)
        {
            _neededPermissions.Add(SystemAction.ScanVoterCard);
            _neededPermissions.Add(SystemAction.LoadPerson);
            _neededPermissions.Add(SystemAction.SetHasVoted);

            _view = view;
            View = _view;

            Disable(_view.VoterIdentification.VoterName);
            Disable(_view.VoterIdentification.VoterAddress);

            _view.VoterIdentification.VoterCardNumber.TextChanged += VoterCardNumberChanged;
            _view.VoterIdentification.VoterCprDigits.LostFocus += CheckCpr;
            _view.RegisterVoterButton.Click += RegisterVoterWrapper;
            _view.RegisterVoterButton.KeyDown += RegisterVoterWrapper;
        }

        protected void Disable(TextBox t)
        {
            t.Background = new SolidColorBrush(Color.FromRgb(210, 210, 210));
            t.IsEnabled = false;
            t.IsTabStop = false;
        }

        private void VoterCardNumberChanged(object sender, EventArgs e)
        {
            TextBox voterCardNumberBox = (TextBox)sender;
            _voterCard = null;
            if (voterCardNumberBox.Text.Length == 8)
            {
                IDataAccessObject dao = DAOFactory.CurrentUserDAO;
                _voterCard = dao.LoadVoterCard(voterCardNumberBox.Text);
            }
            if (voterCardNumberBox.Text.Length > 8)
            {
                voterCardNumberBox.Text = voterCardNumberBox.Text.Substring(0, 8);
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
                SecurityQuesitonView questionView = new SecurityQuesitonView();
                Citizen = _voterCard.Citizen;
            }
            _view.StatusText.Text = "";
            LoadVoterValidation(Citizen);
        }

        private void RegisterVoterWrapper(object sender, EventArgs e)
        {
            if (e is KeyEventArgs && ((KeyEventArgs)e).Key != Key.Enter) return;
            RegisterVoter(sender, e);
        }

        protected abstract void LoadVoterValidation(Citizen c);

        protected abstract void CheckCpr(object sender, EventArgs e);

        protected abstract void RegisterVoter(object sender, EventArgs e);
    }
}
