using System;
using System.Windows.Controls;
using Anime_Quiz_3.Classes;
using GameContext;

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
            switch (CurrentQuestionSet.getInstance().Type)
            {
                case (int)Types.Question:
                    loadQuestion();
                    break;
                case (int)Types.Music:
                    loadMusic();
                    break;
                case (int)Types.Screenshot:
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
                case (int)Types.Question:
                    break;
                case (int)Types.Music:
                    musicPlayer.Stop();
                    break;
                case (int)Types.Screenshot:
                    //TODO
                    break;
            }
            answerLabel.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
