using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Anime_Quiz.Classes;
using Anime_Quiz.Properties;

namespace Anime_Quiz
{
    public partial class GameForm : Form
    {
        //Data
        SQLiteDatabase sqlDB = new SQLiteDatabase();
        DataSet questionDataSet;

        FlowLayoutPanel gamePanel = new FlowLayoutPanel();

        //Constants
        Color ANSWERED_COLOR = Color.Black;
        Color UNANSWERED_BACKCOLOR = Color.Transparent;
        Color UNANSWERED_FORECOLOR = Color.Black;

        public GameForm()
        {
            InitializeComponent();

            //If there is an instance of CurrentQuestionSet, load it. Otherwise prompt for a list to load (TODO)
            if (Settings.Default.reloadPrevious && CurrentQuestionSet.getInstance() != null)
                loadGamePanel();
            else
            {
                //TODO
            }
        }
        
        #region Controls
        public void loadGamePanel()
        {
            clearGamePanel();
            gamePanel.Location = new Point(12, 75);
            gamePanel.AutoScroll = true;
            gamePanel.Width = ClientRectangle.Width - 20;
            gamePanel.Height = ClientRectangle.Height - 60;
            Controls.Add(gamePanel);

            //Load the actual question labels
            if(CurrentQuestionSet.getInstance() != null)
                loadQuestionLabel();
        }
        /// <summary>
        ///     Add a Button for each unanswered Question.
        /// </summary>
        void loadQuestionLabel()
        {
            questionDataSet = sqlDB.getDataSet(String.Format("select * from Questions where questionSet = '{0}'", CurrentQuestionSet.getInstance().name));
            foreach(DataRow row in questionDataSet.Tables[0].Rows)
            {
                if(!Convert.ToBoolean(row["answered"]))
                {
                    Button pointBtn = new Button();
                    pointBtn.Name = row["id"].ToString();
                    pointBtn.Text = row["points"].ToString();
                    pointBtn.Font = new Font("Microsoft Sans Serif", 20);
                    pointBtn.Width = 150;
                    pointBtn.Height = 75;
                    pointBtn.BackColor = UNANSWERED_BACKCOLOR;
                    pointBtn.ForeColor = UNANSWERED_FORECOLOR;
                    pointBtn.Click += new EventHandler(pointBtn_Click);
                    gamePanel.Controls.Add(pointBtn);
                }
            }
        }
        /// <summary>
        ///     Clears the contents of the gamePanel control
        /// </summary>
        public void clearGamePanel()
        {
            while (gamePanel.Controls.Count > 0) gamePanel.Controls.Clear();
        }
        #endregion

        #region Forms
        /// <summary>
        ///     Open a QuestionForm with the selected Question
        /// </summary>
        /// <param name="id"></param>
        void openQuestion(int id)
        {
            QuestionForm questionForm = new QuestionForm(id);
            questionForm.MdiParent = this.MdiParent;
            questionForm.FormClosed += questionForm_FormClosed;
            questionForm.Show();
        }
        #endregion

        #region EventHandlers
        void questionForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            loadGamePanel();
        }
        void pointBtn_Click(object sender, EventArgs e)
        {
            Button caller = (Button)sender;
            openQuestion(Convert.ToInt32(caller.Name));
        }      
        #endregion

        private void GameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //nothing to do here for now
        }
    }
}
