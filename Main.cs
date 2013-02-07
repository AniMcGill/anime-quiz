﻿using System;
using System.Windows.Forms;
using Anime_Quiz.Properties;
//using System.Globalization;
//using System.Threading;


namespace Anime_Quiz
{
    public partial class GameBoard : Form
    {
        //Forms
        GameForm gameForm = new GameForm();
        ScoreForm scoreForm = new ScoreForm();

        public GameBoard()
        {
            InitializeComponent();
            //Localization test
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            
            gameForm.MdiParent = this;
            gameForm.Show();
            
            //BETA
            //scoreForm.Show();
        }

        #region Child Forms

        public static void openTeamEditor()
        {
            TeamEditor teamEditor = new TeamEditor();
            teamEditor.Show();
        }
        public static void openQuestionSetEditor()
        {
            QuestionSetEditor questionSetEditor = new QuestionSetEditor();
            //Set the form in a new Window (on dual screens, it will be on the monitor 1
            questionSetEditor.ShowDialog();
        }
        #endregion

        #region FileMenu
        private void createQuestionToolStripMenuItem_Click_(object sender, EventArgs e)
        {
            openQuestionSetEditor();
            //Once we are done editing, if there is a currentFile, load it.
            //Otherwise clear the game board.
            //if (Settings.Default.currentFile != String.Empty && gameForm.loadFileBehavior(Settings.Default.currentFile))
            //    gameForm.loadGameBehavior();
            //else gameForm.clearGamePanel();
        }
        private void createGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //todo
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
        
        #region TeamMenu
        private void teamConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openTeamEditor();
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

        private void GameBoard_Load(object sender, EventArgs e)
        {
        }
    }
}
