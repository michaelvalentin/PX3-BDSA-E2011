using System.Windows;
using DigitalVoterList.Controllers;
using DigitalVoterList.Election;
using DigitalVoterList.Views;

namespace DigitalVoterList
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows.Documents;

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
            DAOMySql dao = new DAOMySql();
            VoterCard voterCard = dao.LoadVoterCard(1);
            Debug.WriteLine("owner: "+voterCard.Citizen);
            //VoterCardPrinter vcp = new VoterCardPrinter();
            //vcp.Print(voterCard);
            Debug.WriteLine("p1");
            Application app = new Application();
            app.Startup += (o, e) =>
            {
                RunApp(null);
            };
            app.Run();
        }

        public static void RunApp(User user)
        {
            if (user != null && user.Validated)
            {
            }
            else
            {
                //Show the login window
                LoginWindow view = new LoginWindow();
                new LoginController(view);
            }
        }
    }
}
