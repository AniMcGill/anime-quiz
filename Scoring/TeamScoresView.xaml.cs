using System.Linq;
using System.Windows.Controls;
using Anime_Quiz_3.Classes;
using Devart.Data.Linq;
using GameContext;

namespace Anime_Quiz_3.Scoring
{
    /// <summary>
    /// Interaction logic for TeamScoresView.xaml
    /// </summary>
    public partial class TeamScoresView : Page
    {
        static GameDataContext db;
        static IQueryable<TeamScores> teamScores;
        public TeamScoresView()
        {
            InitializeComponent();
            db = new GameDataContext();
            teamScores = db.GetTable<TeamScores>();
            teamScores = from teamScore in db.GetTable<TeamScores>()
                         where teamScore.Teams.Equals(CurrentTeam.getInstance())
                         select teamScore;
            teamScores.OrderBy(t => t.Score);
        }

        void displayScores()
        {
            
        }
    }
}
