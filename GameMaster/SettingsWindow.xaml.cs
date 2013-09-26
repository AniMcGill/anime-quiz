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

namespace Anime_Quiz_3.GameMaster
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();

            defaultMusicFolder.Text = Properties.Settings.Default.defaultMusicFolder;
            defaultMusicFolderLabel.Visibility = Properties.Settings.Default.defaultMusicFolder.Equals(String.Empty) ? Visibility.Visible : Visibility.Hidden;
            defaultScreenshotFolder.Text = Properties.Settings.Default.defaultScreenshotFolder;
            
            autoplayMusicCheckbox.IsChecked = Properties.Settings.Default.autoplay;
            
            durationSlider.Value = Properties.Settings.Default.duration;
            durationValue.Content = Properties.Settings.Default.duration;
        }

        private void defaultMusicFolderButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void defaultScreenshotFolderButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void autoplayMusicCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.autoplay = (bool) (sender as CheckBox).IsChecked;
        }

        private void durationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
