using System;
using System.Windows;
using Anime_Quiz_3.Classes;
using Anime_Quiz_3.GameMaster;
using Anime_Quiz_3.Scoring;

namespace Anime_Quiz_3
{
    /// <summary>
    /// Interaction logic for GameMasterWindow.xaml
    /// </summary>
    public partial class GameMasterWindow : Window
    {
        public GameMasterWindow()
        {
            InitializeComponent();
            _gmWindow.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
            _sideWindow.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
        }

        private void loadLeaderboard()
        {
            _sideWindow.NavigationService.Navigate(new IndividualScores());
            _sideWindow.Visibility = System.Windows.Visibility.Visible;
        }

        void gameStartPage_ScoreUpdated(object sender, EventArgs e)
        {
            loadLeaderboard();
        }

        #region Menu
        private void startGameMenuItem_Click(object sender, RoutedEventArgs e)
        {
            GameStartPage gameStartPage = new GameStartPage();
            gameStartPage.ScoreUpdated += gameStartPage_ScoreUpdated;
            _gmWindow.NavigationService.Navigate(gameStartPage);
            loadLeaderboard();
        }
        private void createQuestionSetMenuItem_Click(object sender, RoutedEventArgs e)
        {
            _gmWindow.NavigationService.Navigate(new QuestionSetEditor());
            _sideWindow.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void teamsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            _gmWindow.NavigationService.Navigate(new TeamEditor());
            _sideWindow.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void buzzerMenuItem_Click(object sender, RoutedEventArgs e)
        {
            BuzzerSetupWindow buzzerSetupWindow = new BuzzerSetupWindow();
            buzzerSetupWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            buzzerSetupWindow.Show();
        }

        private void settingsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingWindow = new SettingsWindow();
            settingWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            settingWindow.Show();
        }

        private void helpMenuItem_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow helpWindow = new HelpWindow();
            helpWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            helpWindow.Show();
        }
        private void exitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (SoundMessageBox.Show("Really exit? Realllllllly exit?", "*・゜:*☆・゜:*(*ノ｀v')ノ||EXIT||", MessageBoxButton.YesNo, Anime_Quiz_3.Properties.Resources.w_lulu) == MessageBoxResult.No
                && !e.Cancel)
                e.Cancel = true;
            base.OnClosing(e);
        }
    }
}
