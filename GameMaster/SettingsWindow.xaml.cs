using System;
using System.Windows;
using Anime_Quiz_3.Classes;

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
            defaultScreenshotFolderLabel.Visibility = Properties.Settings.Default.defaultScreenshotFolder.Equals(String.Empty) ? Visibility.Visible : Visibility.Hidden;

            autoplayMusicCheckbox.IsChecked = Properties.Settings.Default.autoplay;
            
            durationSlider.Value = Properties.Settings.Default.duration;
        }

        private void defaultMusicFolderButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void defaultScreenshotFolderButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void durationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //Y U NO int? Integer division in C#?
            durationSlider.Value = e.NewValue - (e.NewValue % 1);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                Properties.Settings.Default.defaultMusicFolder = defaultMusicFolder.Text;
                Properties.Settings.Default.defaultScreenshotFolder = defaultScreenshotFolder.Text;
                Properties.Settings.Default.autoplay = (bool) autoplayMusicCheckbox.IsChecked;
                Properties.Settings.Default.duration = Int32.Parse(durationSlider.Value.ToString());
                base.OnClosing(e);
            }
            catch (Exception crap)
            {
                SoundMessageBox.Show("There's a mistake in the settings, fix it or you'll regret it later\n" + crap.Message, 
                    "Fail",
                    Properties.Resources.w_lulu);
            }
        }
    }
}
