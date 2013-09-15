using System;
using System.Windows.Controls;

namespace Anime_Quiz_3.Controls
{
    /// <summary>
    /// Interaction logic for Score.xaml
    /// </summary>
    public partial class Score : UserControl
    {
        public Score()
        {
            InitializeComponent();
        }

        public object ScoreName
        {
            get { return scoreLabel.Content; }
            set { scoreLabel.Content = value; }
        }

        public int Points
        {
            get { return Int32.Parse(scoreValue.Content.ToString()); }
            set { scoreValue.Content = value; }
        }
    }
}
