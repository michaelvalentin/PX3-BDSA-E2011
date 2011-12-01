using System.Diagnostics;
using System.Windows;
using DigitalVoterList.Views;

namespace DigitalVoterList
{
    using DigitalVoterList.Election;

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
            Debug.WriteLine("HEY HEY!");
            IDataAccessObject dao = new DAOMySql();
            Person p = dao.LoadPerson(2);
            Debug.WriteLine(p.ToString());
            Application app = new Application();
            app.Startup += (object sender, StartupEventArgs e) =>
            {
                LoginWindow view = new LoginWindow();
                view.Show();
            };
            app.Run();
        }
    }
}
