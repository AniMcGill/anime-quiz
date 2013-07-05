using System;
using System.Windows;

namespace Anime_Quiz_3.Player
{
    /// <summary>
    /// Interaction logic for PlayerWindow.xaml
    /// </summary>
    public partial class PlayerWindow : Window
    {
        public PlayerWindow()
        {
            InitializeComponent();
            _playerFrame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
            loadQuestionSet();
        }

        private void loadQuestionSet()
        {
            QuestionSetPage questionSetPage = new QuestionSetPage();
            questionSetPage.QuestionSelected += questionSetPage_QuestionSelected;
            _playerFrame.NavigationService.Content = questionSetPage;
        }

        /// <summary>
        ///     When a Question has been selected, collapse the leaderboard and prepare the Question.
        /// </summary>
        void questionSetPage_QuestionSelected(object sender, EventArgs e)
        {
            _leaderboardFrame.Visibility = System.Windows.Visibility.Collapsed;
            QuestionPage questionPage = new QuestionPage();
            _playerFrame.NavigationService.Navigate(questionPage);
            OnQuestionReady(EventArgs.Empty);
        }

        public event EventHandler QuestionReady;
        protected virtual void OnQuestionReady(EventArgs e)
        {
            EventHandler handler = QuestionReady;
            if (handler != null)
                handler(this, e);
        }

        public void Refresh()
        {
            //_playerFrame.NavigationService.Refresh();
            loadQuestionSet();
            if (_leaderboardFrame.Visibility != System.Windows.Visibility.Visible)
                _leaderboardFrame.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
