using System;
using System.Collections.Generic;
using System.Linq;
using DigitalVoterList.Views;

namespace DigitalVoterList.Controllers
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using DigitalVoterList.Election;

    class SearchCitizenController
    {
        private SearchCitizenView _view;
        private SearchCitizenWindow _window;
        private List<Citizen> _searchCitizen;
        public Action<Citizen> CitizenFound;
        public List<Citizen> SearchResult
        {
            get { return new List<Citizen>(_searchCitizen); }
        }

        public SearchCitizenController(SearchCitizenView view, SearchCitizenWindow window)
        {
            _window = window;
            _view = view;

            _view.SearchButton.Click += (s, e) => Search();

            _view.SelectButton.Click += (s, e) =>
                                            {
                                                int selectedIndex = _view.SearchListBox.SelectedIndex;
                                                Select(selectedIndex);
                                            };
            _view.SearchListBox.MouseDoubleClick += (s, e) =>
                                                        {
                                                            int selectedIndex = ((ListBox)s).SelectedIndex;
                                                            Select(selectedIndex);
                                                        };

            _view.QuitButton.Click += (s, e) => _window.Close();
            _window.Closed += (s, e) => Clear();
            _window.LostFocus += (s, e) => _window.Focus(); //Force focus on window as long as it is open...
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
            _searchCitizen = DAOFactory.CurrentUserDAO.FindCitizens(searchParams, SearchMatching.Similair);
            LoadListBox();
        }

        private void LoadListBox()
        {
            if (_searchCitizen == null)
            {
                _view.SearchListBox.Items.Clear();
                return;
            }
            foreach (var c in _searchCitizen)
            {
                //TODO: Make a representation..
                /*TextBlock nameTextBlock = adjustTextBlock();
                nameTextBlock.Text = person.Name;
                TextBlock addressTextBlock = adjustTextBlock();
                addressTextBlock.Text = person.Address;
                TextBlock cprTextBlock = adjustTextBlock();
                cprTextBlock.Text = person.Cpr;
                TextBlock placeOfBirthTextBlock = adjustTextBlock();
                placeOfBirthTextBlock.Text = person.PlaceOfBirth;
                TextBlock passportNumberTextBlock = adjustTextBlock();
                passportNumberTextBlock.Text = person.PassportNumber;

                WrapPanel item = new WrapPanel();
                item.HorizontalAlignment = HorizontalAlignment.Left;
                item.Children.Add(nameTextBlock);
                item.Children.Add(addressTextBlock);
                item.Children.Add(cprTextBlock);
                item.Children.Add(placeOfBirthTextBlock);
                item.Children.Add(passportNumberTextBlock);

                _view.SearchListBox.Items.Add(item);*/
            }
        }

        /// <summary>
        /// Select a citizen from the search results, based on his index in the search results.
        /// </summary>
        /// <param name="index">The index to look for</param>
        public void Select(int index)
        {
            if (index < 0 || _searchCitizen == null || index >= _searchCitizen.Count) return;
            CitizenFound.Invoke(_searchCitizen[index]);
            _window.Close();
        }

        /// <summary>
        /// Find person with the information inserted in the textblocks
        /// </summary>
        /// <returns>A list of persons found from the inserted information</returns>
        private List<Citizen> Search2()
        {
            /*
            _searchCitizen.Clear();

            Person person = new Person();
            person.Name = _view.nameTextBox.Text;
            person.Address = _view.addressTextBox.Text;
            person.Cpr = _view.cprTextBox.Text;
            person.PassportNumber = _view.passportTextBox.Text;
            return DAOFactory.CurrentUserDAO.Find(person);
            */
            var persons = new List<Citizen>();
            for (int i = 0; i < 10; i++)
            {
                var p = new Citizen(0, "" + i + i + i + i);
                p.Name = "Anders nr. " + i;
                p.Address = "vej nr. " + i + " 0000 by " + i;
                p.PlaceOfBirth = "by" + i;
                p.Cpr = "" + i + i + i + i + i + i + i + i + i + i + i + i + i;
                p.PassportNumber = "" + i + i + i + i + i + i + i + i + i + i + i + i;
                persons.Add(p);
            }
            return persons;

            //return DAOFactory.CurrentUserDAO.Find(person);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Makes a textblock and adjusts its attributes
        /// </summary>
        /// <returns>Textblock to insert in the wrap panel</returns>
        private TextBlock adjustTextBlock()
        {
            TextBlock textBlock = new TextBlock();
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.TextAlignment = TextAlignment.Left;
            textBlock.HorizontalAlignment = HorizontalAlignment.Left;
            textBlock.Width = 180;

            return textBlock;
        }

        /// <summary>
        /// Adds items of persons to the listbox
        /// </summary>
        /// <param name="list">The list of persons which must be listed as items in the listbox</param>
        private void ListView(IEnumerable<Citizen> list)
        {
            foreach (var person in list)
            {
                TextBlock nameTextBlock = adjustTextBlock();
                nameTextBlock.Text = person.Name;
                TextBlock addressTextBlock = adjustTextBlock();
                addressTextBlock.Text = person.Address;
                TextBlock cprTextBlock = adjustTextBlock();
                cprTextBlock.Text = person.Cpr;
                TextBlock placeOfBirthTextBlock = adjustTextBlock();
                placeOfBirthTextBlock.Text = person.PlaceOfBirth;
                TextBlock passportNumberTextBlock = adjustTextBlock();
                passportNumberTextBlock.Text = person.PassportNumber;

                WrapPanel item = new WrapPanel();
                item.HorizontalAlignment = HorizontalAlignment.Left;
                item.Children.Add(nameTextBlock);
                item.Children.Add(addressTextBlock);
                item.Children.Add(cprTextBlock);
                item.Children.Add(placeOfBirthTextBlock);
                item.Children.Add(passportNumberTextBlock);

                _view.SearchListBox.Items.Add(item);
            }
        }

        /// <summary>
        /// Finds the selected item in the list
        /// </summary>
        private Person SelectedListBoxItem()
        {
            Person p = this._searchCitizen.ElementAt(_view.SearchListBox.SelectedIndex);
            //FireCitizenFoundEvent(p);
            Clear();
            return p;
        }

        /// <summary>
        /// Clears and quits the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuitEvent(object sender, RoutedEventArgs e)
        {
            this.Clear();
            this._window.Close();
        }

        /// <summary>
        /// Finds the selected item
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event</param>
        private void SelectEvent(object sender, RoutedEventArgs e)
        {
            if (_view.SearchListBox.SelectedIndex != -1)
            {
                SelectedListBoxItem();
            }
            else
            {
                this._view.statusTextBlock.Text = "You must select a person";
            }
        }

        /// <summary>
        /// Finds the selected item
        /// </summary>
        private void SearchListBoxMouseDoubleClickEvent(object sender, MouseButtonEventArgs e)
        {
            this.SelectedListBoxItem();
        }
    }
}
