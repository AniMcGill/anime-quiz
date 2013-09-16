using System.Linq;
using System.Windows.Controls;
using Anime_Quiz_3.Controls;
using GameContext;

namespace Anime_Quiz_3.Scoring
{
    /// <summary>
    /// Interaction logic for IndividualScores.xaml
    /// </summary>
    public partial class IndividualScores : Page
    {

        public IndividualScores()
        {
            InitializeComponent();
            displayScores();
        }

        public void displayScores()
        {
            foreach (Teams team in App.teams.OrderByDescending(t => t.Score))
            {
                foreach (TeamMembers teamMember in team.TeamMembers.OrderByDescending(m => m.MemberScore))
                {
                    Score teamMemberScore = new Score();
                    teamMemberScore.ScoreName = teamMember.MemberName + " (" + team.Name + ")";
                    teamMemberScore.Points = teamMember.MemberScore;

                    pageStack.Children.Add(teamMemberScore);
                }
            }
        }
    }
}
