using DigitalVoterList.Election;
using DigitalVoterList.Views;

namespace DigitalVoterList.Controllers
{
    using System;

    /// <summary>
    /// A controller for handling the registration of voters
    /// </summary>
    public class VoterRegistrationController : ContentController
    {
        private VoterRegistrationView _view;
        private VoterCard _voterCard;

        public VoterRegistrationController(VoterRegistrationView view)
        {
            _neededPermissions.Add(SystemAction.ScanVoterCard);
            _neededPermissions.Add(SystemAction.LoadPerson);
            _neededPermissions.Add(SystemAction.SetHasVoted);

            _view = view;
            View = _view;
            _view.VoterCardNumber.TextChanged += VoterCardNumberChanged;
            _view.Cpr.LostFocus += CheckCpr;
            _view.RegisterVoterButton.Click += RegisterVoter;
        }

        private void VoterCardNumberChanged(object sender, EventArgs e)
        {
            _voterCard = null;
            if (_view.VoterCardNumber.Text.Length == 8)
            {
                IDataAccessObject dao = DAOFactory.CurrentUserDAO;
                _voterCard = dao.LoadVoterCard(_view.VoterCardNumber.Text);
            }
            if (_view.VoterCardNumber.Text.Length > 8)
            {
                _view.VoterCardNumber.Text = _view.VoterCardNumber.Text.Substring(0, 8);
            }
            _view.VoterCardNumber.Text = _view.VoterCardNumber.Text.ToUpper();
            _view.VoterCardNumber.CaretIndex = 8;
            LoadData();
        }

        private void LoadData()
        {
            if (_voterCard == null)
            {
                _view.VoterName.Text = "";
                _view.VoterAddress.Text = "";
                _view.SecurityQuestion = new SecurityQuesitonView();
            }
            else
            {
                _view.VoterName.Text = _voterCard.Citizen.Name;
                _view.VoterAddress.Text = _voterCard.Citizen.Address;
                _view.SecurityQuestion = new SecurityQuesitonView();
                new RandomQuestionController(_view.SecurityQuestion, _voterCard.Citizen);
            }
        }

        private void CheckCpr(object sender, EventArgs e)
        {
            // TODO: wirte.. :-)
        }

        private void RegisterVoter(object sender, EventArgs e)
        {

        }
    }
}
