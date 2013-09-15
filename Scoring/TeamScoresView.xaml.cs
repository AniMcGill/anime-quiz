using System.Linq;
using System.Windows.Controls;
using Anime_Quiz_3.Controls;
using GameContext;

namespace Anime_Quiz_3.Scoring
{
    /// <summary>
    /// Interaction logic for TeamScoresView.xaml
    /// </summary>
    public partial class TeamScoresView : Page
    {
        public TeamScoresView()
        {
            InitializeComponent();
            displayScores();  
        }

        void displayScores()
        {
            foreach (Teams team in App.teams.OrderByDescending(t => t.Score))
            {
                Score teamScore = new Score();
                teamScore.ScoreName = team.Name;
                teamScore.Points = team.Score;

                pageStack.Children.Add(teamScore);
            }
        }
    }
}
