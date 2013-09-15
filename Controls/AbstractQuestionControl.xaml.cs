using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Threading;
using Anime_Quiz_3.Classes;

namespace Anime_Quiz_3.Controls
{
    /// <summary>
    /// Interaction logic for AbstractQuestionControl.xaml
    /// </summary>
    [TypeDescriptionProvider(typeof(AbstractQuestionControlDescriptionProvider<AbstractQuestionControl,UserControl>))]
    public abstract partial class AbstractQuestionControl : UserControl
    {
        protected Label answerLabel;

        public AbstractQuestionControl()
        {
            InitializeComponent();
            loadAnswerLabel();
        }

        protected abstract void loadQuestion();
        public abstract void startQuestion();
        public abstract void pauseQuestion();

        public abstract void showAnswer();

        private void loadAnswerLabel()
        {
            answerLabel = new Label();
            answerLabel.Content = "A) " + CurrentQuestion.getInstance().Answer;
            answerLabel.FontSize = 50;
            answerLabel.Visibility = System.Windows.Visibility.Collapsed;
            pageStack.Children.Add(answerLabel);
        }

    }
}
