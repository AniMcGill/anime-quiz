using System;
using System.Windows.Controls;
using Anime_Quiz_3.Classes;
using Anime_Quiz_3.Controls;
using GameContext;

namespace Anime_Quiz_3.Player
{
    /// <summary>
    /// Interaction logic for QuestionPage.xaml
    /// </summary>
    public partial class QuestionPage : Page
    {
        // Controls
        AbstractQuestionControl questionControl;

        public QuestionPage()
        {
            InitializeComponent();
            initializePage();
        }

        private void initializePage()
        {
            switch (CurrentQuestionSet.getInstance().Type)
            {
                case (int)Types.Question:
                    questionControl = new QuestionControl();
                    break;
                case (int)Types.Music:
                    questionControl = new MusicControl();
                    break;
                case(int)Types.Screenshot:
                    questionControl = new ScreenshotControl();
                    break;
            }
            pageStack.Children.Add(questionControl);
            //if settings contain autostart, startQuestion()
            if (Properties.Settings.Default.autoplay)
                questionControl.startQuestion();
        }
        public void showAnswer()
        {
            questionControl.showAnswer();
        }
    }
}
