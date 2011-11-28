using System.Diagnostics;
using System.Windows;

namespace DigitalVoterList
{

    /// <summary>
    /// The main class for initializing the application
    /// </summary>
    public class Start
    {
        [System.STAThread]
        public static void Main()
        {
            Debug.WriteLine("HEY HEY!");
            Application app = new Application();
            app.Startup += (object sender, StartupEventArgs e) =>
            {
                MainWindow view = new MainWindow();
                view.Show();
            };
            app.Run();
        }
    }
}
