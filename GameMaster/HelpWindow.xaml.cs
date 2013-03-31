using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Anime_Quiz_3.GameMaster
{
    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        SoundPlayer soundPlayer;
        public HelpWindow()
        {
            InitializeComponent();
            soundPlayer = new SoundPlayer(Anime_Quiz_3.Properties.Resources.Muda_long);
            soundPlayer.Play();
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            soundPlayer.Stop();
            soundPlayer.Dispose();
            base.OnClosing(e);
        }
    }
}
