using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Anime_Quiz.Classes;
using Microsoft.VisualBasic;

namespace Anime_Quiz
{
    public partial class GameEditor : Form
    {
        //Data
        SQLiteDatabase sqlDB = new SQLiteDatabase();

        ComboBox gameList;
        FlowLayoutPanel questionSetFlowPanel;

        public GameEditor()
        {
            InitializeComponent();
            reloadGameListAndInfo();

            // The rename button doesn't work yet, disable it for now
            renameBtn.Enabled = false;
        }
        #region Database Load
        void loadGameList()
        {
            Controls.Remove(gameList);

            gameList = EmptyGame.getInstance().getGameSelector();
            gameList.Location = new Point(12, 12);
            gameList.TabIndex = 1;
            gameList.SelectedIndexChanged += gameList_SelectedIndexChanged;

            delBtn.Enabled = false;
            renameBtn.Enabled = false;

            Controls.Add(gameList);
        }
        void loadGameInfo()
        {
            Controls.Remove(questionSetFlowPanel);
            loadQuestionSets();
            CurrentGame.getInstance().setCheckboxes();

            delBtn.Enabled = true;
            renameBtn.Enabled = true;
        }
        void reloadGameListAndInfo()
        {
            loadGameList();
            if (CurrentGame.getInstance() != null)
                loadGameInfo();
        }

        void loadQuestionSets()
        {
            questionSetFlowPanel = CurrentGame.getInstance().addQuestionSetsCheckboxes();
            questionSetFlowPanel.Location = new Point(12, 75);
            Controls.Add(questionSetFlowPanel);
        }
        #endregion

        #region Database Save
        /*bool saveGames()
        {
            try
            {
                Dictionary<String, String> data = new Dictionary<string, string>();
                foreach (CheckBox checkbox in questionSetFlowPanel.Controls.OfType<CheckBox>())
                {
                    if (checkbox.Checked)
                    {
                        data.Add("name", CurrentGame.getInstance().name);
                        data.Add("questionSetId", checkbox.Text);
                    }
                }
                sqlDB.Insert("Games", data);
                return true;
            }
            catch
            {
                return false;
            }
        }*/
        #endregion

        #region Buttons
        /// <summary>
        ///     Creates an instance of the game. We do not add it to the database if it is empty.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addBtn_Click(object sender, EventArgs e)
        {
            String gameName = gameNameTextbox.Text;
            if (gameName != String.Empty)
            {
                CurrentGame.setInstance(new Game(gameName));
                loadGameList();
                loadGameInfo();
                SoundMessageBox.Show("The Game has been created. You can now select the Question Sets.", "Game Created", 
                    MessageBoxButtons.OK, Anime_Quiz.Properties.Resources.W_hellonyan); //provide some feedback
            }
        }

        private void delBtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this game? Scores will be lost.", 
                "Confirm deletion", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (sqlDB.DeleteGame(CurrentGame.getInstance().name))
                {
                    CurrentGame.setInstance(null);
                    loadGameList();
                }
                else
                    SoundMessageBox.Show("There was an error deleting the game.", "Fail", 
                        MessageBoxButtons.OK, Anime_Quiz.Properties.Resources.Muda);
            }
        }

        private void renameBtn_Click(object sender, EventArgs e)
        {
            /*
            string newGameName = Interaction.InputBox("Choose a new name: ", "Rename");
            try
            {
                if (newGameName != String.Empty && sqlDB.DeleteGame(CurrentGame.getInstance().name))
                {
                    CurrentGame.setInstance(new Game(newGameName));
                    CurrentGame.getInstance().saveGame();
                    //TODO
                }
                else
                    throw new ArgumentNullException("There was an error. Please reflect deeply upon your actions.");
            }
            catch (Exception crap)
            {
                SoundMessageBox.Show(crap.Message, Anime_Quiz.Properties.Resources.Muda);
            }*/
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Event Handlers
        void gameList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox senderComboBox = sender as ComboBox;
            String gameName = senderComboBox.SelectedItem.ToString();
            CurrentGame.setInstance(new Game(gameName));

            loadGameInfo();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (CurrentGame.getInstance() != null
                && !CurrentGame.getInstance().saveGame()
                && SoundMessageBox.Show("There was an error saving to the database. Close anyways?", "Database error", MessageBoxButtons.YesNo, Anime_Quiz.Properties.Resources.Muda) == DialogResult.No
                && !e.Cancel)
                e.Cancel = true;
            base.OnFormClosing(e);
        }
        #endregion
    }
}
