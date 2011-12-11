using System.Windows;

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
            Closed += (s, e) => VoterListApp.App.Shutdown();
        }
    }
}
