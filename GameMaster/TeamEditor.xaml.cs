using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Anime_Quiz_3.Classes;
using Devart.Data.Linq;
using GameContext;

namespace Anime_Quiz_3.GameMaster
{
    /// <summary>
    /// Interaction logic for TeamEditor.xaml
    /// </summary>
    public partial class TeamEditor : Page
    {
        static GameDataContext db;
        static Table<Teams> teamsList;
        static IQueryable<TeamMembers> teamMembersList;
        public TeamEditor()
        {
            InitializeComponent();
            db = new GameDataContext();
            populateGamesComboBox();
            populateTeamsComboBox();
        }

        public void populateGamesComboBox()
        {
            var gameNames = from game in db.GetTable<Games>()
                            select game.Name;
            gameComboBox.ItemsSource = gameNames;
            if (CurrentGame.getInstance() != null)
                gameComboBox.SelectedItem = CurrentGame.getInstance().Name;
        }
        public void populateTeamsComboBox()
        {
            teamsList = db.GetTable<Teams>();
            var teamNames = from team in teamsList select team.Name;
            teamComboBox.ItemsSource = teamNames;
            if (CurrentTeam.getInstance() != null)
                teamComboBox.SelectedItem = CurrentTeam.getInstance().Name;
        }
        public void populateTeamDataGrid()
        {
            teamMembersList = from teamMember in db.GetTable<TeamMembers>()
                              where teamMember.Teams.Name.Equals(CurrentTeam.getInstance().Name)
                              select teamMember;
            teamDataGrid.ItemsSource = teamMembersList;
            teamDataGrid.Visibility = System.Windows.Visibility.Visible;
        }

        public void saveTeams()
        {
            if (CurrentTeam.getInstance() != null)
            {
                db.SubmitChanges();
                var changedTeamMembers = from teamMember in db.GetTable<TeamMembers>()
                                         where teamMember.TeamId == 0
                                         select teamMember;
                foreach (TeamMembers changedTeamMember in changedTeamMembers)
                {
                    changedTeamMember.TeamId = CurrentTeam.getInstance().TeamId;
                    changedTeamMember.Teams = CurrentTeam.getInstance();
                }
                db.SubmitChanges();
            }
        }

        #region Event Handlers
        private void teamComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            saveTeams();
            if ((sender as ComboBox).SelectedIndex > -1)
            {
                CurrentTeam.setInstance((from team in teamsList
                                         where team.Name.Equals((sender as ComboBox).SelectedValue.ToString())
                                         select team).Single());
                populateTeamDataGrid();
            }
            else
            {
                CurrentTeam.setInstance(null);
                teamDataGrid.Visibility = System.Windows.Visibility.Collapsed;
            }
            delBtn.IsEnabled = (sender as ComboBox).SelectedIndex > -1;
        }

        private void teamTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            addBtn.IsEnabled = (sender as TextBox).Text.Length > 0;
        }

        private void renameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            renameBtn.IsEnabled = teamComboBox.SelectedIndex > -1 && (sender as TextBox).Text.Length > 0;
        }
        private void gameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            registerTeamBtn.IsEnabled = (sender as ComboBox).SelectedIndex > -1;
        }
        #endregion

        #region Buttons
        private void delBtn_Click(object sender, RoutedEventArgs e)
        {
            db.Teams.DeleteOnSubmit(CurrentTeam.getInstance());
            var teamMembersToDelete = from teamMember in teamMembersList 
                                      where teamMember.TeamId == CurrentTeam.getInstance().TeamId 
                                      select teamMember;
            var teamScoresToDelete = from teamScore in db.GetTable<TeamScores>()
                                     where teamScore.TeamId == CurrentTeam.getInstance().TeamId
                                     select teamScore;
            foreach (TeamMembers teamMemberToDelete in teamMembersToDelete)
            {
                var scoresToDelete = from score in db.GetTable<Scores>()
                                    where score.MemberId == teamMemberToDelete.MemberId
                                    select score;
                db.Scores.DeleteAllOnSubmit(scoresToDelete);    //might not be possible to delete teams
            }

            db.TeamMembers.DeleteAllOnSubmit(teamMembersToDelete);
            db.TeamScores.DeleteAllOnSubmit(teamScoresToDelete);
            CurrentTeam.setInstance(null);

            db.SubmitChanges();
            populateTeamsComboBox();
        }
        private void renameBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentTeam.getInstance().Name = renameTextBox.Text;
            db.SubmitChanges();

            populateTeamsComboBox();
            teamComboBox.SelectedItem = renameTextBox.Text;
            renameTextBox.Text = String.Empty;
        }
        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            Teams newTeam = new Teams();
            newTeam.Name = teamTextBox.Text;
            // TODO: pre-register teams to a game?
            db.Teams.InsertOnSubmit(newTeam);
            db.SubmitChanges();

            populateTeamsComboBox();
            teamComboBox.SelectedItem = newTeam.Name;
            teamTextBox.Text = String.Empty;
        }
        private void registerTeamBtn_Click(object sender, RoutedEventArgs e)
        {
            Games registeredGame = (from game in db.GetTable<Games>()
                                    where game.Name.Equals(gameComboBox.SelectedValue.ToString())
                                    select game).Single();
            if (CurrentTeam.getInstance() != null)
                CurrentTeam.getInstance().Games = registeredGame;
            db.SubmitChanges();
        }
        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            saveTeams();
            if (this.NavigationService.CanGoBack)
                this.NavigationService.GoBack();
        }

        #endregion
    }
}
