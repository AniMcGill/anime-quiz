using System;
using System.Windows.Controls;
using Anime_Quiz_3.Classes;

namespace Anime_Quiz_3.Player
{
    /// <summary>
    /// Interaction logic for QuestionPage.xaml
    /// </summary>
    public partial class QuestionPage : Page
    {
        // Controls
        Label answerLabel;
        Label questionControl;
        MediaElement musicPlayer;

        public QuestionPage()
        {
            InitializeComponent();
            initializePage();
        }

        private void initializePage()
        {
            loadAnswerLabel();
            // 0: Question, 1: Music 2: Screenshot
            switch (CurrentQuestionSet.getInstance().Type)
            {
                case 0:
                    loadQuestion();
                    break;
                case 1:
                    loadMusic();
                    break;
                case 2:
                    loadScreenshot();
                    break;
            }
        }
        private void loadAnswerLabel()
        {
            answerLabel = new Label();
            answerLabel.Content = "A) " + CurrentQuestion.getInstance().Answer;
            answerLabel.FontSize = 50;
            answerLabel.Visibility = System.Windows.Visibility.Collapsed;
            pageStack.Children.Add(answerLabel);
        }
        private void loadQuestion()
        {
            questionControl = new Label();
            questionControl.Content = "Q) " + CurrentQuestion.getInstance().Question;
            questionControl.FontSize = 50;
            
            pageStack.Children.Add(questionControl);
        }
        private void loadMusic()
        {
            musicPlayer = new MediaElement();
            musicPlayer.Source = new Uri(CurrentQuestion.getInstance().Question, UriKind.RelativeOrAbsolute);
            musicPlayer.Play(); //TODO: autoplay option

            pageStack.Children.Add(musicPlayer);
        }
        private void loadScreenshot()
        {
            //TODO
        }

        public void showAnswer()
        {
            switch (CurrentQuestionSet.getInstance().Type)
            {
                case 0:
                    break;
                case 1:
                    musicPlayer.Stop();
                    break;
                case 2:
                    //TODO
                    break;
            }
            answerLabel.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
