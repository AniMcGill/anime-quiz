using System.Linq;
using System.Windows.Controls;
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
        static Table<TeamScores> teamScores;
        public TeamScoresView()
        {
            InitializeComponent();
            db = new GameDataContext();
            teamScores = db.GetTable<TeamScores>();
            teamScores.OrderBy(t => t.Score);
        }
    }
}
