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

namespace Anime_Quiz
{
    public partial class GameEditor : Form
    {
        //Data
        SQLiteDatabase sqlDB = new SQLiteDatabase();
        //DataSet gameDataSet;

        ComboBox gameList;
        DataGridView gameGridView;

        public GameEditor()
        {
            InitializeComponent();
            reloadGameListAndInfo();
            loadQuestionSets();
        }
        #region Database Load
        void loadGameList()
        {
            Controls.Remove(gameList);

            String query = "select distinct name from Games";
            DataTable queryData = sqlDB.getDataTable(query);
            gameList = new ComboBox();
            gameList.Location = new Point(12, 12);
            gameList.Size = new Size(163, 21);
            gameList.Text = "Select a game to load";
            gameList.TabIndex = 1;
            gameList.SelectedIndexChanged += gameList_SelectedIndexChanged;
            
            foreach (DataRow row in queryData.Rows)
            {
                gameList.Items.Add(row["name"]);
            }

            clearBtn.Enabled = false;
            delBtn.Enabled = false;
            renameBtn.Enabled = false;

            Controls.Add(gameList);
        }
        void loadGameInfo()
        {
            Controls.Remove(gameGridView);

            String query = String.Format("select distinct questionSetId from Games where name='{0}'", CurrentGame.getInstance());
            DataSet questionSets = sqlDB.getDataSet(query);
            gameGridView = new DataGridView();
            gameGridView.Location = new Point(12, 75);
            gameGridView.AutoSize = true;
            gameGridView.DataSource = questionSets.Tables[0];
            Controls.Add(gameGridView);

            clearBtn.Enabled = true;
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
            String query = "select name from QuestionSets";
            DataTable questionSetList = sqlDB.getDataTable(query);
            FlowLayoutPanel questionSetFlowPanel = new FlowLayoutPanel();
            questionSetFlowPanel.Location = new Point(417, 75);
            questionSetFlowPanel.AutoSize = true;
            questionSetFlowPanel.FlowDirection = FlowDirection.TopDown;
            questionSetFlowPanel.AutoScroll = true;
            Controls.Add(questionSetFlowPanel);

            foreach (DataRow row in questionSetList.Rows)
            {
                CheckBox checkbox = new CheckBox();
                checkbox.Text = row["name"].ToString();
                checkbox.CheckedChanged += checkbox_CheckedChanged;
                questionSetFlowPanel.Controls.Add(checkbox);
            }
        }
        #endregion

        #region Buttons
        private void addBtn_Click(object sender, EventArgs e)
        {
            String gameName = gameNameTextbox.Text;
            if (gameName != String.Empty && sqlDB.CreateBlankGame(gameName))
            {
                CurrentGame.setInstance(gameName);
                reloadGameListAndInfo();
            }
        }
        private void clearBtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear the question sets in this game? Scores will be lost.", 
                "Confirm deletion", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                sqlDB.DeleteGame(CurrentGame.getInstance());
                sqlDB.CreateBlankGame(CurrentGame.getInstance());
                reloadGameListAndInfo();
            }
        }

        private void delBtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this game? Scores will be lost.", 
                "Confirm deletion", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (sqlDB.DeleteGame(CurrentGame.getInstance()))
                {
                    CurrentGame.setInstance(null);
                    loadGameList();
                    Controls.Remove(gameGridView);
                }
                else
                    SoundMessageBox.Show("There was an error deleting the game.", "Fail", 
                        MessageBoxButtons.OK, Anime_Quiz.Properties.Resources.Muda);
            }
        }

        private void renameBtn_Click(object sender, EventArgs e)
        {
            //TODO
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
            CurrentGame.setInstance(gameName);

            loadGameInfo();
        }

        void checkbox_CheckedChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // TODO
            base.OnFormClosing(e);
        }
        #endregion
    }
}
