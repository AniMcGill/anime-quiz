using System;
using System.Windows.Controls;
using Anime_Quiz_3.Classes;

namespace Anime_Quiz_3.Controls
{
    public sealed class QuestionControl : AbstractQuestionControl
    {
        Label questionControl;
        public QuestionControl()
        {
            loadQuestion();
        }

        protected override void loadQuestion()
        {
            questionControl = new Label();
            questionControl.Content = "Q) " + CurrentQuestion.getInstance().Question;
            questionControl.FontSize = 50;

            pageStack.Children.Add(questionControl);
        }
        public override void startQuestion()
        {
            throw new NotImplementedException();
        }
        public override void pauseQuestion()
        {
            throw new NotImplementedException();
        }

        public override void showAnswer()
        {
            answerLabel.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
