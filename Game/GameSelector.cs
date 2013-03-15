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
using Anime_Quiz.Team;

namespace Anime_Quiz
{
    public partial class GameSelector : Form
    {
        //Data
        SQLiteDatabase sqlDB = new SQLiteDatabase();

        ComboBox gameList;
        ComboBox questionSetList;
        Button loadBtn;
        Button teamSelectBtn;

        public GameSelector()
        {
            InitializeComponent();
            loadGameList();
            addLoadBtn();
            addTeamSelectBtn();
        }

        void createScoreList()
        {
            foreach (String team in CurrentTeams.getInstance().teams)
            {
                Dictionary<String, String> data = new Dictionary<string, string>();
                data.Add("gameId", CurrentGame.getInstance().name);
                data.Add("teamId", CurrentTeams.getInstance().getTeamId(team).ToString());
                sqlDB.InsertOrReplace("Scores", data);
            }
        }

        #region Data Load
        void loadGameList()
        {
            Controls.Remove(gameList);

            gameList = EmptyGame.getInstance().getGameSelector();
            gameList.Location = new Point(12, 12);
            gameList.TabIndex = 1;
            gameList.SelectedIndexChanged += gameList_SelectedIndexChanged;

            Controls.Add(gameList);
        }
        void loadQuestionSetList()
        {
            teamSelectBtn.Enabled = false;
            createScoreList();

            Controls.Remove(questionSetList);
            DataTable questionSetTable = CurrentGame.getInstance().getGameQuestionSets();
            questionSetList = new ComboBox();
            questionSetList.Size = new Size(163, 21);
            questionSetList.Location = new Point(12, 43);
            questionSetList.Text = "Select Question Set";
            questionSetList.SelectedIndexChanged += questionSetList_SelectedIndexChanged;
            foreach (DataRow row in questionSetTable.Rows)
            {
                questionSetList.Items.Add(row["questionSetId"]);
            }

            Controls.Add(questionSetList);
            loadBtn.Visible = true;
        }
        #endregion

        #region Buttons
        void addTeamSelectBtn()
        {
            teamSelectBtn = new Button();
            teamSelectBtn.Text = "Register Teams";
            teamSelectBtn.Enabled = false;
            teamSelectBtn.Location = new Point(200, 12);
            teamSelectBtn.Size = new Size(106, 23);
            teamSelectBtn.Click += teamSelectBtn_Click;
            Controls.Add(teamSelectBtn);
        }

        void addLoadBtn()
        {
            loadBtn = new Button();
            loadBtn.Text = "Load";
            loadBtn.Visible = false;
            loadBtn.Enabled = false;
            loadBtn.Location = new Point(200, 41);
            loadBtn.Size = new Size(53, 23);
            loadBtn.Click += loadBtn_Click;
            Controls.Add(loadBtn);
        }
        #endregion

        #region Forms
        void openTeamSelector()
        {
            TeamSelector teamSelector = new TeamSelector();
            teamSelector.FormClosed += teamSelector_FormClosed;
            teamSelector.Show();
        }
        #endregion

        #region Event Handlers
        /// <summary>
        ///     Set the current game instance and load the list of question sets.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gameList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox senderComboBox = sender as ComboBox;
            String gameName = senderComboBox.Text;
            CurrentGame.setInstance(new Game(gameName));
            teamSelectBtn.Enabled = true;
            if (CurrentTeams.getInstance() != null && CurrentTeams.getInstance().teams.Length > 0)
                loadQuestionSetList();
        }

        void teamSelectBtn_Click(object sender, EventArgs e)
        {
            openTeamSelector();
        }
        void teamSelector_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (CurrentTeams.getInstance() != null && CurrentTeams.getInstance().teams.Length > 0)
                loadQuestionSetList();
        }

        /// <summary>
        ///     Set the current question set instance and enable the load button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void questionSetList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox senderComboBox = sender as ComboBox;
            String questionSetName = senderComboBox.Text;
            Types questionSetType = sqlDB.getQuestionSetType(questionSetName);
            CurrentQuestionSet.setInstance(new QuestionSet(questionSetName, questionSetType));
            loadBtn.Enabled = true;
        }

        /// <summary>
        ///     Reloads the game form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void loadBtn_Click(object sender, EventArgs e)
        {
            GameBoard.gameForm.loadGamePanel();
        }
        #endregion
    }
}
