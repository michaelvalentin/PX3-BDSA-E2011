﻿using System;
using DigitalVoterList.Election;
using DigitalVoterList.Views;

namespace DigitalVoterList.Controllers
{

    /// <summary>
    /// A controller for the manual registration project
    /// </summary>
    public class ManualVoterRegistrationController : VoterRegistrationController
    {
        private VoterRegistrationView _view;

        public ManualVoterRegistrationController(VoterRegistrationView view)
            : base(view)
        {
            _view = view;
            _neededPermissions.Add(SystemAction.FindPerson);
            _neededPermissions.Add(SystemAction.SetHasVotedManually);
        }

        protected override void LoadVoterValidation(Citizen c)
        {
            //TODO: Make
        }

        protected override void CheckCpr(object sender, EventArgs e)
        {
            //TODO: Make
        }

        protected override void RegisterVoter(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
