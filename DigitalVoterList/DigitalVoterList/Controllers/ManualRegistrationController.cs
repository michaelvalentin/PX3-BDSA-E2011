using DigitalVoterList.Election;
using DigitalVoterList.Views;

namespace DigitalVoterList.Controllers
{

    /// <summary>
    /// A controller for the manual registration project
    /// </summary>
    public class ManualRegistrationController : ContentController
    {
        private ManualRegistrationView _view;

        public ManualRegistrationController(ManualRegistrationView view)
        {
            _view = view;
            View = _view;
            _neededPermissions.Add(SystemAction.FindVoterCard);
            _neededPermissions.Add(SystemAction.ScanVoterCard);
            _neededPermissions.Add(SystemAction.SetHasVoted);
            _neededPermissions.Add(SystemAction.FindPerson);
            _neededPermissions.Add(SystemAction.SetHasVotedManually);
        }
    }
}
