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
        IQueryable<Teams> teams;
        public TeamScoresView()
        {
            InitializeComponent();
            getScores();   
        }
        void getScores()
        {
            teams = GameStartPage.db.GetTable<Teams>();
            teams.OrderBy(t => t.Score);
        }

        void displayScores()
        {
            foreach (Teams team in teams)
            {
                Label teamNameLabel = new Label();
                teamNameLabel.Content = team.Name;
                teamNameLabel.Margin = new System.Windows.Thickness(0, 20, 0, 0);
                Label teamScoreLabel = new Label();
                teamScoreLabel.Content = team.Score;

                pageStack.Children.Add(teamNameLabel);
                pageStack.Children.Add(teamScoreLabel);
            }
        }
    }
}
