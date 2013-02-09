using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Media;
using System.Reflection;
using System.Windows.Forms;
using Anime_Quiz.DataModel;
using Anime_Quiz.Properties;
//using System.Globalization;
using Microsoft.VisualBasic;

namespace Anime_Quiz
{
    public partial class QuestionSetEditor : Form
    {
        //Data
        SQLiteDatabase sqlDB = new SQLiteDatabase();
        DataSet questionDataSet;

        ComboBox questionSetList;
        DataGridView questionGridView;
        FlowLayoutPanel mediaPanel;
        MusicPlayer musicPlayer;

        public QuestionSetEditor()
        {
            InitializeComponent();
            reloadQuestionsAndSets();
        }

        #region Database Load
        /// <summary>
        ///     Loads the QuestionSets from the database and display them in a ComboBox
        /// </summary>
        private void loadQuestionSets()
        {
            Controls.Remove(questionSetList);

            String query = "select * from QuestionSets";
            DataTable queryData = sqlDB.getDataTable(query);
            questionSetList = new ComboBox();
            questionSetList.Location = new Point(12,12);
            questionSetList.Size = new Size(163, 21);
            questionSetList.Text = "Select a game to load";
            questionSetList.TabIndex = 1;
            questionSetList.SelectedIndexChanged += questionSetList_SelectedIndexChanged;

            foreach (DataRow row in queryData.Rows)
            {
                questionSetList.Items.Add(row["name"]);
            }
            Controls.Add(questionSetList);
        }
        /// <summary>
        ///     Load the Questions associated with the given QuestionSet
        /// </summary>
        /// <param name="questionSet">The QuestionSet from which to load the Questions.</param>
        private void loadQuestions(string questionSet)
        {
            Controls.Remove(questionGridView);
            questionDataSet = sqlDB.getDataSet(String.Format("Select * from Questions where questionSet = '{0}'", questionSet));
            questionGridView = new DataGridView();
            questionGridView.Location = new Point(12, 75);
            questionGridView.AutoSize = true;
            questionGridView.MaximumSize = new Size((int)(0.70 * this.Width), (int)(0.8 * this.Height));
            questionGridView.DataSource = questionDataSet.Tables[0];
            questionGridView.CellMouseClick += questionGridView_CellMouseClick;
            questionGridView.CellFormatting += questionGridView_CellFormatting;
            Controls.Add(questionGridView);
            
            clrBtn.Enabled = true;
            delBtn.Enabled = true;

            loadMediaPanel();
        }
        /// <summary>
        ///     Loads the QuestionSets and if there is an instance of CurrentQuestionSet, loads the Questions.
        /// </summary>
        private void reloadQuestionsAndSets()
        {
            loadQuestionSets();
            if (CurrentQuestionSet.getInstance() != null)
                loadQuestions(CurrentQuestionSet.getInstance().name);
        }
        #endregion

        #region Controls
        private void loadMediaPanel()
        {
            Controls.Remove(mediaPanel);
            mediaPanel = new FlowLayoutPanel();
            mediaPanel.Location = new Point(this.Width - (int)(0.25 * this.Width), 75);
            mediaPanel.AutoSize = true;
            mediaPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            Controls.Add(mediaPanel);
        }
        private void addPictureBox(Image image)
        {
            int pictureWidth = (int)(0.2 * (this.Width - 12));
            int pictureHeight = (int)(3 * pictureWidth / 4);
            PictureBox pictureBox = new PictureBox();
            pictureBox.Size = new Size(pictureWidth, pictureHeight);
            Bitmap resizedImage = new Bitmap(image, new Size(pictureWidth, pictureHeight));
            pictureBox.Image = resizedImage;
            mediaPanel.Controls.Add(pictureBox);
        }
        #endregion

