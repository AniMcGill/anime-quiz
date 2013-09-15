using System;
using System.Windows.Controls;
using Anime_Quiz_3.Classes;

namespace Anime_Quiz_3.Controls
{
    public sealed class MusicControl : AbstractQuestionControl
    {
        MediaElement musicPlayer;
        public MusicControl()
        {
            loadQuestion();
        }

        protected override void loadQuestion()
        {
            musicPlayer = new MediaElement();
            musicPlayer.Source = new Uri(CurrentQuestion.getInstance().Question, UriKind.RelativeOrAbsolute);

            pageStack.Children.Add(musicPlayer);
        }

        public override void startQuestion()
        {
            musicPlayer.Play();
        }
        public override void pauseQuestion()
        {
            musicPlayer.Pause();
        }
        public override void showAnswer()
        {
            musicPlayer.Stop();
            answerLabel.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
