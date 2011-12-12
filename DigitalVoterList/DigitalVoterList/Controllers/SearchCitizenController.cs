using System;
using System.Collections.Generic;
using DigitalVoterList.Views;

namespace DigitalVoterList.Controllers
{
    using DigitalVoterList.Election;

    class SearchCitizenController
    {
        private SearchCitizenView _view;
        private List<Citizen> _searchCitizen;
        public Action<Citizen> CitizenFound;
        public List<Citizen> SearchResult
        {
            get { return new List<Citizen>(_searchCitizen); }
        }

        public SearchCitizenController(SearchCitizenView view)
        {
            _view = view;
            _view.SearchResultsGrid.IsReadOnly = true;

            _view.SearchButton.Click += (s, e) => Search();

            _view.SelectButton.Click += (s, e) => Select();
            _view.SearchResultsGrid.MouseDoubleClick += (s, e) => Select();
        }

        /// <summary>
        /// Clear the listbox and the textblocks in the view
        /// </summary>
        private void Clear()
        {
            _searchCitizen = null;
            LoadListBox();
            _view.nameTextBox.Text = "";
            _view.addressTextBox.Text = "";
            _view.cprTextBox.Text = "";
            _view.passportTextBox.Text = "";
        }

        /// <summary>
        /// Search for a person with the information inserted in the textblocks and insert
        /// every person as an item in the listbox
        /// </summary>
        public void Search()
        {
            var searchParams = new Dictionary<CitizenSearchParam, object>();
            if (_view.nameTextBox.Text != "")
            {
                searchParams.Add(CitizenSearchParam.Name, _view.nameTextBox.Text);
            }
            if (_view.addressTextBox.Text != "")
            {
                searchParams.Add(CitizenSearchParam.Address, _view.addressTextBox.Text);
            }
            if (_view.cprTextBox.Text != "")
            {
                searchParams.Add(CitizenSearchParam.Cpr, _view.cprTextBox.Text);
            }
            if (searchParams.Count > 0) _searchCitizen = DAOFactory.CurrentUserDAO.FindCitizens(searchParams, SearchMatching.Similair);
            LoadListBox();
        }

        private void LoadListBox()
        {
            var citizenData = new List<CitizenData>();
            if (_searchCitizen != null)
            {
                foreach (var c in _searchCitizen)
                {
                    citizenData.Add(new CitizenData()
                                        {
                                            Name = c.Name,
                                            Address = c.Address,
                                            Cpr = c.Cpr,
                                            EligibleToVote = c.EligibleToVote,
                                            HasVoted = c.HasVoted
                                        });
                }
            }
            _view.SearchResultsGrid.ItemsSource = citizenData;
        }

        /// <summary>
        /// Invoke the Citizen found event with the currently selected citizen from the search results
        /// </summary>
        public void Select()
        {
            int index = _view.SearchResultsGrid.SelectedIndex;
            if (index < 0 || _searchCitizen == null || index >= _searchCitizen.Count) return;
            CitizenFound.Invoke(_searchCitizen[index]);
        }

        struct CitizenData
        {
            public string Name { get; set; }
            public string Address { get; set; }
            public string Cpr { get; set; }
            public bool EligibleToVote { get; set; }
            public bool HasVoted { get; set; }
        }
    }
}
