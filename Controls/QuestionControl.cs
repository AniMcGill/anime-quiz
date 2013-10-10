using System;
using System.Timers;
using System.Windows.Controls;
using Anime_Quiz_3.Classes;

namespace Anime_Quiz_3.Controls
{
    public sealed class QuestionControl : AbstractQuestionControl
    {
        TextBlock questionControl;
        Timer timer;
        CharEnumerator charEnumerator;

        public QuestionControl()
        {
            loadQuestion();
        }

        protected override void loadQuestion()
        {
            charEnumerator = CurrentQuestion.getInstance().Question.GetEnumerator();
            questionControl = new TextBlock();
            questionControl.MaxWidth = 1000;
            questionControl.TextWrapping = System.Windows.TextWrapping.Wrap;
            questionControl.Text = "Q) ";
            questionControl.FontSize = 50;

            pageStack.Children.Add(questionControl);

            // Convert the duration to miliseconds, then divide by the number of characters in the question
            timer = new Timer();
            timer.Interval = Properties.Settings.Default.duration * 1000 / CurrentQuestion.getInstance().Question.Length;
            timer.Elapsed += timer_Elapsed;
        }

        public override void startQuestion()
        {
            timer.Start();
        }
        public override void pauseQuestion()
        {
            timer.Stop();
        }
        public override void showAnswer()
        {
            timer.Stop();
            if (charEnumerator.MoveNext())
                questionControl.Text = "Q) " + CurrentQuestion.getInstance().Question;
            answerLabel.Visibility = System.Windows.Visibility.Visible;
        }

        void updateQuestion()
        {
            if (charEnumerator.MoveNext())
                questionControl.Text += charEnumerator.Current.ToString();
            else
                timer.Stop();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(updateQuestion, System.Windows.Threading.DispatcherPriority.Normal);
        }
    }
}
