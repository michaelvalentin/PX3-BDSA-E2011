using System;
using DigitalVoterList.Election;
using DigitalVoterList.Views;

namespace DigitalVoterList.Controllers
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// A controller for the manual registration project
    /// </summary>
    public class ManualVoterRegistrationController : VoterRegistrationController
    {
        private VoterRegistrationView _view;
        private SearchPersonController _searchController;
        private SearchPersonView _searchView;
        private SearchPersonWindow _searchWindow;
        private Citizen _citizen;

        public ManualVoterRegistrationController(VoterRegistrationView view)
            : base(view)
        {
            _view = view;
            _searchView = new SearchPersonView();
            _searchWindow = new SearchPersonWindow();
            _searchController = new SearchPersonController(_searchView, _searchWindow);
            _citizen = null;
            Disable(_view.RegisterVoterButton);

            _neededPermissions.Add(SystemAction.FindPerson);
            _neededPermissions.Add(SystemAction.SetHasVotedManually);

            _view.VoterValidation.Children.Clear();
            _view.VoterValidation.Children.Add(new ManualVoterValidationView());
            _view.Height = 420;

            _view.VoterIdentification.VoterCprDigits.PasswordChanged += CheckVoterValidationCpr;
            _view.VoterIdentification.VoterCprBirthday.TextChanged += FocusToCpr;
            _searchController.PersonFound += SearchPersonFound;
            _view.SearchVoterButton.Click += SearchEvent;
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
            _view.VoterValidation.Children.Clear();
            ManualVoterValidationView validationView = new ManualVoterValidationView();
            _view.VoterValidation.Children.Add(validationView);
            if (c != null)
            {
                ManualVoterValidationController mvc = new ManualVoterValidationController(validationView, c);
                mvc.Show();
            }
        }

        private void CheckVoterValidationCpr(object sender, RoutedEventArgs routedEventArgs)
        {
            string birthday = _view.VoterIdentification.VoterCprBirthday.Text;
            string digits = _view.VoterIdentification.VoterCprDigits.Password;
            _citizen = null;

            if (digits.Length == 4 && _view.VoterIdentification.VoterCardNumber.Text.Length == 0)
            {
                IDataAccessObject dao = DAOFactory.CurrentUserDAO;
                ManualVoterValidationView validationView = new ManualVoterValidationView();

                _view.VoterValidation.Children.Clear();
                _view.VoterValidation.Children.Add(validationView);

                _citizen = dao.LoadCitizen(birthday + digits);

                if (this._citizen != null)
                {
                    ManualVoterValidationController mvc = new ManualVoterValidationController(validationView, _citizen);
                    mvc.Show();
                    Enable(_view.RegisterVoterButton);
                    _view.RegisterVoterButton.Focus();
                }
                LoadVoterData();
            }
        }

        private void LoadVoterData()
        {
            if (_citizen == null)
            {
                _view.VoterIdentification.VoterName.Text = "";
                _view.VoterIdentification.VoterAddress.Text = "";
                _view.VoterIdentification.VoterCprDigits.Password = "";
                _citizen = null;
            }
            else
            {
                _view.VoterIdentification.VoterName.Text = _citizen.Name;
                _view.VoterIdentification.VoterAddress.Text = _citizen.Address;
            }
            ClearStatusMessage();
        }

        private void FocusToCpr(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            if (_view.VoterIdentification.VoterCprBirthday.Text.Length == 6)
            {
                _view.VoterIdentification.VoterCprDigits.Focus();

                //var validationController = new ManuVoterValidationController(validationView, c);
                //validationController.Show();

            }
        }
    }
}
