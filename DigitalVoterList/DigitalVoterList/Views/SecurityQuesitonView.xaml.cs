using System;
using System.Windows.Controls;
using DigitalVoterList.Utilities;

namespace DigitalVoterList.Views
{
    /// <summary>
    /// Interaction logic for SecurityQuesitonView.xaml
    /// </summary>
    public partial class SecurityQuesitonView : UserControl
    {
        public event EventHandler<EventArgs> QuestionRequest;

        public SecurityQuesitonView()
        {
            InitializeComponent();
        }

        private void FireQuestionRequest(EventArgs e)
        {
            if (QuestionRequest != null) QuestionRequest(this, e);
        }

        private void _newQuestionBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FireQuestionRequest(e);
        }

        public void SetQuestion(Quiz q)
        {
            Question.Text = q.Question;
            Answer.Text = q.Answer;
        }

        public void Reset()
        {
            Question.Text = "";
            Answer.Text = "";
        }
    }
}
