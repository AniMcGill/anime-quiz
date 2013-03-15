using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Anime_Quiz.Classes;

namespace Anime_Quiz
{
    public partial class QuestionForm : Form
    {
        //Data
        DataSet questionDataSet;
        DataRow questionData;
        SQLiteDatabase sqlDB = new SQLiteDatabase();
        int questionId;

        // Questions constructors
        FlowLayoutPanel questionPanel = new FlowLayoutPanel();
        Label questionLabel = new Label();
        MusicPlayer musicPlayer;
        PictureBox screenshotBox;

        // Scoring system
        List<int> answeringOrder;

        public QuestionForm(int questionId)
        {
            InitializeComponent();
            this.KeyPreview = true;
            closeBtn.Visible = false;
            this.questionId = questionId;
            answeringOrder = new List<int>(CurrentTeams.getInstance().teams.Length);

            loadQuestionPanel();
            loadQuestionLabel();
            loadQuestion();
        }

        #region Controls
        private void loadQuestionPanel()
        {
            //Initialize the questionPanel
            questionPanel.Location = new Point(12, 50);
            questionPanel.Width = ClientRectangle.Width;
            questionPanel.Height = ClientRectangle.Height - 50;
            Controls.Add(questionPanel);
        }
        private void loadQuestionLabel()
        {
            questionLabel.Text = "Entry）";
            questionLabel.TextAlign = ContentAlignment.TopLeft;
            questionLabel.AutoSize = true;
            questionLabel.Width = ClientRectangle.Width;
            questionLabel.Height = 150;
            questionLabel.Font = new Font("Microsoft Sans Serif", 50);

            questionLabel.Location = new Point(12, 50);
            questionPanel.Controls.Add(questionLabel);
        }
        private void loadQuestion()
        {
            questionDataSet = sqlDB.getDataSet(String.Format("select * from Questions where id = '{0}'", questionId));
            questionData = questionDataSet.Tables[0].Rows[0];
            String question = questionData["question"].ToString();
            switch (CurrentQuestionSet.getInstance().type)
            {
                case Types.Question:
                    questionLabel.Text += question;
                    questionLabel.Width = ClientRectangle.Width - 20;
                    questionLabel.Height = ClientRectangle.Height - 64;
                    break;
                case Types.Music:
                    musicPlayer = new MusicPlayer(question, questionPanel);
                    break;
                case Types.Screenshot:
                    Image screenshot = Image.FromFile(question);
                    //Resize the image to fit a safe size (projector 1024x768)
                    int width = ClientRectangle.Width;
                    int height = width * screenshot.Height / screenshot.Width;
                    if (height > ClientRectangle.Height - 100)
                    {
                        height = ClientRectangle.Height - 100;
                        width = height * screenshot.Width / screenshot.Height;
                    }
                    Bitmap resizedScreenshot = new Bitmap(screenshot, new Size(width, height));

                    //Initialize the image
                    screenshotBox = new PictureBox();
                    screenshotBox.Image = resizedScreenshot;
                    screenshotBox.Location = new Point(12, 200);
                    screenshotBox.Width = ClientRectangle.Width;
                    screenshotBox.Height = ClientRectangle.Height;
                    questionPanel.Controls.Add(screenshotBox);
                    break;
            }
        }
        #endregion

