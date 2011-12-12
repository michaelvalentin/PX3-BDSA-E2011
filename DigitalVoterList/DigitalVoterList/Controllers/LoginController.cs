/*
 * Authors:
 * Team: PX3
 * Date: 12-12-2011
 */

using System;
using System.Windows.Media;
using DigitalVoterList.Election;
using DigitalVoterList.Views;

namespace DigitalVoterList.Controllers
{
    /// <summary>
    /// A controller for managing the login process
    /// </summary>
    public class LoginController
    {
        private LoginWindow _view;

        public LoginController(LoginWindow view)
        {
            _view = view;
            _view.LoginEvent += ValidateUser;
            _view.Show();
        }

        private void ValidateUser(Object sender, LoginEventArgs e)
        {
            _view.StatusText.Text = "";
            User u = User.GetUser(e.Username, e.Password);
            if (u == null)
            {
                _view.StatusText.Foreground = new SolidColorBrush(Color.FromRgb(225, 0, 0));
                _view.StatusText.Text = "Wrong username/password.";
            }
            else
            {
                _view.StatusText.Foreground = new SolidColorBrush(Color.FromRgb(0, 235, 0));
                _view.StatusText.Text = "Login was successfull. Loading the Digital Voter List.";
                VoterListApp.RunApp(u);
                _view.Close();
            }
        }
    }
}
