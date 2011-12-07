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
            DAOMySql dao = new DAOMySql();
            Citizen citizen = (Citizen)dao.LoadPerson(1);
            VoterCard vc = dao.LoadVoterCard(1);
            PrintVoterCard voterCard = new PrintVoterCard(vc);
            PrintDialog dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            { dialog.PrintVisual(voterCard.printPage, "Print Voter Card"); }
        }
    }
}
