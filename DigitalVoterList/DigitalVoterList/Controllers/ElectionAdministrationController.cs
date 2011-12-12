/*
 * Authors:
 * Team: PX3
 * Date: 12-12-2011
 */

using DigitalVoterList.Election;
using DigitalVoterList.Views;

namespace DigitalVoterList.Controllers
{

    /// <summary>
    /// Controller for collection administration
    /// </summary>
    public class ElectionAdministrationController : ContentController
    {
        private ElectionAdministrationView _view;

        public ElectionAdministrationController(ElectionAdministrationView view)
        {
            View = view;
            _view = view;
            _neededPermissions.Add(SystemAction.AllVotingPlaces);
            //TODO: Write more needed permissions...
        }
    }
}
