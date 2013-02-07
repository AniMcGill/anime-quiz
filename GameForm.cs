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
        QuestionSet questionSet;
        FlowLayoutPanel gamePanel = new FlowLayoutPanel();

        //Constants
        Color ANSWERED_COLOR = Color.Black;
        Color UNANSWERED_BACKCOLOR = Color.Transparent;
        Color UNANSWERED_FORECOLOR = Color.Black;

        public GameForm()
        {
            InitializeComponent();

            //If there is a currentFile, load it. This doesn't work from closing GameEditor
            if (Settings.Default.reloadPrevious && CurrentQuestionSet.getInstance() != null)
                loadGameBehavior();
        }
        
        #region Behaviors
        public void loadGameBehavior()
        {
            gamePanel.Location = new Point(12, 75);
            gamePanel.AutoScroll = true;
            gamePanel.Width = ClientRectangle.Width - 20;
            gamePanel.Height = ClientRectangle.Height - 60;
            Controls.Add(gamePanel);

            //Load the actual question labels
            loadLabelBehavior();
            
            // Open the team editor if no team has been configured.
            if (Settings.Default.scoreSet == null && Settings.Default.useScoreSystem)
            {
                GameBoard.openTeamEditor();
            }
        }

        void loadLabelBehavior()
        {
            //Clear the gamePanel
            clearGamePanel();
            //Reload the gamePanel without answered questions
            for (int i = 0; i < questionSet.Count; i++)
            {
                if (!questionSet[i].answered)
                {
                    Button pointBtn = new Button();
                    pointBtn.Name = i.ToString();
                    pointBtn.Text = questionSet[i].points.ToString();
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
        void openQuestion(int index)
        {
            //Load the question and questionType into temporary variables to pass to QuestionForm
            //Settings.Default.tempQuestion = questionSet[index].question;
            //Settings.Default.tempType = questionSet[index].type;

            //TODO: temporarily store the current question or questionID

            //Open the Question in a new form
            QuestionForm questionForm = new QuestionForm();

            questionForm.answer = questionSet[index].answer;
            questionForm.answered = questionSet[index].answered;
            questionForm.MdiParent = this.MdiParent;
            questionForm.FormClosed += new FormClosedEventHandler((sender,args) => questionForm_FormClosed(sender, args, questionForm.answered, index));
            questionForm.Show();
        }

        void questionForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void clearGamePanel()
        {
            while (gamePanel.Controls.Count > 0) gamePanel.Controls.Clear();
        }

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
        #endregion
        
        #region EventHandlers

        void questionForm_FormClosed(object sender, FormClosedEventArgs e, bool answered, int index)
        {
            //After the question has been answered, get information
            questionSet[index].answered = answered;
            //Autosave
            //saveBehavior();
            //Reload the form
            loadLabelBehavior();
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
        
        #endregion

        private void GameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
