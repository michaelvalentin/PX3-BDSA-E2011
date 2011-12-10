// -----------------------------------------------------------------------
// <copyright file="NormalVoterRegistrationController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Windows;
using DigitalVoterList.Election;
using DigitalVoterList.Views;
using System.Linq.Expressions;

namespace DigitalVoterList.Controllers
{
    using System;
    using System.Linq;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class NormalVoterRegistrationController : VoterRegistrationController
    {
        private VoterRegistrationView _view;
        private int cprTries;

        public NormalVoterRegistrationController(VoterRegistrationView view)
            : base(view)
        {
            _view = view;
            Disable(_view.VoterIdentification.VoterCprBirthday);
            _view.VoterIdentification.VoterCprBirthday.Text = "XXXXXX";

            _view.SearchVoterButton.Visibility = Visibility.Hidden;
            cprTries = 0;
            
            _view.VoterValidation.Children.Clear();
            _view.VoterValidation.Children.Add(new SecurityQuesitonView());
            _view.Height = 314;

            
            _view.VoterIdentification.VoterCprDigits.PasswordChanged += CheckCpr;
            _view.RegisterVoterButton.Click += RegisterVoter;
            _view.VoterIdentification.PreviewKeyDown += HideImages;
        }
    }
}
