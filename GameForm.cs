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
            Controls.Remove(gamePanel);

            gamePanel.Location = new Point(12, 75);
            gamePanel.AutoScroll = true;
            gamePanel.Width = ClientRectangle.Width - 20;
            gamePanel.Height = ClientRectangle.Height - 60;
            Controls.Add(gamePanel);

            //Load the actual question labels
            loadQuestionLabel();
            
            // Open the team editor if no team has been configured.
            /*
            if (Settings.Default.scoreSet == null && Settings.Default.useScoreSystem)
            {
                GameBoard.openTeamEditor();
            }*/
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
        #endregion

        #region Forms
        void openQuestion(int id)
        {
            //Load the question and questionType into temporary variables to pass to QuestionForm
            //Settings.Default.tempQuestion = questionSet[index].question;
            //Settings.Default.tempType = questionSet[index].type;

            //TODO: temporarily store the current question or questionID

            //Open the Question in a new form
            QuestionForm questionForm = new QuestionForm();

            //questionForm.answer = questionSet[index].answer;
            //questionForm.answered = questionSet[index].answered;
            questionForm.MdiParent = this.MdiParent;
            //questionForm.FormClosed += new FormClosedEventHandler((sender,args) => questionForm_FormClosed(sender, args, questionForm.answered, id));
            questionForm.Show();
        }
        #endregion



        //to deprecate
        public void clearGamePanel()
        {
            while (gamePanel.Controls.Count > 0) gamePanel.Controls.Clear();
        }
        
        #region EventHandlers

        void questionForm_FormClosed(object sender, FormClosedEventArgs e, bool answered, int index)
        {
            //After the question has been answered, get information
            //questionSet[index].answered = answered;
            
            //Autosave
            //saveBehavior();
            //Reload the form
            loadQuestionLabel();
        }
        void pointLabel_Click(object sender, EventArgs e)
        {
            Label caller = (Label)sender;
            int index = Convert.ToInt32(caller.Name);
            openQuestion(index);
        }
        void pointBtn_Click(object sender, EventArgs e)
        {
            Button caller = (Button)sender;
            openQuestion(Convert.ToInt32(caller.Name));
        }
        /*
        void questionForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            throw new NotImplementedException();
        }*/
        
        #endregion

        private void GameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
