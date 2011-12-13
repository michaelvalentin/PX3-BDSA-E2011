﻿/*
 * Authors:
 * Team: PX3
 * Date: 12-12-2011
 */

using System;
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
        private static LoginWindow _loginWindow;

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
            /*
            VoterCardPrinter.Print(new List<VoterCard>()
                                       {
                                           new VoterCard(){Id=1, IdKey="ABCD1234", Citizen = new Citizen(1,"1212971234"){Name="Test person",Address="En by et eller andet sted", VotingPlace = new VotingVenue(1, "Test valgsted","Test adresse på valgsted")}, ElectionEvent = Settings.Election, Valid = true},
                                           new VoterCard(){Id=1, IdKey="ABCD1234", Citizen = new Citizen(1,"1212971234"){Name="Test person",Address="En by et eller andet sted", VotingPlace = new VotingVenue(1, "Test valgsted","Test adresse på valgsted")}, ElectionEvent = Settings.Election, Valid = true},
                                           new VoterCard(){Id=1, IdKey="ABCD1234", Citizen = new Citizen(1,"1212971234"){Name="Test person",Address="En by et eller andet sted", VotingPlace = new VotingVenue(1, "Test valgsted","Test adresse på valgsted")}, ElectionEvent = Settings.Election, Valid = true},
                                           new VoterCard(){Id=1, IdKey="ABCD1234", Citizen = new Citizen(1,"1212971234"){Name="Test person",Address="En by et eller andet sted", VotingPlace = new VotingVenue(1, "Test valgsted","Test adresse på valgsted")}, ElectionEvent = Settings.Election, Valid = true}, 
                                           new VoterCard(){Id=1, IdKey="ABCD1234", Citizen = new Citizen(1,"1212971234"){Name="Test person",Address="En by et eller andet sted", VotingPlace = new VotingVenue(1, "Test valgsted","Test adresse på valgsted")}, ElectionEvent = Settings.Election, Valid = true},
                                           new VoterCard(){Id=1, IdKey="ABCD1234", Citizen = new Citizen(1,"1212971234"){Name="Test person",Address="En by et eller andet sted", VotingPlace = new VotingVenue(1, "Test valgsted","Test adresse på valgsted")}, ElectionEvent = Settings.Election, Valid = true},
                                           new VoterCard(){Id=1, IdKey="ABCD1234", Citizen = new Citizen(1,"1212971234"){Name="Test person",Address="En by et eller andet sted", VotingPlace = new VotingVenue(1, "Test valgsted","Test adresse på valgsted")}, ElectionEvent = Settings.Election, Valid = true},
                                           new VoterCard(){Id=1, IdKey="ABCD1234", Citizen = new Citizen(1,"1212971234"){Name="Test person",Address="En by et eller andet sted", VotingPlace = new VotingVenue(1, "Test valgsted","Test adresse på valgsted")}, ElectionEvent = Settings.Election, Valid = true},
                                           new VoterCard(){Id=1, IdKey="ABCD1234", Citizen = new Citizen(1,"1212971234"){Name="Test person",Address="En by et eller andet sted", VotingPlace = new VotingVenue(1, "Test valgsted","Test adresse på valgsted")}, ElectionEvent = Settings.Election, Valid = true},
                                           new VoterCard(){Id=1, IdKey="ABCD1234", Citizen = new Citizen(1,"1212971234"){Name="Test person",Address="En by et eller andet sted", VotingPlace = new VotingVenue(1, "Test valgsted","Test adresse på valgsted")}, ElectionEvent = Settings.Election, Valid = true},
                                           new VoterCard(){Id=1, IdKey="ABCD1234", Citizen = new Citizen(1,"1212971234"){Name="Test person",Address="En by et eller andet sted", VotingPlace = new VotingVenue(1, "Test valgsted","Test adresse på valgsted")}, ElectionEvent = Settings.Election, Valid = true},
                                           new VoterCard(){Id=1, IdKey="ABCD1234", Citizen = new Citizen(1,"1212971234"){Name="Test person",Address="En by et eller andet sted", VotingPlace = new VotingVenue(1, "Test valgsted","Test adresse på valgsted")}, ElectionEvent = Settings.Election, Valid = true},
                                           new VoterCard(){Id=1, IdKey="ABCD1234", Citizen = new Citizen(1,"1212971234"){Name="Test person",Address="En by et eller andet sted", VotingPlace = new VotingVenue(1, "Test valgsted","Test adresse på valgsted")}, ElectionEvent = Settings.Election, Valid = true},
                                           new VoterCard(){Id=1, IdKey="ABCD1234", Citizen = new Citizen(1,"1212971234"){Name="Test person",Address="En by et eller andet sted", VotingPlace = new VotingVenue(1, "Test valgsted","Test adresse på valgsted")}, ElectionEvent = Settings.Election, Valid = true}                                                                                                                                                                                                                                                                   
                                       });
            return;*/
            App = new Application();
            App.Startup += (o, e) =>
            {
                //US AWS...
                /*DAOFactory.ConnectionString = 
                "SERVER=ec2-107-20-53-16.compute-1.amazonaws.com;" +
                "DATABASE=px3;" +
                "UID=root;" +
                "PASSWORD=abcd1234;";*/

                //EU AWS
                DAOFactory.ConnectionString =
                "SERVER=ec2-79-125-81-60.eu-west-1.compute.amazonaws.com;" +
                "DATABASE=px3;" +
                "UID=px3;" +
                "PASSWORD=abcd1234;";

                //LOCAL
                /*DAOFactory.ConnectionString = 
                "SERVER=localhost;" +
                "DATABASE=px3;" +
                "UID=root;" +
                "PASSWORD=abcd1234;";*/

                RunApp(CurrentUser);
            };
            try
            {
                App.Run();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " Unfortunately the application couldn't be recovered, and is therefore restarting.");
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }
        }

        public static void RunApp(User user)
        {
            _currentUser = user;
            if (user != null && user.Validated)
            {
                if (_mainWindow == null) _mainWindow = new MainWindow();
                new MainWindowController(_mainWindow);
            }
            else
            {
                if (_loginWindow != null)
                {
                    _loginWindow.Close();
                    _loginWindow = null;
                }
                _loginWindow = new LoginWindow();
                _loginWindow.Closing += (s, e) =>
                                            {
                                                _loginWindow = null;
                                            };
                new LoginController(_loginWindow);
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
