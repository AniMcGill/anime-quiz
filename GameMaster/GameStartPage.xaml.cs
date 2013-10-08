using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Anime_Quiz_3.Classes;
using Anime_Quiz_3.Controls;
using Anime_Quiz_3.Player;
using GameContext;

namespace Anime_Quiz_3.GameMaster
{
    /// <summary>
    /// Interaction logic for GameStartPage.xaml
    /// </summary>
    public partial class GameStartPage : Page
    {
        PlayerWindow playerWindow;

        public GameStartPage()
        {
            InitializeComponent();
            
            getQuestionSets();
        }

        private void getQuestionSets()
        {
            var questionSetNames = from questionSet in App.questionSets select questionSet.Name;
            questionSetComboBox.ItemsSource = questionSetNames;
            if (CurrentQuestionSet.getInstance() != null)
                questionSetComboBox.SelectedItem = CurrentQuestionSet.getInstance().Name;
        }
        
        void loadTeams()
        {
            foreach (Teams team in App.teams)
            {
                Separator separator = new Separator();
                ScoringControl teamScoringControl = new ScoringControl();
                teamScoringControl.Text = "Team " + team.Name;
                teamScoringControl.AddButtonClicked += (sender, args) =>
                    teamScoringControl_AddButtonClicked(team.TeamId, args);
                teamScoringControl.IsTeam = true;

                teamsStackPanel.Children.Add(teamScoringControl);
                teamsStackPanel.Children.Add(separator);

                IQueryable<TeamMembers> teamMembers = from teamMember in App.db.GetTable<TeamMembers>()
                                                      where teamMember.TeamId.Equals(team.TeamId)
                                                      select teamMember;
                foreach (TeamMembers teamMember in teamMembers)
                {
                    ScoringControl teamMemberScoringControl = new ScoringControl();
                    teamMemberScoringControl.Text = teamMember.MemberName;
                    teamMemberScoringControl.AddButtonClicked += (sender, args) =>
                        teamMemberScoringControl_AddButtonClicked(teamMember.MemberId, args);

                    teamsStackPanel.Children.Add(teamMemberScoringControl);
                }
                Separator endSeparator = new Separator();
                endSeparator.Margin = new Thickness(0, 0, 0, 20);
                teamsStackPanel.Children.Add(endSeparator);
            }
        }      
        /// <summary>
        ///     Toggles the visibility of the Current Question information section
        /// </summary>
        /// <param name="show">True if visible; False otherwise</param>
        void toggleQuestionInfo(bool show)
        {
            showAnswerBtn.IsEnabled = show;
            closeQuestionBtn.IsEnabled = show;
            if (show)
                currentQuestionStack.Visibility = System.Windows.Visibility.Visible;
            else
                currentQuestionStack.Visibility = System.Windows.Visibility.Collapsed;
        }
        void setQuestionInfo()
        {
            currentQuestionAnswerLabel.Content = "Answer: " + CurrentQuestion.getInstance().Answer.ToString();
            currentQuestionPointLabel.Content = "Points: " + CurrentQuestion.getInstance().Points.ToString();
        }
        void loadAnsweringOrderStack()
        {
            answeringOrderStack.Visibility = System.Windows.Visibility.Visible;
            foreach (KeyValuePair<int, string> buzzerParam in App.buzzerParams)
            {
                BluetoothBuzzer buzzer = new BluetoothBuzzer(buzzerParam.Value, buzzerParam.Key);
                answeringOrderStack.Children.Add(buzzer);
            }
            /*
            foreach (Teams team in App.teams)
            {
                Label teamLabel = new Label();
                teamLabel.Name = "team" + team.TeamId;
                teamLabel.Content = team.Name;
                teamLabel.Visibility = System.Windows.Visibility.Collapsed;
                answeringOrderStack.Children.Add(teamLabel);
            }*/
        }
        void listenForAnsweringOrder()
        {
            foreach (BluetoothBuzzer buzzer in answeringOrderStack.Children.OfType<BluetoothBuzzer>())
            {
                buzzer.BuzzerStandby();
            }
        }
        void resetAnsweringOrder()
        {
            foreach (BluetoothBuzzer buzzer in answeringOrderStack.Children.OfType<BluetoothBuzzer>())
            {
                buzzer.BuzzerStop();
            }
            /*
            foreach (Label label in answeringOrderStack.Children.OfType<Label>())
            {
                if (label.Name != answeringOrderTitle.Name)
                    label.Visibility = System.Windows.Visibility.Collapsed;
            }*/
        }
        /*
        void showAnsweringOrderLabel(int teamId)
        {
            (answeringOrderStack.FindName("team" + teamId) as Label).Visibility = System.Windows.Visibility.Visible;
        }*/

