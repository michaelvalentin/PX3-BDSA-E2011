using System.Windows;
using System.Windows.Media;

namespace DigitalVoterList.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            _username.Focus();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (_username.Text.Equals("mier"))
            {
                _statusText.Text = "Michael Valentin is awesome!";
                _statusText.Foreground = new SolidColorBrush(Color.FromRgb(0, 180, 0));
            }
            else if (_username.Text.Equals("mhyl"))
            {
                _statusText.Text = "Morten Hyllekilde is a monkey!";
                _statusText.Foreground = new SolidColorBrush(Color.FromRgb(230, 220, 0));
            }
            else
            {
                _statusText.Text = "Wrong username or password";
                _statusText.Foreground = new SolidColorBrush(Color.FromRgb(210, 0, 0));
            }
        }
    }
}
