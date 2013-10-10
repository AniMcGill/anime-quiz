using Anime_Quiz_3.Classes;
using CheckerboardImage;

namespace Anime_Quiz_3.Controls
{
    public sealed class ScreenshotControl : AbstractQuestionControl
    {
        Checkerboard screenshot;
        public ScreenshotControl()
        {
            loadQuestion();
        }
        protected override void loadQuestion()
        {
            screenshot = new Checkerboard();
            screenshot.ImageSource = CurrentQuestion.getInstance().Question;
            screenshot.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            screenshot.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            // Convert the duration to milliseconds, then divide by number of checkerboard boxes.
            screenshot.Interval = Properties.Settings.Default.duration * 1000 / 100;

            pageStack.Children.Add(screenshot);
        }
        public override void startQuestion()
        {
            screenshot.startImageAnimation();
        }
        public override void pauseQuestion()
        {
            screenshot.pauseImageAnimation();
        }

        public override void showAnswer()
        {
            screenshot.revealImage();
            answerLabel.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
