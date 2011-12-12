/*
 * Authors:
 * Team: PX3
 * Date: 12-12-2011
 */

using System;
using System.Windows.Input;
using System.Windows.Media;
using DigitalVoterList.Election;
using DigitalVoterList.Views;

namespace DigitalVoterList.Controllers
{

    /// <summary>
    /// A controller to handle change of password
    /// </summary>
    public class ChangeOwnPasswordController
    {
        private ChangePasswordWindow _view;

        public ChangeOwnPasswordController(ChangePasswordWindow view)
        {
            _view = view;
            _view.SaveBtn.Click += ChangePassword;
            _view.SaveBtn.KeyDown += ChangePassword;
        }

        private void ChangePassword(object sender, EventArgs e)
        {
            if (e is KeyEventArgs && ((KeyEventArgs)e).Key != Key.Enter) return;
            if (User.GetUser(VoterListApp.CurrentUser.Username, _view.OldPassword.Password) == null)
            {
                _view.StatusText.Text = "Wrong old password";
                _view.StatusText.Foreground = new SolidColorBrush(Color.FromRgb(215, 0, 0));
                return;
            }
            if (!_view.NewPassword.Password.Equals(_view.RepeatNewPassword.Password))
            {
                _view.StatusText.Text = "Different new passwords";
                _view.StatusText.Foreground = new SolidColorBrush(Color.FromRgb(215, 0, 0));
                return;
            }
            try
            {
                VoterListApp.CurrentUser.ChangePassword(_view.OldPassword.Password, _view.NewPassword.Password);
                _view.StatusText.Text = "Password changed.";
                _view.StatusText.Foreground = new SolidColorBrush(Color.FromRgb(0, 205, 0));
            }
            catch (Exception ex)
            {
                _view.StatusText.Text = "Unexpected error occured. Try again.";
                _view.StatusText.Foreground = new SolidColorBrush(Color.FromRgb(215, 0, 0));
            }
        }
    }
}
