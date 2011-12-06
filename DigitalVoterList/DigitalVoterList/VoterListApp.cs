using System.Windows;
using DigitalVoterList.Controllers;
using DigitalVoterList.Election;
using DigitalVoterList.Views;

namespace DigitalVoterList
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows.Controls;
    using System.Windows.Documents;

    /// <summary>
    /// The main class for initializing the application
    /// </summary>
    public class VoterListApp
    {
        private static User _currentUser;
        public static Application App;

        public static User CurrentUser
        {
            get { return _currentUser ?? new User(); }
            set { _currentUser = value; }
        }

        /// <summary>
        /// DigitalVoterList the application
        /// </summary>
        [System.STAThread]
        public static void Main()
        {
            //DAOMySql dao = new DAOMySql();
            //Citizen c = (Citizen)dao.LoadPerson(1);
            //VoterCard vc = new VoterCard(Settings.Election, c);
            //PrintVoterCard pv = new PrintVoterCard(vc);
            //pv.Show();

            //VoterCardPrinter vcp = new VoterCardPrinter();
            //vcp.Print(vc);
            //Debug.WriteLine("JEG ER HER!!");
            VoterCardPrinter vcp = new VoterCardPrinter();
            vcp.Print(vc);
            Debug.WriteLine("JEG ER HER!!");*/
            Application app = new Application();
            VoterListApp.App = app;
            app.Startup += (o, e) =>
            {
                RunApp(null);
            };
            app.Run();
        }

        public static void RunApp(User user)
        {
            VoterListApp.CurrentUser = user;
            if (user != null && user.Validated)
            {
                MainWindow view = new MainWindow();
                new MainWindowController(view);
                /*
                IDataAccessObject dao = DAOFactory.CurrentUserDAO;
                User u = dao.LoadUser(2);
                u.ChangePassword("12345");
                 */
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
