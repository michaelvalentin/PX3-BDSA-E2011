/*
 * Authors:
 * Team: PX3
 * Date: 12-12-2011
 */

using System;
using System.Windows;
using System.Windows.Controls;
using DigitalVoterList.Election;
using DigitalVoterList.Utilities;
using DigitalVoterList.Views;

namespace DigitalVoterList.Controllers
{
    using System.Diagnostics;

    class ManualVoterValidationController
    {
        private ManualVoterValidationView _view;
        private Citizen _voter;

        public ManualVoterValidationController(ManualVoterValidationView view, Citizen voter)
        {
            _view = view;
            _voter = voter;
            _view.QuestionListBox.Items.Clear();
            _view.LeftTextBlock.Text = "Passport Number:" + Environment.NewLine + "Birthday:" + Environment.NewLine + "Place of birth:" + Environment.NewLine + "Eligible to vote:" + Environment.NewLine + "Has voted:";
        }

        public void Show()
        {
            if (_voter == null) return;
            foreach (var quiz in _voter.SecurityQuestions)
            {
                WrapPanel questionWrapPanel = new WrapPanel();
                questionWrapPanel.HorizontalAlignment = HorizontalAlignment.Left;
                questionWrapPanel.Children.Add(AdjustedQuestionTextBox(quiz));
                questionWrapPanel.Children.Add(AdjustedAnswerTextBox(quiz));

                _view.QuestionListBox.Items.Add(questionWrapPanel);
            }
            _view.RightTextBlock.Text = _voter.PassportNumber + Environment.NewLine + Birthday(_voter) + Environment.NewLine
                + _voter.PlaceOfBirth + Environment.NewLine + (_voter.EligibleToVote ? "Yes" : "No") + Environment.NewLine + (_voter.HasVoted ? "Yes" : "No");
        }

        private TextBlock AdjustedQuestionTextBox(Quiz quiz)
        {
            Debug.WriteLine("Made Question: " + quiz.Question);
            TextBlock questionTextBlock = new TextBlock();
            questionTextBlock.Text = quiz.Question;
            questionTextBlock.TextWrapping = TextWrapping.Wrap;
            questionTextBlock.TextAlignment = TextAlignment.Left;
            questionTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
            questionTextBlock.Width = 250;

            return questionTextBlock;
        }

        private TextBlock AdjustedAnswerTextBox(Quiz quiz)
        {
            Debug.WriteLine("Made Answer: " + quiz.Answer);
            TextBlock answerTextBlock = new TextBlock();
            answerTextBlock.Text = quiz.Answer + Environment.NewLine;
            answerTextBlock.TextWrapping = TextWrapping.Wrap;
            answerTextBlock.TextAlignment = TextAlignment.Left;
            answerTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
            answerTextBlock.Width = 150;

            return answerTextBlock;
        }

        public void Hide()
        {
            _view.QuestionListBox.Items.Clear();
        }

        private string Birthday(Citizen citizen)
        {
            string temp = citizen.Cpr.Substring(0, 6);
            return temp.Substring(0, 2) + "-" + temp.Substring(2, 2) + "-" + temp.Substring(4, 2);
        }
    }
}
