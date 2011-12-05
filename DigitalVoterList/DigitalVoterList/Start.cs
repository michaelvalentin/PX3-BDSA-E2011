using System.Diagnostics;
using System.Windows;
using DigitalVoterList.Election;
using DigitalVoterList.Views;

namespace DigitalVoterList
{
    using DigitalVoterList.Election.Administration;

    /// <summary>
    /// The main class for initializing the application
    /// </summary>
    public class Start
    {
        /// <summary>
        /// Start the application
        /// </summary>
        [System.STAThread]
        public static void Main()
        {
            Citizen c = new Citizen(1,14492819);
            VoterCard vc = new VoterCard(Settings.Election, c);
            VoterCardPrinter vcp = new VoterCardPrinter();
            vcp.Print(vc);
            Debug.WriteLine("JEG ER HER!!");
            Application app = new Application();
            app.Startup += (object sender, StartupEventArgs e) =>
            {
                RunApp(null);
            };
            app.Run();
        }

        public static void RunApp(User user)
        {
            if (user != null && user.Validated)
            {
                Debug.WriteLine("TEST");
            }
            else
            {
                //Show the login window
                LoginWindow view = new LoginWindow();
                new Controllers.LoginController(view);
                view.Show();
            }
        }
    }
}
