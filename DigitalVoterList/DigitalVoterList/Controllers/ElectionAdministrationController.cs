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
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Controller for collection administration
    /// </summary>
    public class ElectionAdministrationController : ContentController
    {

        public ElectionAdministrationController(ElectionAdministrationView view)
        {
            Contract.Requires(view != null);
            View = view;
            _neededPermissions.Add(SystemAction.AllVotingPlaces);
            _neededPermissions.Add(SystemAction.LoadCitizen);
            _neededPermissions.Add(SystemAction.LoadVoterCard);
            _neededPermissions.Add(SystemAction.FindCitizen);
            _neededPermissions.Add(SystemAction.FindVotingVenue);
            _neededPermissions.Add(SystemAction.UpdateCitizens);
            _neededPermissions.Add(SystemAction.UpdateVoterCards);
            _neededPermissions.Add(SystemAction.PrintVoterCards);

            _view.ImportDataBtn.Click += (s, e) => ImportData();
            _view.UpdateVoterCardsBtn.Click += (s, e) => UpdateVoterCards();
            _view.PrintVoterCardsBtn.Click += (s, e) => PrintVoterCards();
        }

        private ElectionAdministrationView _view
        {
            get
            {
                return (ElectionAdministrationView)View;
            }
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
