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
        //static GameDataContext db;
        //static Table<Teams> teamsList;
        static IQueryable<TeamMembers> teamMembersList;
        public TeamEditor()
        {
            InitializeComponent();
            //db = new GameDataContext();
            populateTeamsComboBox();
            this.GotFocus += TeamEditor_GotFocus;
        }

        public void populateTeamsComboBox()
        {
            //teamsList = db.GetTable<Teams>();
            var teamNames = from team in App.teams select team.Name;
            teamComboBox.ItemsSource = teamNames;
            if (CurrentTeam.getInstance() != null)
                teamComboBox.SelectedItem = CurrentTeam.getInstance().Name;
        }
        public void populateTeamDataGrid()
        {
            teamMembersList = from teamMember in App.db.GetTable<TeamMembers>()
                              where teamMember.Teams.Name.Equals(CurrentTeam.getInstance().Name)
                              select teamMember;
            teamDataGrid.ItemsSource = teamMembersList;
            teamDataGrid.Visibility = System.Windows.Visibility.Visible;
        }

        public void saveTeams()
        {
            if (CurrentTeam.getInstance() != null)
            {
                App.db.SubmitChanges();
                var changedTeamMembers = from teamMember in App.db.GetTable<TeamMembers>()
                                         where teamMember.TeamId == 0
                                         select teamMember;
                foreach (TeamMembers changedTeamMember in changedTeamMembers)
                {
                    changedTeamMember.TeamId = CurrentTeam.getInstance().TeamId;
                    changedTeamMember.Teams = CurrentTeam.getInstance();
                }
                App.db.SubmitChanges();
                App.db.Refresh(RefreshMode.KeepChanges, App.teams);
            }
        }

        #region Event Handlers
        private void teamComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            saveTeams();
            if ((sender as ComboBox).SelectedIndex > -1)
            {
                CurrentTeam.setInstance((from team in App.teams
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
        #endregion

        #region Buttons
        private void delBtn_Click(object sender, RoutedEventArgs e)
        {
            App.db.Teams.DeleteOnSubmit(CurrentTeam.getInstance());
            var teamMembersToDelete = from teamMember in teamMembersList 
                                      where teamMember.TeamId == CurrentTeam.getInstance().TeamId 
                                      select teamMember;

            App.db.TeamMembers.DeleteAllOnSubmit(teamMembersToDelete);
            CurrentTeam.setInstance(null);

            App.db.SubmitChanges();
            App.db.Refresh(RefreshMode.KeepChanges, App.teams);
            populateTeamsComboBox();
        }
        private void renameBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentTeam.getInstance().Name = renameTextBox.Text;
            App.db.SubmitChanges();
            App.db.Refresh(RefreshMode.KeepChanges, App.teams);

            populateTeamsComboBox();
            teamComboBox.SelectedItem = renameTextBox.Text;
            renameTextBox.Text = String.Empty;
        }
        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            Teams newTeam = new Teams();
            newTeam.Name = teamTextBox.Text;
            // TODO: pre-register teams to a game?
            App.db.Teams.InsertOnSubmit(newTeam);
            App.db.SubmitChanges();
            App.db.Refresh(RefreshMode.KeepChanges, App.teams);

            populateTeamsComboBox();
            teamComboBox.SelectedItem = newTeam.Name;
            teamTextBox.Text = String.Empty;
        }

        void TeamEditor_GotFocus(object sender, RoutedEventArgs e)
        {
            closeBtn.IsEnabled = this.NavigationService.CanGoBack;
        }
        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            saveTeams();
            this.NavigationService.GoBack();
        }

        #endregion
    }
}
