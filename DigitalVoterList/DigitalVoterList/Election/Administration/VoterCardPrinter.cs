namespace DigitalVoterList.Election.Administration
{
    using System;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Printing;
    using System.Windows.Documents;
    using System.Windows.Media;
    using System.Windows.Xps;

    /// <summary>
    /// A printer used to create the physical voter cards
    /// </summary>
    public class VoterCardPrinter : FrameworkElement
    {
        public VoterCardPrinter()
        {
        }
        
        public void Print(VoterCard voterCard)
        {
            {
                PrintVoterCard printVoterCard = new PrintVoterCard(voterCard);
                PrintDialog dialog = new PrintDialog();
                if (dialog.ShowDialog() == true)
                { dialog.PrintVisual(printVoterCard.PrintPage, "Print Voter Card"); }
            }
        }
    }
}
