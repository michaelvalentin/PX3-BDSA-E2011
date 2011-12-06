using System.Windows;
using DigitalVoterList.Controllers;

namespace DigitalVoterList.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            VoterRegistrationView regView = new VoterRegistrationView();
            VoterRegistrationController regCont = new VoterRegistrationController(regView);
            MainContent.Children.Add(regView);
        }
    }
}
