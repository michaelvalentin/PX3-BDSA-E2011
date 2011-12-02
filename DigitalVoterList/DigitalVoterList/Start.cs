using System.Diagnostics;
using System.Windows;
using DigitalVoterList.Views;

namespace DigitalVoterList
{
    using System.Collections.Generic;
    using System.Windows.Documents;

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
            Debug.WriteLine("START!");
            IDataAccessObject dao = new DAOMySql();
            List<Citizen> p = dao.FindElegibleVoters();
            foreach (var eligibleVoters in p)
            {
                Debug.WriteLine("cpr: "+eligibleVoters.Cpr);   
            }
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