        #region Buttons
        /// <summary>
        ///     Processes order information from the teams.
        /// </summary>
        /// <param name="key"></param>
        private void processAnswerButton(int key)
        {
            int teamIndex = key - 97;
            if (answeringOrder.Count < CurrentTeams.getInstance().teams.Length
                && !answeringOrder.Contains(teamIndex))
            {
                answeringOrder.Add(teamIndex);
                addAnswerButtons();
            }
            if (answeringOrder.Count == CurrentTeams.getInstance().teams.Length)
                addAnswerButtons();
        }
        FlowLayoutPanel buttonLayoutPanel;
        void addButtonLayoutPanel()
        {
            Controls.Remove(buttonLayoutPanel);

            buttonLayoutPanel = new FlowLayoutPanel();
            buttonLayoutPanel.Location = new Point(168 + 5, 9);
            buttonLayoutPanel.Size = new Size(168 + 75 * (1 + CurrentTeams.getInstance().teams.Length) + 10, 25);
            Controls.Add(buttonLayoutPanel);
        }
        void addAnswerButtons()
        {
            addButtonLayoutPanel();

            answeringOrder.ForEach(delegate(int i)
            {
                Button answerBtn = new Button();
                answerBtn.Text = CurrentTeams.getInstance().teams[i];
                answerBtn.Location = new Point((168 + 75 * answeringOrder.IndexOf(i)) + 5, 12);
                answerBtn.Size = new Size(75, 23);
                answerBtn.Click += answerBtn_Click;
                buttonLayoutPanel.Controls.Add(answerBtn);
            });
            addResetButton();
        }
        void addResetButton()
        {
            Button resetBtn = new Button();
            resetBtn.Text = "Reset";
            resetBtn.Location = new Point((168 + 75 * CurrentTeams.getInstance().teams.Length) + 10, 12);
            resetBtn.Size = new Size(75, 23);
            resetBtn.Click += resetBtn_Click;
            buttonLayoutPanel.Controls.Add(resetBtn);
        }
        #endregion

        #region Event Handlers
        private bool isAnswerButton = false;
        /// <summary>
        ///     Listens to the order with which each team presses the answer button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuestionForm_KeyDown(object sender, KeyEventArgs e)
        {
            isAnswerButton = false;
            int numOfTeams = CurrentTeams.getInstance().teams.Length;
            if (e.KeyValue >= 97 && e.KeyValue <= 97+numOfTeams-1)
            {
                isAnswerButton = true;
                processAnswerButton(e.KeyValue);
            }
        }

        /// <summary>
        ///     Ignores keyboard input from the keys corresponding to the answer buttons.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuestionForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!isAnswerButton)
                e.Handled = true;
        }
        /// <summary>
        ///     When a question has been answered, show the answer and update the database.
        ///     The team who has answered the question will receive the points.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void answerBtn_Click(object sender, EventArgs e)
        {
            questionLabel.Text = questionData["answer"].ToString();
            questionLabel.TextAlign = ContentAlignment.TopCenter;
            //questionData["answered"] = true;  //Concurrency issue when pushing this back to DB, using alternate method
            String command = String.Format("update Questions set answered = 1 where id = '{0}'", questionId);
            sqlDB.executeNonQuery(command);

            Button senderBtn = sender as Button;
            if (!senderBtn.Text.Equals("Answer"))
            {
                senderBtn.Enabled = false;  // prevents accidentally double-clicking
                String answeringTeam = senderBtn.Text;
                int answeringTeamId = CurrentTeams.getInstance().getTeamId(answeringTeam);
                String gameId = CurrentGame.getInstance().name;
                int currentPoints = Score.getScore(answeringTeam);
                int points = currentPoints + Convert.ToInt32(questionData["points"].ToString());

                Dictionary<String, String> data = new Dictionary<string, string>();
                data.Add("score", points.ToString());
                String updateCmd = String.Format("gameId = '{0}' and teamId = {1}", gameId, answeringTeamId);
                sqlDB.Update("Scores", data, updateCmd);
            }

            closeBtn.Visible = true;
            switch (CurrentQuestionSet.getInstance().type)
            {
                case Types.Question:
                    break;
                case Types.Music:
                    musicPlayer.dispose(questionPanel);
                    break;
                case Types.Screenshot:
                    screenshotBox.Dispose();
                    break;
                default:
                    break;
            }
        }
        void resetBtn_Click(object sender, EventArgs e)
        {
            Controls.Remove(buttonLayoutPanel);
            answeringOrder.Clear();
        }
        void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void QuestionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CurrentQuestionSet.getInstance().type == Types.Music)
                musicPlayer.dispose(questionPanel);
        }
        #endregion
    }
}
