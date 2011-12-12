/*
 * Authors:
 * Team: PX3
 * Date: 12-12-2011
 */

using DigitalVoterList.Election;
using DigitalVoterList.Election.Administration;
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

            _view.ImportDataBtn.Click += (s, e) => ImportData();
            _view.UpdateVoterCardsBtn.Click += (s, e) => UpdateVoterCards();
            _view.PrintVoterCardsBtn.Click += (s, e) => PrintVoterCards();
        }

        private void ImportData()
        {
            var dataTransformer = new DataTransformer();
            dataTransformer.TransformData();
        }

        private void UpdateVoterCards()
        {
            DAOFactory.CurrentUserDAO.UpdateVoterCards();
        }

        private void PrintVoterCards()
        {
            var printer = new VoterCardPrinter();
            DAOFactory.CurrentUserDAO.PrintVoterCards(printer);
        }
    }
}
