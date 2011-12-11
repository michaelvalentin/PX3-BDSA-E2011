using System;
using DigitalVoterList.Election;
using DigitalVoterList.Views;

namespace DigitalVoterList.Controllers
{

    /// <summary>
    /// A controller for the manual registration project
    /// </summary>
    public class ManualVoterRegistrationController : VoterRegistrationController
    {
        private VoterRegistrationView _view;
        private SearchPersonController _searchController;
        private SearchPersonView _searchView;
        private SearchPersonWindow _searchWindow;

        public ManualVoterRegistrationController(VoterRegistrationView view)
            : base(view)
        {
            _view = view;
            _searchView = new SearchPersonView();
            _searchWindow = new SearchPersonWindow();
            _searchController = new SearchPersonController(_searchView, _searchWindow);

            _neededPermissions.Add(SystemAction.FindPerson);
            _neededPermissions.Add(SystemAction.SetHasVotedManually);

            _view.VoterValidation.Children.Clear();
            //_view.VoterValidation.Children.Add(new ManualVoterValidationView());

            _searchController.PersonFound += SearchPersonFound;
            _view.SearchVoterButton.Click += SearchEvent;
            //_view.VoterIdentification.VoterCprDigits.PasswordChanged += CheckCpr;
            _view.RegisterVoterButton.Click += RegisterVoter;
            //view.VoterIdentification.PreviewKeyDown += HideImages;
        }

        private void SearchEvent(object sender, EventArgs e)
        {
            _searchWindow.Show();
        }

        private void SearchPersonFound(object sender, SearchPersonEventArgs e)
        {
            Person p = e.Person;
            if (p != null)
            {
                _view.VoterIdentification.VoterName.Text = p.Name;
                _view.VoterIdentification.VoterCprBirthday.Text = p.Cpr.Substring(0, 6);
                _view.VoterIdentification.VoterAddress.Text = p.Address;
            }
            _searchWindow.Close();
        }

        protected override void LoadVoterValidation(Citizen c)
        {
            throw new NotImplementedException();
        }
    }
}
