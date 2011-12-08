using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DigitalVoterList.Views
{
    using DigitalVoterList.Election;
    using DigitalVoterList.Utilities;

    /// <summary>
    /// Interaction logic for ManualVoterValidationView.xaml
    /// </summary>
    public partial class ManualVoterValidationView : UserControl
    {
        public ManualVoterValidationView()
        {
            InitializeComponent();
            LeftTextBlock.Text = "Passport Number:" + Environment.NewLine + "Birthday:" + Environment.NewLine + "Place of birth:";
        }

        public void Show(Citizen citizen)
        {
            foreach (var quiz in citizen.SecurityQuestions)
            {
                TextBlock leftTextBlock = new TextBlock();
                leftTextBlock.Text = quiz.Question;
                leftTextBlock.TextWrapping = TextWrapping.Wrap;
                leftTextBlock.TextAlignment = TextAlignment.Left;
                leftTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
                leftTextBlock.Width = 250;

                TextBlock rightTextBlock = new TextBlock();
                rightTextBlock.Text = quiz.Answer + Environment.NewLine;
                rightTextBlock.TextWrapping = TextWrapping.Wrap;
                rightTextBlock.TextAlignment = TextAlignment.Left;
                rightTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
                rightTextBlock.Width = 150;

                WrapPanel newWrapPanel = new WrapPanel();
                newWrapPanel.HorizontalAlignment = HorizontalAlignment.Left;
                newWrapPanel.Children.Add(leftTextBlock);
                newWrapPanel.Children.Add(rightTextBlock);

                stackPanel1.Children.Add(newWrapPanel);
            }
            RightTextBlock.Text = citizen.PassportNumber + Environment.NewLine + Birthday(citizen) + Environment.NewLine
                                  + citizen.PlaceOfBirth;
        }

        public void Hide()
        {
            stackPanel1.Children.RemoveRange(0, stackPanel1.Children.Count);
            RightTextBlock.Text = "";
        }

        private string Birthday(Citizen citizen)
        {
            string temp = citizen.Cpr.Substring(0, 6);
            return temp.Substring(0, 2) + "-" + temp.Substring(2, 2) + "-" + temp.Substring(4, 2);
        }
    }
}