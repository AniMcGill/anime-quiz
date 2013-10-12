using System;
using System.Timers;
using System.Windows.Controls;
using Anime_Quiz_3.Classes;

namespace Anime_Quiz_3.Controls
{
    public sealed class MusicControl : AbstractQuestionControl
    {
        MediaElement musicPlayer;
        Timer mediaTimer;
        public MusicControl()
        {
            loadQuestion();
        }

        protected override void loadQuestion()
        {
            musicPlayer = new MediaElement();
            musicPlayer.Source = new Uri(CurrentQuestion.getInstance().Question, UriKind.RelativeOrAbsolute);
            musicPlayer.LoadedBehavior = MediaState.Manual;

            pageStack.Children.Add(musicPlayer);
        }

        public override void startQuestion()
        {
            musicPlayer.Play();
            mediaTimer = new Timer();
            mediaTimer.Interval = Properties.Settings.Default.duration * 1000;
            mediaTimer.Elapsed += mediaTimer_Elapsed;
            mediaTimer.Start();
        }
        public override void pauseQuestion()
        {
            musicPlayer.Pause();
            mediaTimer.Stop(); //can't pause timer
        }
        public override void showAnswer()
        {
            musicPlayer.Stop();
            answerLabel.Visibility = System.Windows.Visibility.Visible;
            mediaTimer.Stop();
        }

        void mediaTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(pauseQuestion, System.Windows.Threading.DispatcherPriority.Normal);
        }
    }
}
