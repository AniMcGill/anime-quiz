using System;
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

        public QuestionForm(int questionId)
        {
            InitializeComponent();
            closeBtn.Visible = false;
            this.questionId = questionId;

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

        #region Event Handlers
        /// <summary>
        ///     When a question has been answered, show the answer and update the database.
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
