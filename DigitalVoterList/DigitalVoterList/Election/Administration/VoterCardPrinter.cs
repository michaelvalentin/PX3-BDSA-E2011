namespace DigitalVoterList.Election.Administration
{
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
            PrintDialog printDlg = new System.Windows.Controls.PrintDialog();
            PrintVoterCard newVoterCard = new PrintVoterCard(voterCard);
            if (printDlg.ShowDialog() == true)
            {
                printDlg.PrintVisual(newVoterCard, "Print Single VoterCard");
            }
        }
    }
}
