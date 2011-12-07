using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DigitalVoterList.Election.Administration
{
    //using System.Drawing;

    using System.Diagnostics;

    using Color = System.Windows.Media.Color;

    /// <summary>
    /// Interaction logic for PrintVoterCard.xaml
    /// </summary>
    public partial class PrintVoterCard : Window
    {
        public PrintVoterCard(VoterCard voterCard)
        {
            InitializeComponent();
            
            ElectionNameLabel.Content = voterCard.ElectionEvent.Name;
            //VotingVenueTextBlock.Text = voterCard.VotingVenue,name;
            IdLabel.Content = voterCard.Id;
            voterCard.IdKey = "1234abcd";
            BarcodeLabel.Content = voterCard.IdKey;
            Debug.WriteLine("IDKEY: "+voterCard.IdKey);
            BarCodeTextBlock.Text = voterCard.IdKey;
            AddressTextBlock.Text = voterCard.Citizen.Name + Environment.NewLine + voterCard.Citizen.Address;
        }
    }
}