        #region Event Handlers
        public EventHandler ScoreUpdated;
        protected virtual void OnScoreUpdated(EventArgs e)
        {
            EventHandler handler = ScoreUpdated;
            if (handler != null)
                handler(this, e);
        }
        private void questionSetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentQuestionSet.setInstance((from questionSet in App.questionSets
                                            where questionSet.Name.Equals((sender as ComboBox).SelectedValue.ToString())
                                            select questionSet).Single());
            questionSetLoadBtn.IsEnabled = questionSetComboBox.SelectedIndex >= 0;
        }
       
        /// <summary>
        ///     Refresh the Player Window when a new set is loaded,
        ///     or set up the windows if this is the first set we load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void questionSetLoadBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                playerWindow.Refresh();
            }
            catch
            {
                playerWindow = new PlayerWindow();
                playerWindow.QuestionReady += playerWindow_QuestionReady;
                playerWindow.Show();

                loadTeams();
                loadAnsweringOrderStack();
            }
        }
        private void showAnswerBtn_Click(object sender, RoutedEventArgs e)
        {
            playerWindow.showAnswer();
            CurrentQuestion.getInstance().Answered = true;
            App.db.SubmitChanges();
            App.refreshDb(CurrentQuestion.getInstance());
        }

        private void closeQuestionBtn_Click(object sender, RoutedEventArgs e)
        {
            toggleQuestionInfo(false);
            playerWindow.Refresh();
            CurrentQuestion.setInstance(null);
        }

        void playerWindow_QuestionReady(object sender, EventArgs e)
        {
            toggleQuestionInfo(true);
            setQuestionInfo();

            resetAnsweringOrder(); //TODO: this should be used elsewhere...
            //TODO: start serial listenner
            listenForAnsweringOrder();
        }

        /// <summary>
        ///     Add 100 to the score of each other team
        /// </summary>
        /// <param name="teamId">The Id of the team to exclude</param>
        /// <param name="e"></param>
        void teamScoringControl_AddButtonClicked(int teamId, EventArgs e)
        {
            foreach (Teams team in App.teams)
            {
                if (team.TeamId != teamId)
                {
                    team.Score += 100;
                }
            }

            App.db.SubmitChanges();
            App.refreshDb(App.teams);

            OnScoreUpdated(EventArgs.Empty);
        }

        void teamMemberScoringControl_AddButtonClicked(int teamMemberId, EventArgs e)
        {
            try
            {
                TeamMembers scoringTeamMember = (from teamMember in App.db.GetTable<TeamMembers>()
                                           where teamMember.MemberId.Equals(teamMemberId)
                                           select teamMember).Single();
                scoringTeamMember.MemberScore += CurrentQuestion.getInstance().Points;
                scoringTeamMember.Teams.Score += CurrentQuestion.getInstance().Points;

                App.db.SubmitChanges();
                App.refreshDb(App.teams);
                
                OnScoreUpdated(EventArgs.Empty);
                playerWindow.showAnswer();
            }
            catch (NullReferenceException crap)
            {
                SoundMessageBox.Show("No Question has been loaded/n/n" + crap.Message, "Fail", Anime_Quiz_3.Properties.Resources.Muda);
            }
        }
        #endregion
    }
}
