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
