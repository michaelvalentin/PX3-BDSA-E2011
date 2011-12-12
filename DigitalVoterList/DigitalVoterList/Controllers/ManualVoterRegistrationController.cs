using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Controls;
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
        private readonly VoterRegistrationView _view;
        private readonly SearchCitizenController _searchController;
        private readonly SearchCitizenView _searchView;
        private readonly SearchCitizenWindow _searchWindow;

        public ManualVoterRegistrationController(VoterRegistrationView view)
            : base(view)
        {
            _neededPermissions.Add(SystemAction.FindPerson);
            _neededPermissions.Add(SystemAction.SetHasVotedManually);

            _view = view;
            _searchView = new SearchCitizenView();
            _searchWindow = new SearchCitizenWindow();
            _searchController = new SearchCitizenController(_searchView, _searchWindow);

            _view.VoterValidation.Children.Clear();
            _view.VoterValidation.Children.Add(new ManualVoterValidationView());
            _view.Height = 420;

            _view.VoterIdentification.VoterCardNumber.TextChanged += (s, e) =>
                {
                    if (!((TextBox)s).Text.Equals(""))
                    {
                        _view.VoterIdentification.VoterCprBirthday.Text = "";
                        _view.VoterIdentification.VoterCprDigits.Password = "";
                    }
                };
            _view.VoterIdentification.VoterCprBirthday.TextChanged += (s, e) =>
                {
                    var t = (TextBox)s;
                    if (t.Text.Length == 6)
                    {
                        _view.VoterIdentification.VoterCprDigits.Password = "";
                        _view.VoterIdentification.VoterCprDigits.Focus();
                    }
                    if (!t.Text.Equals("")) _view.VoterIdentification.VoterCardNumber.Text = "";
                    CheckCpr();
                };
            _view.VoterIdentification.VoterCprBirthday.TextChanged += DigitsOnlyText;
            _view.VoterIdentification.VoterCprDigits.PasswordChanged += (s, e) =>
                {
                    if (!((PasswordBox)s).Password.Equals("")) _view.VoterIdentification.VoterCardNumber.Text = "";
                    CheckCpr();
                };
            _view.VoterIdentification.VoterCprDigits.PasswordChanged += DigitsOnlyPassword;

            _view.SearchVoterButton.Click += (s, e) => _searchWindow.Show();
            _searchController.CitizenFound += SearchPersonFound;

            CitizenChanged += LoadVoterValidation;
        }

        private void CheckCpr()
        {
            string cprDate = _view.VoterIdentification.VoterCprBirthday.Text;
            string cprDigits = _view.VoterIdentification.VoterCprDigits.Password;
            if (cprDate.Length != 6 || cprDigits.Length != 4)
            {
                Citizen = null;
                return;
            }
            var result = DAOFactory.CurrentUserDAO.FindCitizens(new Dictionary<CitizenSearchParam, object>()
                                                                        {
                                                                            {CitizenSearchParam.Cpr,cprDate + cprDigits}
                                                                        });
            if (result.Count == 0)
            {
                ShowWarning("No person found with the supplied CPR.");
                Citizen = null;
                return;
            }
            Citizen = result[0];
        }

        private void DigitsOnlyPassword(object sender, EventArgs e)
        {
            var p = (PasswordBox)sender;
            string input = p.Password;
            string digits = Regex.Replace(input, "[^0-9]", "");
            if (!input.Equals(digits))
            {
                p.Password = digits;
            }
        }

        private void DigitsOnlyText(object sender, EventArgs e)
        {
            var t = (TextBox)sender;
            int i = t.CaretIndex - 1;
            string input = t.Text;
            string digits = Regex.Replace(input, "[^0-9]", "");
            if (!input.Equals(digits))
            {
                t.Text = digits;
                t.CaretIndex = i;
            }
        }

        private void SearchPersonFound(Citizen c)
        {
            //Citizen = c;
            _searchWindow.Close();
        }

        protected void LoadVoterValidation()
        {
            _view.VoterValidation.Children.Clear();
            var validationView = new ManualVoterValidationView();
            _view.VoterValidation.Children.Add(validationView);
            if (Citizen != null)
            {
                var mvc = new ManualVoterValidationController(validationView, Citizen);
                //mvc.Show(); //TODO: Skal være default behaviour for controller..
            }
        }

        protected override void RegisterVoter(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
