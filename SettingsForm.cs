using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Anime_Quiz.Properties;

namespace Anime_Quiz
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            defaultFolderBox.Text = Settings.Default.defaultFolder;
            musicFolderTextBox.Text = Settings.Default.defaultMusicFolder;
            screenshotFolderTextBox.Text = Settings.Default.defaultScreenshotFolder;
            reloadCheckBox.Checked = Settings.Default.reloadPrevious;
            useScoreSystemCheckbox.Checked = Settings.Default.useScoreSystem;

            autostartMusicBtn.Checked = Settings.Default.autostartSong;
            volumeBar.Value = Settings.Default.defaultVolume;
            songDuration.Text = Settings.Default.songDuration.ToString();
        }


        #region Folders
        private void browseBtn_Click(object sender, EventArgs e)
        {
            if (defaultFolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                defaultFolderBox.Text = defaultFolderBrowserDialog.SelectedPath;
            }
        }
        private void musicFolderBrowseBtn_Click(object sender, EventArgs e)
        {
            if (defaultFolderBrowserDialog.ShowDialog() == DialogResult.OK)
                musicFolderTextBox.Text = defaultFolderBrowserDialog.SelectedPath;
        }
        private void screenshotFolderBrowseBtn_Click(object sender, EventArgs e)
        {
            if (defaultFolderBrowserDialog.ShowDialog() == DialogResult.OK) 
                screenshotFolderTextBox.Text = defaultFolderBrowserDialog.SelectedPath;
            
        }
        #endregion
        //Deprecated methods
        /*private void defaultFolderCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox caller = sender as CheckBox;
            if (caller.Checked && Settings.Default.defaultFolder == String.Empty)
            {
                if (defaultFolderBrowserDialog.ShowDialog() == DialogResult.OK) 
                    defaultFolderBox.Text = defaultFolderBrowserDialog.SelectedPath;
            }
            else defaultFolderBox.Text = String.Empty;
        }
        private void reloadCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox caller = sender as CheckBox;
            Settings.Default.reloadPrevious = caller.Checked;
        }
        private void autostartMusicBtn_Click(object sender, EventArgs e)
        {
            CheckBox caller = sender as CheckBox;
            Settings.Default.autostartSong = caller.Checked;
        }*/

        private void num_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                e.Handled = true;
        }
        //Save the settings
        private void okBtn_Click(object sender, EventArgs e)
        {
            Settings.Default.defaultFolder = defaultFolderBox.Text;
            Settings.Default.defaultMusicFolder = musicFolderTextBox.Text;
            Settings.Default.defaultScreenshotFolder = screenshotFolderTextBox.Text;
            Settings.Default.reloadPrevious = reloadCheckBox.Checked;
            Settings.Default.useScoreSystem = useScoreSystemCheckbox.Checked;

            Settings.Default.autostartSong = autostartMusicBtn.Checked;
            Settings.Default.defaultVolume = volumeBar.Value;
            Settings.Default.songDuration = Convert.ToInt32(songDuration.Text);
            this.Close();
        }
        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
