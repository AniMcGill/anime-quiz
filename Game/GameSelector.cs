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
    public partial class GameSelector : Form
    {
        //Data
        SQLiteDatabase sqlDB = new SQLiteDatabase();

        ComboBox gameList;
        ComboBox questionSetList;
        Button loadBtn;

        public GameSelector()
        {
            InitializeComponent();
            loadGameList();
            addLoadBtn();
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
