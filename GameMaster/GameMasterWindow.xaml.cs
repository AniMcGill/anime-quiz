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
using System.Windows.Shapes;
using Anime_Quiz_3.Classes;
using Anime_Quiz_3.GameMaster;

namespace Anime_Quiz_3
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class GameMasterWindow : Window
    {
        public GameMasterWindow()
        {
            InitializeComponent();
        }

        private void helpMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var helpWindow = new HelpWindow();
            helpWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            helpWindow.Show();
        }

        private void exitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (SoundMessageBox.Show("Really exit? Realllllllly exit?", "*・゜:*☆・゜:*(*ノ｀v')ノ||EXIT||", MessageBoxButton.YesNo, Anime_Quiz_3.Properties.Resources.w_lulu) == MessageBoxResult.No
                && !e.Cancel)
                e.Cancel = true;
            base.OnClosing(e);
        }
    }
}
