using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Anime_Quiz_3.Classes;
using Anime_Quiz_3.Controls;
using Anime_Quiz_3.Player;
using Devart.Data.Linq;
using GameContext;

namespace Anime_Quiz_3.GameMaster
{
    /// <summary>
    /// Interaction logic for GameStartPage.xaml
    /// </summary>
    public partial class GameStartPage : Page
    {
        public static GameDataContext db;
        static Table<Games> games;
        public static IQueryable<QuestionSets> questionSets;
        static IQueryable<Teams> teams;

        PlayerWindow playerWindow;

        public GameStartPage()
        {
            InitializeComponent();
            db = new GameDataContext();
            
            getGames();
        }

        private void getGames()
        {
            games = db.GetTable<Games>();
            IEnumerable<String> gameNames = from game in games select game.Name;
            gameComboBox.ItemsSource = gameNames;
            if (CurrentGame.getInstance() != null)
                gameComboBox.SelectedItem = CurrentGame.getInstance().Name;
        }
        private void getQuestionSets()
        {
            questionSets = from questionSet in db.GetTable<QuestionSets>()
                           where questionSet.GameId.Equals(CurrentGame.getInstance().GameId)
                           select questionSet;
            IEnumerable<String> questionSetNames = from questionSet in questionSets select questionSet.Name;
            questionSetComboBox.ItemsSource = questionSetNames;
            if (CurrentQuestionSet.getInstance() != null)
                questionSetComboBox.SelectedItem = CurrentQuestionSet.getInstance().Name;
        }
        private void getTeams()
        {
            teams = from team in db.GetTable<Teams>()
                    where team.GameId.Equals(CurrentGame.getInstance().GameId)
                    select team;
        }
        private void loadTeams()
        {
            foreach (Teams team in teams)
            {
                Separator separator = new Separator();
                ScoringControl teamScoringControl = new ScoringControl();
                teamScoringControl.Text = "Team " + team.Name;
                teamScoringControl.IsTeam = true;
                teamScoringControl.RemoveButtonClicked += (sender, args) =>
                    teamScoringControl_RemoveButtonClicked(team.TeamId, args);

                teamsStackPanel.Children.Add(teamScoringControl);
                teamsStackPanel.Children.Add(separator);

                IQueryable<TeamMembers> teamMembers = from teamMember in db.GetTable<TeamMembers>()
                                                      where teamMember.TeamId.Equals(team.TeamId)
                                                      select teamMember;
                foreach (TeamMembers teamMember in teamMembers)
                {
                    ScoringControl teamMemberScoringControl = new ScoringControl();
                    teamMemberScoringControl.Text = teamMember.Name;
                    teamMemberScoringControl.AddButtonClicked += (sender, args) =>
                        teamMemberScoringControl_AddButtonClicked(teamMember.MemberId, teamMember.TeamId, args);
                    teamMemberScoringControl.RemoveButtonClicked += (sender,args) =>
                        teamMemberScoringControl_RemoveButtonClicked(teamMember.MemberId, teamMember.TeamId, args);

                    teamsStackPanel.Children.Add(teamMemberScoringControl);
                }
                Separator endSeparator = new Separator();
                endSeparator.Margin = new Thickness(0, 0, 0, 20);
                teamsStackPanel.Children.Add(endSeparator);
            }
        }
        private void initializeScores()
        {
            getTeams();
            foreach (Teams team in teams)
            {
                // Create team score entry
                TeamScores teamScore = new TeamScores();
                teamScore.TeamId = team.TeamId;
                teamScore.GameId = CurrentGame.getInstance().GameId;
                db.TeamScores.InsertOnSubmit(teamScore);

                //Create individual score entry
                IQueryable<TeamMembers> teamMembers = from teamMember in db.GetTable<TeamMembers>()
                                                      where teamMember.TeamId.Equals(team.TeamId)
                                                      select teamMember;
                foreach (TeamMembers teamMember in teamMembers)
                {
                    Scores score = new Scores();
                    score.MemberId = teamMember.MemberId;
                    score.GameId = CurrentGame.getInstance().GameId;
                    db.Scores.InsertOnSubmit(score);
                }
            }
            db.SubmitChanges();
        }
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
        private void gameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentGame.setInstance((from game in games
                                     where game.Name.Equals((sender as ComboBox).SelectedValue.ToString())
                                     select game).Single());
            getQuestionSets();
            questionSetComboBox.IsEnabled = gameComboBox.SelectedIndex >= 0;
            loadGameBtn.IsEnabled = gameComboBox.SelectedIndex >= 0;
        }
        private void loadGameBtn_Click(object sender, RoutedEventArgs e)
        {
            loadGameBtn.IsEnabled = false;
            initializeScores();
        }

        private void questionSetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentQuestionSet.setInstance((from questionSet in questionSets
                                            where questionSet.Name.Equals((sender as ComboBox).SelectedValue.ToString())
                                            select questionSet).Single());
            questionSetLoadBtn.IsEnabled = questionSetComboBox.SelectedIndex >= 0 && !loadGameBtn.IsEnabled;
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


        void teamScoringControl_RemoveButtonClicked(int teamId, EventArgs e)
        {
            try
            {
                TeamScores teamScore = (from team in db.GetTable<TeamScores>()
                                        where team.TeamId.Equals(teamId)
                                        select team).Single();
                teamScore.Score -= CurrentQuestion.getInstance().Points;
                db.SubmitChanges();
            }
            catch (NullReferenceException crap)
            {
                SoundMessageBox.Show("No Question has been loaded", "Fail", Anime_Quiz_3.Properties.Resources.Muda);
            }
        }
        void teamMemberScoringControl_AddButtonClicked(int teamMemberId, int teamId, EventArgs e)
        {
            try
            {
                Scores teamMemberScore = (from teamMember in db.GetTable<Scores>()
                                          where teamMember.MemberId.Equals(teamMemberId)
                                          select teamMember).Single();
                teamMemberScore.Score += CurrentQuestion.getInstance().Points;

                TeamScores teamScore = (from team in db.GetTable<TeamScores>()
                                        where team.TeamId.Equals(teamId)
                                        select team).Single();
                teamScore.Score += CurrentQuestion.getInstance().Points;
                db.SubmitChanges();
                playerWindow.showAnswer();
            }
            catch (NullReferenceException crap)
            {
                SoundMessageBox.Show("No Question has been loaded", "Fail", Anime_Quiz_3.Properties.Resources.Muda);
            }
        }
        void teamMemberScoringControl_RemoveButtonClicked(int teamMemberId, int teamId, EventArgs e)
        {
            try
            {
                Scores teamMemberScore = (from teamMember in db.GetTable<Scores>()
                                          where teamMember.MemberId.Equals(teamMemberId)
                                          select teamMember).Single();
                teamMemberScore.Score -= CurrentQuestion.getInstance().Points;

                TeamScores teamScore = (from team in db.GetTable<TeamScores>()
                                        where team.TeamId.Equals(teamId)
                                        select team).Single();
                teamScore.Score -= CurrentQuestion.getInstance().Points;
                db.SubmitChanges();
            }
            catch (NullReferenceException crap)
            {
                SoundMessageBox.Show("No Question has been loaded", "Fail", Anime_Quiz_3.Properties.Resources.Muda);
            }
        }
        #endregion
    }
}
