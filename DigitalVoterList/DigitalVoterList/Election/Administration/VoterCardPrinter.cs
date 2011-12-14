/*
 * Authors: Morten, Jens
 * Team: PX3
 * Date: 12-12-2011
 */

using System;

namespace DigitalVoterList.Election.Administration
{
    using System.Diagnostics.Contracts;
    using System.Windows.Controls;

    /// <summary>
    /// A printer used to create the physical voter cards
    /// </summary>
    public class VoterCardPrinter
    {
        public VoterCardPrinter()
        {
        }

        [STAThread]
        public void Print(VoterCard voterCard)
        {
            Contract.Requires(voterCard != null);
            PrintVoterCard printVoterCard = new PrintVoterCard(voterCard);

            PrintDialog dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            {
                dialog.PrintVisual(printVoterCard.printPage, "Print Voter Card");
            }
        }

        //WHY is there no simple way to print multiple pages from C#?!
        /*public static void Print(List<VoterCard> voterCards)
        {
            var p = new StackPanel();
            p.Children.Add(new SearchCitizenView());
            p.Children.Add(new SearchCitizenView());
            p.Children.Add(new SearchCitizenView());
            p.Children.Add(new SearchCitizenView());
            PrintDialog dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            {
                dialog.PrintDocument(DocumentPaginator.);
            }
        }*/
    }
}
