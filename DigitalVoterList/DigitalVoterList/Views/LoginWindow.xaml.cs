using System;
using System.Windows;
using System.Windows.Input;

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
            _username.KeyDown += EnterClicked;
            _password.KeyDown += EnterClicked;
            LoginBtn.KeyDown += EnterClicked;
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

        private void EnterClicked(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) RaiseLoginEvent();
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
