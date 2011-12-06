﻿using System.Windows;
using DigitalVoterList.Controllers;
using DigitalVoterList.Election;
using DigitalVoterList.Views;

namespace DigitalVoterList
{
    using System.Diagnostics;

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
            Citizen c = (Citizen)dao.LoadPerson(1);
            VoterCard vc = new VoterCard(Settings.Election, c);
            VoterCardPrinter vcp = new VoterCardPrinter();
            vcp.Print(vc);
            Debug.WriteLine("JEG ER HER!!");
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
                MainWindow view = new MainWindow();
                view.Show();
                IDataAccessObject dao = DAOFactory.GlobalDAO;
                User u = dao.LoadUser(2);
                u.ChangePassword("12345");
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
