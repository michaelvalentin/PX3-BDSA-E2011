using System;
using System.Collections.Generic;
using System.Windows.Controls;
using DigitalVoterList.Views;

namespace DigitalVoterList.Controllers
{
    /// <summary>
    /// A controller for the main window
    /// </summary>
    public class MainWindowController
    {
        private MainWindow _view;
        private Dictionary<MenuItem, ContentController> _functionMapping;

        public MainWindowController(MainWindow view)
        {
            _view = view;
            _view.Show();

            _functionMapping = new Dictionary<MenuItem, ContentController>();
            _functionMapping.Add(_view.NormalRegistration, new VoterRegistrationController(new VoterRegistrationView()));
            _functionMapping.Add(_view.ManualRegistration, new ManualRegistrationController(new ManualRegistrationView()));
            _functionMapping.Add(_view.ElectionAdministration, new ElectionAdministrationController(new ElectionAdministrationView()));

            UpdateMenuAccess();
            ShowScreen(_functionMapping[_view.NormalRegistration]);

            _view.Exit.Click += (s, e) => VoterListApp.App.Shutdown();
        }

        public void UpdateMenuAccess()
        {
            foreach (KeyValuePair<MenuItem, ContentController> entry in _functionMapping)
            {
                MenuItem menu = entry.Key;
                ContentController cont = entry.Value;

                //Disable function that user doesn't have permission to use
                if (cont.HasPermissionToUse(VoterListApp.CurrentUser))
                {
                    menu.IsEnabled = true;
                    menu.Click += MenuClicked;
                }
                else
                {
                    menu.IsEnabled = false;
                    menu.Click -= MenuClicked;
                }
            }
        }

        private void MenuClicked(object sender, EventArgs e)
        {
            MenuItem clicked = (MenuItem)sender;
            ShowScreen(_functionMapping[clicked]);
        }

        public void ShowScreen(ContentController c)
        {
            _view.MainContent.Children.Clear();
            _view.MainContent.Children.Add(c.View);
        }
    }
}
