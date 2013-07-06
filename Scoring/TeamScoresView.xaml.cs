using System.Linq;
using System.Windows.Controls;
using Anime_Quiz_3.Classes;
using Anime_Quiz_3.GameMaster;
using GameContext;

namespace Anime_Quiz_3.Scoring
{
    /// <summary>
    /// Interaction logic for TeamScoresView.xaml
    /// </summary>
    public partial class TeamScoresView : Page
    {
        IQueryable<TeamScores> teamScores;
        public TeamScoresView()
        {
            InitializeComponent();
            getScores();   
        }
        void getScores()
        {
            teamScores = from teamScore in GameStartPage.db.GetTable<TeamScores>()
                         where teamScore.TeamId.Equals(CurrentTeam.getInstance().TeamId) &&
                                teamScore.GameId.Equals(CurrentGame.getInstance().GameId)
                         select teamScore;
            teamScores.OrderBy(t => t.Score);
        }

        void displayScores()
        {
            foreach (TeamScores teamScore in teamScores)
            {
                Label teamNameLabel = new Label();
                teamNameLabel.Content = teamScore.Teams.Name;
                teamNameLabel.Margin = new System.Windows.Thickness(0, 20, 0, 0);
                Label teamScoreLabel = new Label();
                teamScoreLabel.Content = teamScore.Score;

                pageStack.Children.Add(teamNameLabel);
                pageStack.Children.Add(teamScoreLabel);
            }
        }
    }
}
