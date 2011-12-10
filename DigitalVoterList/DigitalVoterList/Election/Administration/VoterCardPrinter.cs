namespace DigitalVoterList.Election.Administration
{
    using System.Diagnostics.Contracts;
    using System.Windows;
    using System.Windows.Controls;

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
            Contract.Requires(voterCard != null);

            PrintVoterCard printVoterCard = new PrintVoterCard(voterCard);
            PrintDialog dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            { dialog.PrintVisual(printVoterCard.printPage, "Print Voter Card"); }
        }
    }
}
