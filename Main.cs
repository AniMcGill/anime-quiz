using System;
using System.Windows.Forms;
using Anime_Quiz.Properties;


namespace Anime_Quiz
{
    public partial class GameBoard : Form
    {
        //Forms
        public static GameForm gameForm = new GameForm();

        public GameBoard()
        {
            InitializeComponent();
            
            gameForm.MdiParent = this;
            gameForm.Show();
        }

        #region Child Forms

        public static void openTeamEditor()
        {
            TeamEditor teamEditor = new TeamEditor();
            teamEditor.Show();
        }
        public static void openGameSelector()
        {
            GameSelector gameSelector = new GameSelector();
            gameSelector.Show();
        }
        public static void openQuestionSetEditor()
        {
            QuestionSetEditor questionSetEditor = new QuestionSetEditor();
            questionSetEditor.FormClosed += questionSetEditor_FormClosed;
            //Set the form in a new Window (on dual screens, it will be on the monitor 1)
            questionSetEditor.ShowDialog();
        }
        public static void openGameEditor()
        {
            GameEditor gameEditor = new GameEditor();
            gameEditor.Show();
        }
        #endregion

        #region Event Handlers
        static void questionSetEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            gameForm.loadGamePanel();
        }
        #endregion

        #region FileMenu
        private void openGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openGameSelector();
        }
        private void createQuestionToolStripMenuItem_Click_(object sender, EventArgs e)
        {
            openQuestionSetEditor();
        }
        private void createGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openGameEditor();
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.Default.Save();
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
