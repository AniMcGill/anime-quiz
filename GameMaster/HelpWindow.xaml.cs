using System.Media;
using System.Windows;

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
