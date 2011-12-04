using System;
using System.Windows;

namespace DigitalVoterList.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public event EventHandler<LoginEventArgs> LoginEvent;

        public LoginWindow()
        {
            InitializeComponent();
            _username.Focus();
        }

        private void RaiseLoginEvent()
        {
            StatusText.Text = "Trying to login";
            if (LoginEvent != null) LoginEvent.Invoke(this, new LoginEventArgs(_username.Text ?? "", _password.Password ?? ""));
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            RaiseLoginEvent();
        }
    }

    public class LoginEventArgs : EventArgs
    {
        public LoginEventArgs(string username, string password)
        {
            Password = password;
            Username = username;
        }
        public string Password { get; private set; }
        public string Username { get; private set; }
    }

}
