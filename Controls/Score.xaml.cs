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

        public object Name
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
