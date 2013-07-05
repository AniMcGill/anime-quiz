using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Anime_Quiz_3.Classes;
using Anime_Quiz_3.GameMaster;
using GameContext;

namespace Anime_Quiz_3.Player
{
    /// <summary>
    /// Interaction logic for QuestionSetPage.xaml
    /// </summary>
    public partial class QuestionSetPage : Page
    {
        WrapPanel pointBoxesPanel;
        public QuestionSetPage()
        {
            InitializeComponent();
            loadQuestions();
        }

        private void loadQuestions()
        {
            pointBoxesPanel = new WrapPanel();
            pointBoxesPanel.Margin = new Thickness(10, 10, 10, 10);
            pageGrid.Children.Add(pointBoxesPanel);
            var questions = from question in GameStartPage.db.GetTable<Questions>()
                                 where question.QuestionSetId.Equals(CurrentQuestionSet.getInstance().QuestionSetId) &&
                                       question.Answered.Equals(false)
                                 select question;
            foreach (var question in questions)
            {
                Button pointBtn = new Button();
                pointBtn.Content = question.Points;
                pointBtn.FontSize = 20.0;
                pointBtn.Width = 150;
                pointBtn.Height = 75;
                pointBtn.Click += (sender,args) =>
                    pointBtn_Click(question.QuestionId, args);
                pointBoxesPanel.Children.Add(pointBtn);
            }
        }

        public event EventHandler QuestionSelected;
        protected virtual void OnQuestionSelected(EventArgs e)
        {
            EventHandler handler = QuestionSelected;
            if (handler != null)
                handler(this, e);
        }
        void pointBtn_Click(int questionId, RoutedEventArgs e)
        {
            CurrentQuestion.setInstance((from question in GameStartPage.db.GetTable<Questions>()
                                         where question.QuestionId.Equals(questionId)
                                         select question).Single());
            OnQuestionSelected(EventArgs.Empty);
        }
    }
}
