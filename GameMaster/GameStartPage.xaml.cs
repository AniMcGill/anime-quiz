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
        public static GameDataContext db;
        public static IQueryable<QuestionSets> questionSets;
        static IQueryable<Teams> teams;

        PlayerWindow playerWindow;

        public GameStartPage()
        {
            InitializeComponent();
            db = new GameDataContext();
            
            getQuestionSets();
            getTeams();
        }

        private void getQuestionSets()
        {
            questionSets = db.GetTable<QuestionSets>();
            IEnumerable<String> questionSetNames = from questionSet in questionSets select questionSet.Name;
            questionSetComboBox.ItemsSource = questionSetNames;
            if (CurrentQuestionSet.getInstance() != null)
                questionSetComboBox.SelectedItem = CurrentQuestionSet.getInstance().Name;
        }
        private void getTeams()
        {
            teams = db.GetTable<Teams>();
        }
        private void loadTeams()
        {
            foreach (Teams team in teams)
            {
                Separator separator = new Separator();
                ScoringControl teamScoringControl = new ScoringControl();
                teamScoringControl.Text = "Team " + team.Name;
                teamScoringControl.AddButtonClicked += (sender, args) =>
                    teamScoringControl_AddButtonClicked(team.TeamId, args);
                teamScoringControl.IsTeam = true;

                teamsStackPanel.Children.Add(teamScoringControl);
                teamsStackPanel.Children.Add(separator);

                IQueryable<TeamMembers> teamMembers = from teamMember in db.GetTable<TeamMembers>()
                                                      where teamMember.TeamId.Equals(team.TeamId)
                                                      select teamMember;
                foreach (TeamMembers teamMember in teamMembers)
                {
                    ScoringControl teamMemberScoringControl = new ScoringControl();
                    teamMemberScoringControl.Text = teamMember.MemberName;
                    teamMemberScoringControl.AddButtonClicked += (sender, args) =>
                        teamMemberScoringControl_AddButtonClicked(teamMember.MemberId, teamMember.TeamId, args);

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
        private void toggleQuestionInfo(bool show)
        {
            showAnswerBtn.IsEnabled = show;
            closeQuestionBtn.IsEnabled = show;
            if (show)
                currentQuestionStack.Visibility = System.Windows.Visibility.Visible;
            else
                currentQuestionStack.Visibility = System.Windows.Visibility.Collapsed;
        }

        #region Event Handlers

        private void questionSetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentQuestionSet.setInstance((from questionSet in questionSets
                                            where questionSet.Name.Equals((sender as ComboBox).SelectedValue.ToString())
                                            select questionSet).Single());
            questionSetLoadBtn.IsEnabled = questionSetComboBox.SelectedIndex >= 0;
        }
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

                getTeams();
                loadTeams();
            }
            /*bool playerWindowExists = false;
            foreach (Window window in Application.Current.Windows)
            {
                if (window is PlayerWindow)
                {
                    (window as PlayerWindow).Refresh();
                    playerWindowExists = true;
                    //window.Close();
                    break;
                }
            }
            if (!playerWindowExists)
            {
                playerWindow = new PlayerWindow();
                playerWindow.QuestionReady += playerWindow_QuestionReady;
                playerWindow.Show();
                
                getTeams();
                loadTeams();
            }*/
        }
        private void showAnswerBtn_Click(object sender, RoutedEventArgs e)
        {
            playerWindow.showAnswer();
            CurrentQuestion.getInstance().Answered = true;
            db.SubmitChanges();
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
            currentQuestionAnswerLabel.Content = "Answer: " + CurrentQuestion.getInstance().Answer.ToString();
            currentQuestionPointLabel.Content = "Points: " + CurrentQuestion.getInstance().Points.ToString();
            //TODO: Reset answering order
        }

        /// <summary>
        ///     Add 100 to the score of each other team
        /// </summary>
        /// <param name="teamId">The Id of the team to exclude</param>
        /// <param name="e"></param>
        void teamScoringControl_AddButtonClicked(int teamId, EventArgs e)
        {
            IEnumerable<Teams> otherTeams = from team in teams
                                             where !team.TeamId.Equals(teamId)
                                             select team;
            foreach (Teams otherTeam in otherTeams)
            {
                otherTeam.Score += 100;
            }
            db.SubmitChanges();
        }

        void teamMemberScoringControl_AddButtonClicked(int teamMemberId, int teamId, EventArgs e)
        {
            try
            {
                TeamMembers scoringTeamMember = (from teamMember in db.GetTable<TeamMembers>()
                                           where teamMember.MemberId.Equals(teamMemberId)
                                           select teamMember).Single();
                scoringTeamMember.MemberScore += CurrentQuestion.getInstance().Points;

                Teams scoringTeam = (from team in teams where teamId.Equals(teamId) select team).Single();
                scoringTeam.Score += CurrentQuestion.getInstance().Points;

                db.SubmitChanges();
                playerWindow.showAnswer();
            }
            catch (NullReferenceException crap)
            {
                SoundMessageBox.Show("No Question has been loaded", "Fail", Anime_Quiz_3.Properties.Resources.Muda);
            }
        }
        #endregion
    }
}
