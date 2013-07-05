using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Anime_Quiz_3.Classes;
using Anime_Quiz_3.Properties;
using Devart.Data.Linq;
using GameContext;
using Anime_Quiz_3.Controls;
using Anime_Quiz_3.Player;

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
                teamScoringControl.Text = team.Name;
                teamScoringControl.IsTeam = true;
                teamScoringControl.RemoveButtonClicked += teamScoringControl_RemoveButtonClicked;

                teamsStackPanel.Children.Add(teamScoringControl);
                teamsStackPanel.Children.Add(separator);

                IQueryable<TeamMembers> teamMembers = from teamMember in db.GetTable<TeamMembers>()
                                                      where teamMember.TeamId.Equals(team.TeamId)
                                                      select teamMember;
                foreach (TeamMembers teamMember in teamMembers)
                {
                    ScoringControl teamMemberScoringControl = new ScoringControl();
                    teamMemberScoringControl.Text = teamMember.Name;
                    teamMemberScoringControl.AddButtonClicked += teamMemberScoringControl_AddButtonClicked;
                    teamMemberScoringControl.RemoveButtonClicked += teamMemberScoringControl_RemoveButtonClicked;

                    teamsStackPanel.Children.Add(teamMemberScoringControl);
                }
                Separator endSeparator = new Separator();
                endSeparator.Margin = new Thickness(0, 0, 0, 20);
                teamsStackPanel.Children.Add(endSeparator);
            }
        }

        #region Event Handlers
        private void gameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentGame.setInstance((from game in games
                                     where game.Name.Equals((sender as ComboBox).SelectedValue.ToString())
                                     select game).Single());
            getQuestionSets();
            questionSetComboBox.IsEnabled = gameComboBox.SelectedIndex >= 0;
        }

        private void questionSetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentQuestionSet.setInstance((from questionSet in questionSets
                                            where questionSet.Name.Equals((sender as ComboBox).SelectedValue.ToString())
                                            select questionSet).Single());
            questionSetLoadBtn.IsEnabled = questionSetComboBox.SelectedIndex >= 0;
        }

        private void questionSetLoadBtn_Click(object sender, RoutedEventArgs e)
        {
            bool playerWindowExists = false;
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
                PlayerWindow playerWindow = new PlayerWindow();
                playerWindow.QuestionReady += playerWindow_QuestionReady;
                playerWindow.Show();
                
                getTeams();
                loadTeams();
            } 
        }

        void playerWindow_QuestionReady(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public event EventHandler ShowAnswer;
        protected virtual void OnShowAnswer(EventArgs e)
        {
            EventHandler handler = ShowAnswer;
            if (handler != null)
                handler(this, e);
        }

        void teamScoringControl_RemoveButtonClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        void teamMemberScoringControl_AddButtonClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        void teamMemberScoringControl_RemoveButtonClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
