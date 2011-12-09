using System.Windows;
using DigitalVoterList.Controllers;
using DigitalVoterList.Election;
using DigitalVoterList.Views;

namespace DigitalVoterList
{

    /// <summary>
    /// The main class for initializing the application
    /// </summary>
    public class VoterListApp
    {
        private static User _currentUser;
        public static Application App;
        private static MainWindow _mainWindow;

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
            Application app = new Application();
            VoterListApp.App = app;
            app.Startup += (o, e) =>
            {
                DAOFactory.ConnectionString = "SERVER=localhost;" +
                "DATABASE=px3;" +
                "UID=root;" +
                "PASSWORD=abcd1234;";
                User u = User.GetUser("mier", "12345");
                RunApp(u);
            };
            app.Run();
        }

        public static void RunApp(User user)
        {
            _currentUser = user;
            if (user != null && user.Validated)
            {
                _mainWindow = new MainWindow();
                new MainWindowController(_mainWindow);
            }
            else
            {
                //Show the login window
                LoginWindow view = new LoginWindow();
                new LoginController(view);
            }
        }

        public static void RunApp()
        {
            RunApp(CurrentUser);
        }

        public static void LogOut()
        {
            _currentUser = null;
            RunApp();
            _mainWindow.Close();
        }
    }
}