        private void linkQuestionToQuestionSet()
        {
            try
            {
                foreach (DataRow row in questionDataSet.Tables[0].Rows)
                    row["questionSet"] = CurrentQuestionSet.getInstance().name;
            }
            catch (DeletedRowInaccessibleException crap)
            { }
        }
        /// <summary>
        ///     Saves the questions to the database. For the time being, malformed questions will be ignored.
        /// </summary>
        /// <returns></returns>
        private bool saveQuestions()
        {
            if (CurrentQuestionSet.getInstance() != null
                && questionDataSet != null)
            {
                linkQuestionToQuestionSet();
                return sqlDB.updateDataSet(questionDataSet, "Questions");
            }
            return false;
        }

        #region Buttons
        /// <summary>
        ///     Clears the questions in the current QuestionSet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearBtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear this QuestionSet? This cannot be undone.", "Confirm deletion", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                sqlDB.ClearQuestionsFromSet(CurrentQuestionSet.getInstance().name);
                clrBtn.Enabled = false;
                reloadQuestionsAndSets();
            }
        }
        /// <summary>
        ///     Save the current QuestionSet just in case, then add and load the new one.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addBtn_Click(object sender, EventArgs e)
        {
            String newSetName = newSetTextbox.Text;
            Types newSetType = (Types)gameType.SelectedIndex;
            Dictionary<String, String> data = new Dictionary<string, string>();
            data.Add("name", newSetName);
            data.Add("type", ((int)newSetType).ToString());
            if (sqlDB.Insert("QuestionSets", data))
            {
                saveQuestions();
                CurrentQuestionSet.setInstance(new QuestionSet(newSetName, newSetType));
                reloadQuestionsAndSets();
            }
        }
        /// <summary>
        ///     Deletes the entire QuestionSet from the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delBtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the entire QuestionSet? This cannot be undone.", "Confirm deletion", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (sqlDB.Delete("QuestionSets", String.Format("name = '{0}'", CurrentQuestionSet.getInstance().name)))
                {
                    CurrentQuestionSet.setInstance(null);   //set the instance to null?
                    delBtn.Enabled = false;
                    loadQuestionSets();
                    Controls.Remove(questionGridView);
                }
                else
                    MessageBox.Show("There was a problem completing the operation", "Error", MessageBoxButtons.OK);
            }
        }
        /// <summary>
        ///     Renames the QuestionSet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void renameBtn_Click(object sender, EventArgs e)
        {
            string questionSetName = Interaction.InputBox("Choose a new name: ", "Rename");
            try
            {
                if (questionSetName != String.Empty)
                {
                    String oldName = CurrentQuestionSet.getInstance().name;
                    CurrentQuestionSet.getInstance().name = questionSetName;
                    if (!sqlDB.renameQuestionSet(oldName,questionSetName))
                        throw new Exception("There was an error renaming the QuestionSet.");
                    reloadQuestionsAndSets();
                }
                else
                    throw new ArgumentNullException("Name cannot be blank.");
            }
            catch (Exception crap)
            {
                MessageBox.Show(crap.Message);
            }
        }
        /// <summary>
        ///     Unchecks the Answered flag for each Question in the QuestionSet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uncheckBtn_Click(object sender, EventArgs e)
        {
            foreach (DataRow row in questionDataSet.Tables[0].Rows)
                row["answered"] = false;
        }
        /// <summary>
        ///     Closes the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
        
        #region EventHandlers
        /// <summary>
        ///     Keeps track of the selected QuestionSet and load the Questions from it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void questionSetList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Save the currently loaded Questions, if any
            saveQuestions();
            // Save the current selection
            ComboBox senderComboBox = sender as ComboBox;
            String questionSetName = senderComboBox.SelectedItem.ToString();
            Types questionSetType = sqlDB.getQuestionSetType(questionSetName);
            CurrentQuestionSet.setInstance(new QuestionSet(questionSetName, questionSetType));

            // Load the Questions
            loadQuestions(questionSetName);

            // Load an EasterEgg
            loadPlaceholderMedia();
        }
        /// <summary>
        ///     When a question cell has been clicked, show file picker dialog if it is a music or screenshot type of QuestionSet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void questionGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (questionGridView.Columns[e.ColumnIndex] == questionGridView.Columns["Question"]
                && CurrentQuestionSet.getInstance().type != Types.Question)
            {
                String filename = questionGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                if(filename == String.Empty)
                    openFilePicker(e);
                else                  
                    displayMedia(filename);
            }
         }
        /// <summary>
        ///     Prompts the user for a file and enter save the file path.
        /// </summary>
        /// <param name="e"></param>
        void openFilePicker(DataGridViewCellMouseEventArgs e)
        {
            OpenFileDialog filePicker = new OpenFileDialog();
            switch (CurrentQuestionSet.getInstance().type)
            {
                case Types.Music:
                    if (Settings.Default.defaultMusicFolder != String.Empty)
                        filePicker.InitialDirectory = Settings.Default.defaultMusicFolder;
                    filePicker.Filter = "Music Formats|" +
                        "*.mp3;*.ram;*.rm;*.wav;*.wma;*.mid;*.mp4|" +
                        "mp3 (*.mp3)|*.mp3|ram (*.ram)|*.ram|rm (*.rm)|*.rm|" +
                        "wav (*.wav)|*.wav|wma (*.wma)|*.wma|mid (*.mid)|*.mid|" +
                        "mp4 (*.mp4)|*.mp4";
                    break;
                case Types.Screenshot:
                    if (Settings.Default.defaultScreenshotFolder != String.Empty)
                        filePicker.InitialDirectory = Settings.Default.defaultScreenshotFolder;
                    filePicker.Filter = "JPG files (*.jpg)|*.jpg|PNG files (*.png)|*.png|BMP files (*.bmp)|*.bmp";
                    break;
            }
            if (filePicker.ShowDialog() == DialogResult.OK)
                questionGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = filePicker.FileName;
        }
        /// <summary>
        ///     Display a preview of the relevant media (picture or music).
        /// </summary>
        /// <param name="filename">The path to the media file to load.</param>
        void displayMedia(string filename)
        {
            loadMediaPanel();
            switch (CurrentQuestionSet.getInstance().type)
            {
                case Types.Music:
                    if (musicPlayer != null)
                        musicPlayer.dispose(mediaPanel);
                    musicPlayer = new MusicPlayer(filename, mediaPanel);
                    break;
                case Types.Screenshot: 
                    Image image = Image.FromFile(filename);
                    addPictureBox(image);
                    break;
            }
        }
                
        /// <summary>
        ///     Enables the add QuestionSet button when a type has been selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gameType_SelectedIndexChanged(object sender, EventArgs e)
        {
            addBtn.Enabled = true;
        }
        /// <summary>
        ///     Change the format of the cells that shouldn't be edited to gray
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void questionGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if ((sender as DataGridView).Columns[e.ColumnIndex].Name == "id"
                    || (sender as DataGridView).Columns[e.ColumnIndex].Name == "questionSet")
                {
                    if (e.Value != null)
                        e.CellStyle.BackColor = Color.Gray;
                }
            }
            catch (ArgumentOutOfRangeException crap)
            { }
        }
        /// <summary>
        ///     Override the FormClosing event handler to handle database errors.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (CurrentQuestionSet.getInstance() != null
                && !saveQuestions()
                && MessageBox.Show("There was an error saving to the database. Close anyways?", "Database error", MessageBoxButtons.YesNo) == DialogResult.No
                && !e.Cancel)
                e.Cancel = true;
            base.OnFormClosing(e);
        }
        #endregion

        #region Easter Eggs
        /// <summary>
        ///     Loads placeholder media files. Audio solution inspired from:
        ///     http://social.msdn.microsoft.com/Forums/en/csharpgeneral/thread/63e7575e-9265-47bb-a9d9-d96d632b461c
        /// </summary>
        private void loadPlaceholderMedia()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            switch (CurrentQuestionSet.getInstance().type)
            {
                case Types.Music:
                    SoundPlayer soundPlayer = new SoundPlayer(Anime_Quiz.Properties.Resources.Muda);
                    soundPlayer.Play();
                    break;
                case Types.Screenshot:
                    Stream imageStream = assembly.GetManifestResourceStream("Anime_Quiz.Data.maedax.png");
                    Image image = Image.FromStream(imageStream);
                    loadMediaPanel();
                    addPictureBox(image);
                    break;
            }
        }
        #endregion
    }
}
