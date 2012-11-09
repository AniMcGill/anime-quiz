using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Anime_Quiz.DataModel;
using Anime_Quiz.Properties;
//using System.Globalization;
//using System.Threading;


namespace Anime_Quiz
{
    public partial class GameBoard : Form
    {
        //Forms
        GameForm gameForm = new GameForm();

        public GameBoard()
        {
            InitializeComponent();
            //Localization test
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            Settings.Default.saveState = true;
            
            gameForm.MdiParent = this;
            gameForm.Show();
        }

        #region Behaviors
        //If we can't display recent files, remove this method and its references altogether
        /*private void updateRecentFiles()
        {
            //Add the file to the list of recent files
            if (Settings.Default.recentFiles.Contains(Settings.Default.currentFile))
                Settings.Default.recentFiles.Remove(Settings.Default.currentFile);
            //Remove the oldest file if the list is full
            else if (Settings.Default.recentFiles.Count == 10) Settings.Default.recentFiles.RemoveAt(0);
            Settings.Default.recentFiles.Add(Settings.Default.currentFile);
        }*/
        private bool isSafeOverwrite(string message)
        {
            if (!Settings.Default.saveState)
            {
                DialogResult confirm = new DialogResult();
                confirm = MessageBox.Show(message, "Unsaved changes", MessageBoxButtons.YesNoCancel);
                if (confirm == DialogResult.Cancel) return false;
                else if (confirm == DialogResult.Yes) gameForm.saveBehavior();
            }
            return true;
        }
        #endregion

        #region FileMenu
        private void createGameToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            //Save in progress game and open editor
            if (isSafeOverwrite("There is a game in progress. Do you want to save it before proceeding?"))
            {
                GameEditor newGame = new GameEditor();
                //Set the form in a new Window (on dual screens, it will be on the monitor 1
                newGame.ShowDialog();
                //Settings.Default.saveState = true;
                //Once we are done editing, if there is a currentFile, load it.
                //Otherwise clear the game board.
                if (Settings.Default.currentFile != String.Empty && gameForm.loadFileBehavior(Settings.Default.currentFile))
                    gameForm.loadGameBehavior();
                else gameForm.clearGamePanel();
            }
        }
        private void loadGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog gameLoad = new OpenFileDialog();
            if (Settings.Default.defaultFolder != String.Empty) gameLoad.InitialDirectory = Settings.Default.defaultFolder;
            gameLoad.Filter = "XML files|*xml";
            if (gameLoad.ShowDialog() == DialogResult.OK && gameForm.loadFileBehavior(gameLoad.FileName))
                gameForm.loadGameBehavior();
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.Default.Save();
            //if(MessageBox.Show(CultureInfo.CurrentUICulture.Name.Equals("ja-JP")?"ゲームを終了します。よろしいですか？":"Are you sure you want to quit?",
            //    CultureInfo.CurrentUICulture.Name.Equals("ja-JP") ? "( TДT)" : "You are making me sad", MessageBoxButtons.OKCancel) == DialogResult.OK)
            if (MessageBox.Show("Are you sure you want to quit?", "You make me sad Eri-chan", MessageBoxButtons.OKCancel) == DialogResult.OK)
                Application.Exit();
        }
        #endregion

        #region ViewMenu
        private void fullToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Bounds = Screen.FromControl(this).Bounds;
            this.TopMost = true;
            this.mainMenu.Visible = true;
        }
        private void windowedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.mainMenu.Visible = true;
        }

        #endregion

        #region SettingsMenu
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm settings = new SettingsForm();
            settings.Show();
        }
        #endregion

        #region HelpMenu
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help helpForm = new Help();
            helpForm.Show();
        }
        #endregion

    }
}
