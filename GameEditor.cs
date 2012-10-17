using System;
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
using System.Collections;
//using System.Globalization;

namespace Anime_Quiz
{
    public partial class GameEditor : Form
    {
        string selectedType;
        int numQuest;
        FlowLayoutPanel gamePanel = new FlowLayoutPanel();

        //Data array
        QuestionSet questionSet;

        public GameEditor()
        {
            InitializeComponent();

            //Set the initial directory for our file choosers if the default folder is configured
            if (Settings.Default.defaultFolder != String.Empty)
            {
                gameSave.InitialDirectory = Settings.Default.defaultFolder;
                gameLoad.InitialDirectory = Settings.Default.defaultFolder;
            }
            //Load the last game if there is one in memory
            if (Settings.Default.currentFile != String.Empty)
                loadBehavior();
        }

        private void reinitializeGameBoard()
        {
            //Change the saveState to false
            Settings.Default.saveState = false;
            //cancelBtn.Text = CultureInfo.CurrentUICulture.Name.Equals("ja-JP")?"キャンセル":"Cancel";
            cancelBtn.Text = "Cancel";

            //Create the number of specified question boxes for the selected game type.          
            gamePanel.Location = new Point(12, 56);
            gamePanel.AutoScroll = true;
            gamePanel.Width = ClientRectangle.Width - 20;
            gamePanel.Height = ClientRectangle.Height - 64;
            Controls.Add(gamePanel);
            switch (selectedType)
            {
                case "Question":
                    for (int i = 0; i < numQuest; i++)
                        addQuestion();
                    break;
                case "Music":
                    for (int i = 0; i < numQuest; i++)
                        addMusic();
                    break;
                case "Screenshot":
                    for (int i = 0; i < numQuest; i++)
                        addScreenshot();
                    break;
                default:
                    /*if (CultureInfo.CurrentUICulture.Name.Equals("ja-JP"))
                        MessageBox.Show("エラー：ゲームタイプを選んでいなかった \nβακα…_〆(゜▽゜*)(((;゜Д゜))",
                        "エラー", MessageBoxButtons.OK);*/
                    //else 
                    MessageBox.Show("FAIL: You have not selected any question type herp derp.", 
                        "Something went wrong", MessageBoxButtons.OK);
                    break;
            }
            setAddRemoveGenBtn(true);
        }

        private void addQuestion()
        {
            FlowLayoutPanel questionPanel = new FlowLayoutPanel();
            questionPanel.Width = 200;
            questionPanel.Height = 210;
            gamePanel.Controls.Add(questionPanel);

            //Question text box
            Label questionLabel = new Label();
            questionLabel.Text = selectedType;
            questionLabel.Margin = new Padding(20, 20, 0, 0);
            questionPanel.Controls.Add(questionLabel);
            TextBox questionTextBox = new TextBox();
            questionTextBox.Multiline = true;
            questionTextBox.AcceptsReturn = true;
            questionTextBox.Width = 200;
            questionTextBox.Height = 40;
            questionTextBox.Margin = new Padding(20, 0, 20, 0);
            questionPanel.Controls.Add(questionTextBox);

            //Answer text box
            Label answerLabel = new Label();
            //answerLabel.Text = CultureInfo.CurrentUICulture.Name.Equals("ja-JP")?"答え:":"Answer:";
            answerLabel.Text = "Answer";
            answerLabel.Margin = new Padding(20, 5, 0, 0);
            questionPanel.Controls.Add(answerLabel);
            TextBox answerTextBox = new TextBox();
            answerTextBox.Multiline = true;
            answerTextBox.AcceptsReturn = true;
            answerTextBox.Width = 200;
            answerTextBox.Margin = new Padding(20, 0, 20, 0);
            questionPanel.Controls.Add(answerTextBox);

            //Number of points
            Label pointLabel = new Label();
            //pointLabel.Text = CultureInfo.CurrentUICulture.Name.Equals("ja-JP")?"ポイント: ":"Points: ";
            pointLabel.Text = "Points: ";
            pointLabel.Width = 50;
            pointLabel.Margin = new Padding(20, 0, 0, 0);
            questionPanel.Controls.Add(pointLabel);
            TextBox pointTextBox = new TextBox();
            pointTextBox.Multiline = false;
            pointTextBox.Width = 25;
            pointTextBox.Margin = new Padding(0, 0, 0, 0);
            pointTextBox.KeyPress += num_KeyPress;
            questionPanel.Controls.Add(pointTextBox);

            //Question answered or not
            CheckBox answeredCheckBox = new CheckBox();
            answeredCheckBox.Text = "Answered";
            answeredCheckBox.Margin = new Padding(20, 0, 0, 0);
            answeredCheckBox.Checked = false;
            questionPanel.Controls.Add(answeredCheckBox);
        }
        private void addMusic()
        {
            FlowLayoutPanel soundPanel = new FlowLayoutPanel();
            soundPanel.Width = 200;
            soundPanel.Height = 210;
            gamePanel.Controls.Add(soundPanel);

            //Screenshot box
            Button soundPicker = new Button();
            soundPicker.Text = selectedType;
            soundPicker.Margin = new Padding(20, 20, 0, 0);
            soundPicker.Width = 75;
            soundPicker.Height = 23;
            soundPicker.Click += new EventHandler(soundPicker_Click);
            soundPanel.Controls.Add(soundPicker);
            Label soundPath = new Label();
            soundPath.Margin = new Padding(20, 0, 20, 0);
            soundPath.Width = 200;
            soundPath.Height = 50;
            soundPanel.Controls.Add(soundPath);

            //Answer text box
            Label answerLabel = new Label();
            //answerLabel.Text = CultureInfo.CurrentUICulture.Name.Equals("ja-JP") ? "答え:" : "Answer:";
            answerLabel.Text = "Answer:";
            answerLabel.Margin = new Padding(20, 5, 0, 0);
            soundPanel.Controls.Add(answerLabel);
            TextBox answerTextBox = new TextBox();
            answerTextBox.Multiline = true;
            answerTextBox.AcceptsReturn = true;
            answerTextBox.Width = 200;
            answerTextBox.Margin = new Padding(20, 0, 20, 0);
            soundPanel.Controls.Add(answerTextBox);

            //Number of points
            Label pointLabel = new Label();
            //pointLabel.Text = CultureInfo.CurrentUICulture.Name.Equals("ja-JP") ? "ポイント: " : "Points: ";
            pointLabel.Text = "Points: ";
            pointLabel.Width = 50;
            pointLabel.Margin = new Padding(20, 0, 0, 20);
            soundPanel.Controls.Add(pointLabel);
            TextBox pointTextBox = new TextBox();
            pointTextBox.Multiline = false;
            pointTextBox.Width = 25;
            pointTextBox.Margin = new Padding(0, 0, 0, 0);
            pointTextBox.KeyPress += num_KeyPress;
            soundPanel.Controls.Add(pointTextBox);

            //Question answered or not
            CheckBox answeredCheckBox = new CheckBox();
            answeredCheckBox.Text = "Answered";
            answeredCheckBox.Margin = new Padding(20, 0, 0, 0);
            answeredCheckBox.Checked = false;
            soundPanel.Controls.Add(answeredCheckBox);
        }
        private void addScreenshot()
        {
            FlowLayoutPanel screenshotPanel = new FlowLayoutPanel();
            screenshotPanel.Width = 200;
            screenshotPanel.Height = 210;
            gamePanel.Controls.Add(screenshotPanel);

            //Screenshot box
            Button imageChooser = new Button();
            imageChooser.Text = selectedType;
            imageChooser.Margin = new Padding(20, 20, 0, 0);
            imageChooser.Width = 75;
            imageChooser.Height = 23;
            imageChooser.Click += new EventHandler(imageChooser_Click);
            screenshotPanel.Controls.Add(imageChooser);
            Label imagePath = new Label();
            imagePath.Margin = new Padding(20, 0, 20, 0);
            imagePath.Width = 200;
            imagePath.Height = 70;
            //TODO: set a blank placeholder
            //Image image = Image.FromFile(picture.question);
            //imagePath.Image = image.GetThumbnailImage(200, 60, null, new System.IntPtr());
            screenshotPanel.Controls.Add(imagePath);

            //Answer text box
            Label answerLabel = new Label();
            //answerLabel.Text = CultureInfo.CurrentUICulture.Name.Equals("ja-JP") ? "答え:" : "Answer:";
            answerLabel.Text = "Answer";
            answerLabel.Margin = new Padding(20, 5, 0, 0);
            screenshotPanel.Controls.Add(answerLabel);
            TextBox answerTextBox = new TextBox();
            answerTextBox.Multiline = true;
            answerTextBox.AcceptsReturn = true;
            answerTextBox.Width = 200;
            answerTextBox.Margin = new Padding(20, 0, 20, 0);
            screenshotPanel.Controls.Add(answerTextBox);

            //Number of points
            Label pointLabel = new Label();
            //pointLabel.Text = CultureInfo.CurrentUICulture.Name.Equals("ja-JP") ? "ポイント: " : "Points: ";
            pointLabel.Text = "Points: ";
            pointLabel.Width = 50;
            pointLabel.Margin = new Padding(20, 0, 0, 0);
            screenshotPanel.Controls.Add(pointLabel);
            TextBox pointTextBox = new TextBox();
            pointTextBox.Multiline = false;
            pointTextBox.Width = 25;
            pointTextBox.Margin = new Padding(0, 0, 0, 0);
            pointTextBox.KeyPress += num_KeyPress;
            screenshotPanel.Controls.Add(pointTextBox);

            //Question answered or not
            CheckBox answeredCheckBox = new CheckBox();
            answeredCheckBox.Text = "Answered";
            answeredCheckBox.Margin = new Padding(20, 0, 0, 0);
            answeredCheckBox.Checked = false;
            screenshotPanel.Controls.Add(answeredCheckBox);
        }
        private void addQuestion(Question question)
        {
            FlowLayoutPanel questionPanel = new FlowLayoutPanel();
            questionPanel.Width = 200;
            questionPanel.Height = 210;
            gamePanel.Controls.Add(questionPanel);

            //Question text box
            Label questionLabel = new Label();
            questionLabel.Text = question.type;
            questionLabel.Margin = new Padding(20, 20, 0, 0);
            questionPanel.Controls.Add(questionLabel);
            TextBox questionTextBox = new TextBox();
            questionTextBox.Multiline = true;
            questionTextBox.AcceptsReturn = true;
            questionTextBox.Text = question.question;
            questionTextBox.Width = 200;
            questionTextBox.Height = 40;
            questionTextBox.Margin = new Padding(20, 0, 20, 0);
            questionPanel.Controls.Add(questionTextBox);

            //Answer text box
            Label answerLabel = new Label();
            //answerLabel.Text = CultureInfo.CurrentUICulture.Name.Equals("ja-JP")?"答え:":"Answer:";
            answerLabel.Text = "Answer";
            answerLabel.Margin = new Padding(20, 5, 0, 0);
            questionPanel.Controls.Add(answerLabel);
            TextBox answerTextBox = new TextBox();
            answerTextBox.Multiline = true;
            answerTextBox.AcceptsReturn = true;
            answerTextBox.Text = question.answer;
            answerTextBox.Width = 200;
            answerTextBox.Margin = new Padding(20, 0, 20, 0);
            questionPanel.Controls.Add(answerTextBox);

            //Number of points
            Label pointLabel = new Label();
            //pointLabel.Text = CultureInfo.CurrentUICulture.Name.Equals("ja-JP")?"ポイント: ":"Points: ";
            pointLabel.Text = "Points: ";
            pointLabel.Width = 50;
            pointLabel.Margin = new Padding(20, 0, 0, 20);
            questionPanel.Controls.Add(pointLabel);
            TextBox pointTextBox = new TextBox();
            pointTextBox.Multiline = false;
            pointTextBox.Text = question.points.ToString();
            pointTextBox.Width = 25;
            pointTextBox.Margin = new Padding(0, 0, 0, 0);
            pointTextBox.KeyPress += num_KeyPress;
            questionPanel.Controls.Add(pointTextBox);

            //Question answered or not
            CheckBox answeredCheckBox = new CheckBox();
            answeredCheckBox.Text = "Answered";
            answeredCheckBox.Margin = new Padding(20, 0, 0, 0);
            answeredCheckBox.Checked = question.answered;
            questionPanel.Controls.Add(answeredCheckBox);
        }
        private void addMusic(Question song)
        {
            FlowLayoutPanel soundPanel = new FlowLayoutPanel();
            soundPanel.Width = 200;
            soundPanel.Height = 210;
            gamePanel.Controls.Add(soundPanel);

            //Screenshot box
            Button soundPicker = new Button();
            soundPicker.Text = song.type;
            soundPicker.Margin = new Padding(20, 20, 0, 0);
            soundPicker.Width = 75;
            soundPicker.Height = 23;
            soundPicker.Click += new EventHandler(soundPicker_Click);
            soundPanel.Controls.Add(soundPicker);
            Label soundPath = new Label();
            soundPath.Text = song.question;
            soundPath.Margin = new Padding(20, 0, 20, 0);
            soundPath.Width = 200;
            soundPath.Height = 50;
            soundPanel.Controls.Add(soundPath);

            //Answer text box
            Label answerLabel = new Label();
            //answerLabel.Text = CultureInfo.CurrentUICulture.Name.Equals("ja-JP") ? "答え:" : "Answer:";
            answerLabel.Text = "Answer:";
            answerLabel.Margin = new Padding(20, 5, 0, 0);
            soundPanel.Controls.Add(answerLabel);
            TextBox answerTextBox = new TextBox();
            answerTextBox.Multiline = true;
            answerTextBox.AcceptsReturn = true;
            answerTextBox.Text = song.answer;
            answerTextBox.Width = 200;
            answerTextBox.Margin = new Padding(20, 0, 20, 0);
            soundPanel.Controls.Add(answerTextBox);

            //Number of points
            Label pointLabel = new Label();
            //pointLabel.Text = CultureInfo.CurrentUICulture.Name.Equals("ja-JP") ? "ポイント: " : "Points: ";
            pointLabel.Text = "Points: ";
            pointLabel.Width = 50;
            pointLabel.Margin = new Padding(20, 0, 0, 20);
            soundPanel.Controls.Add(pointLabel);
            TextBox pointTextBox = new TextBox();
            pointTextBox.Multiline = false;
            pointTextBox.Text = song.points.ToString();
            pointTextBox.Width = 25;
            pointTextBox.Margin = new Padding(0, 0, 0, 0);
            pointTextBox.KeyPress += num_KeyPress;
            soundPanel.Controls.Add(pointTextBox);

            //Question answered or not
            CheckBox answeredCheckBox = new CheckBox();
            answeredCheckBox.Text = "Answered";
            answeredCheckBox.Margin = new Padding(20, 0, 0, 0);
            answeredCheckBox.Checked = song.answered;
            soundPanel.Controls.Add(answeredCheckBox);
        }
        private void addScreenshot(Question picture)
        {
            FlowLayoutPanel screenshotPanel = new FlowLayoutPanel();
            screenshotPanel.Width = 200;
            screenshotPanel.Height = 210;
            gamePanel.Controls.Add(screenshotPanel);

            //Screenshot box
            Button imageChooser = new Button();
            imageChooser.Text = picture.type;
            imageChooser.Margin = new Padding(20, 20, 0, 0);
            imageChooser.Width = 75;
            imageChooser.Height = 23;
            imageChooser.Click += new EventHandler(imageChooser_Click);
            screenshotPanel.Controls.Add(imageChooser);
            Label imagePath = new Label();
            imagePath.Text = picture.question;
            imagePath.Margin = new Padding(20, 0, 20, 0);
            imagePath.Width = 200;
            imagePath.Height = 70;
            Image image = Image.FromFile(picture.question);
            imagePath.Image = image.GetThumbnailImage(200, 60, null, new System.IntPtr());
            screenshotPanel.Controls.Add(imagePath);

            //Answer text box
            Label answerLabel = new Label();
            //answerLabel.Text = CultureInfo.CurrentUICulture.Name.Equals("ja-JP") ? "答え:" : "Answer:";
            answerLabel.Text = "Answer";
            answerLabel.Margin = new Padding(20, 5, 0, 0);
            screenshotPanel.Controls.Add(answerLabel);
            TextBox answerTextBox = new TextBox();
            answerTextBox.Multiline = true;
            answerTextBox.AcceptsReturn = true;
            answerTextBox.Text = picture.answer;
            answerTextBox.Width = 200;
            answerTextBox.Margin = new Padding(20, 0, 20, 0);
            screenshotPanel.Controls.Add(answerTextBox);

            //Number of points
            Label pointLabel = new Label();
            //pointLabel.Text = CultureInfo.CurrentUICulture.Name.Equals("ja-JP") ? "ポイント: " : "Points: ";
            pointLabel.Text = "Points: ";
            pointLabel.Width = 50;
            pointLabel.Margin = new Padding(20, 0, 0, 0);
            screenshotPanel.Controls.Add(pointLabel);
            TextBox pointTextBox = new TextBox();
            pointTextBox.Multiline = false;
            pointTextBox.Text = picture.points.ToString();
            pointTextBox.Width = 25;
            pointTextBox.Margin = new Padding(0, 0, 0, 0);
            pointTextBox.KeyPress += num_KeyPress;
            screenshotPanel.Controls.Add(pointTextBox);

            //Question answered or not
            CheckBox answeredCheckBox = new CheckBox();
            answeredCheckBox.Text = "Answered";
            answeredCheckBox.Margin = new Padding(20, 0, 0, 0);
            answeredCheckBox.Checked = picture.answered;
            screenshotPanel.Controls.Add(answeredCheckBox);
        }

        private bool storeData()
        {
            questionSet = new QuestionSet();

            //For each questionPanel in gamePanel, get each Question and save it.
            for (int i = 0; i < gamePanel.Controls.Count; i++)
            {
                //Check if there is an empty question and skips it
                if (gamePanel.Controls[i].Controls[1].Text == string.Empty &&
                    gamePanel.Controls[i].Controls[3].Text == string.Empty &&
                    gamePanel.Controls[i].Controls[5].Text == string.Empty) { /*do nothing*/}
                //Check if there is an empty field and return an error.
                else if (gamePanel.Controls[i].Controls[1].Text == string.Empty ||
                    gamePanel.Controls[i].Controls[3].Text == string.Empty ||
                    gamePanel.Controls[i].Controls[5].Text == string.Empty)
                {
                    //if (CultureInfo.CurrentUICulture.Name.Equals("ja-JP"))
                    //    MessageBox.Show("エラー：空のフィールドが有ります。全ての質問を確認してください", "エラー", MessageBoxButtons.OK);
                    //else 
                    MessageBox.Show("FAIL: One of the fields is empty. Please check all the questions and retry", 
                        "Empty text error", MessageBoxButtons.OK);
                    return false;
                }
                else
                {
                    string question = (string)gamePanel.Controls[i].Controls[1].Text;
                    string answer = (string)gamePanel.Controls[i].Controls[3].Text;
                    string type = (string)gamePanel.Controls[i].Controls[0].Text;
                    int point = Convert.ToInt32(gamePanel.Controls[i].Controls[5].Text);
                    bool answered = ((CheckBox)gamePanel.Controls[i].Controls[6]).Checked;
                    questionSet.Add(new Question(question, answer, type, point, answered));
                }
            }
            return true;
        }
        private void saveData(string filename)
        {
            //If the data has been correctly stored, write it to file.
            if (storeData())
            {
                //XmlRootAttribute xRoot = new XmlRootAttribute(selectedType);
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                Type[] newTypes = new Type[2];
                newTypes[0] = typeof(Question);
                newTypes[1] = typeof(ArrayList);

                // Note that only the collection is serialized -- not the 
                // QuestionSetType or any other public property of the class.
                XmlSerializer serializer = new XmlSerializer(typeof(QuestionSet),newTypes);
                TextWriter stream = new StreamWriter(filename);
                serializer.Serialize(stream, questionSet,ns);
                stream.Close();
                Settings.Default.saveState = true;
                //cancelBtn.Text = CultureInfo.CurrentUICulture.Name.Equals("ja-JP")?"閉じる":"Close";
                cancelBtn.Text = "Close";
            }
            else Settings.Default.currentFile = String.Empty;
        }
        
        #region Behaviors

        private void saveAsBehavior()
        {
            if (gameSave.ShowDialog() == DialogResult.OK)
            {
                Settings.Default.currentFile = gameSave.FileName;
                saveData(Settings.Default.currentFile);
            }
        }
        //If there are no currentFile, prompt for SaveAs
        private void saveBehavior()
        {
            if (Settings.Default.currentFile == String.Empty) saveAsBehavior();
            else saveData(Settings.Default.currentFile);
            updateRecentFiles();
        }
        private void loadBehavior()
        {
            clearPanel();
            XmlSerializer serializer = new XmlSerializer(typeof(QuestionSet));
            //FileStream ReadFileStream = new FileStream(@gameLoad.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            FileStream ReadFileStream = new FileStream(Settings.Default.currentFile, FileMode.Open, FileAccess.Read, FileShare.Read);
            questionSet = (QuestionSet)serializer.Deserialize(ReadFileStream);
            ReadFileStream.Close();
            //Change the saveState to false
            Settings.Default.saveState = false;
            cancelBtn.Text = "Close";
            //Create the number of specified question boxes for the selected game type.          
            gamePanel.Location = new Point(12, 56);
            gamePanel.AutoScroll = true;
            gamePanel.Width = ClientRectangle.Width - 20;
            gamePanel.Height = ClientRectangle.Height - 64;
            Controls.Add(gamePanel);
            //Get the type
            selectedType = questionSet[0].type;
            switch (selectedType)
            {
                case "Question":
                    foreach (Question question in questionSet)
                        addQuestion(question);
                    break;
                case "Music":
                    foreach (Question song in questionSet)
                        addMusic(song);
                    break;
                case "Screenshot":
                    foreach (Question picture in questionSet)
                        addScreenshot(picture);
                    break;
            }
            setAddRemoveGenBtn(true);
        }
        void clearPanel()
        {
            while (gamePanel.Controls.Count > 0) gamePanel.Controls.Clear();
        }
        void setAddRemoveGenBtn(bool state)
        {
            addBtn.Enabled = state;
            removeBtn.Enabled = state;
            genBtn.Enabled = !state;
        }
        private void updateRecentFiles()
        {
            //Add the file to the list of recent files
            if (Settings.Default.recentFiles.Contains(Settings.Default.currentFile))
                Settings.Default.recentFiles.Remove(Settings.Default.currentFile);
            //Remove the oldest file if the list is full
            else if (Settings.Default.recentFiles.Count == 10) Settings.Default.recentFiles.RemoveAt(0);
            Settings.Default.recentFiles.Add(Settings.Default.currentFile);
        }
        private bool isSafeOverwrite(string message)
        {
            if (!Settings.Default.saveState)
            {
                DialogResult confirm = new DialogResult();
                //confirm = MessageBox.Show(message, CultureInfo.CurrentUICulture.Name.Equals("ja-JP")?"編集中のゲーム":"Unsaved changes", MessageBoxButtons.YesNoCancel);
                confirm = MessageBox.Show(message, "Unsaved changes", MessageBoxButtons.YesNoCancel);
                if (confirm == DialogResult.Cancel) return false;
                else if (confirm == DialogResult.Yes) saveBehavior();
            }
            return true;
        }
        private bool isSafeOverwrite(string message, MessageBoxButtons buttons)
        {
            if (!Settings.Default.saveState)
            {
                DialogResult confirm = new DialogResult();
                //confirm = MessageBox.Show(message, CultureInfo.CurrentUICulture.Name.Equals("ja-JP") ? "編集中のゲーム" : "Unsaved changes", buttons);
                confirm = MessageBox.Show(message, "Unsaved changes", buttons);
                if (confirm == DialogResult.Cancel) return false;
                else if (confirm == DialogResult.Yes) saveBehavior();
            }
            return true;
        }

        #endregion

        #region EventHandlers

        void soundPicker_Click(object sender, EventArgs e)
        {
            OpenFileDialog soundFile = new OpenFileDialog();
            //If a default directory has been defined for the soundPicker, set it
            if (Settings.Default.defaultMusicFolder != String.Empty) 
                soundFile.InitialDirectory = Settings.Default.defaultMusicFolder;
            //soundFile.Filter = "MP3 files (*.mp3)|*.mp3|MP4 files (*.mp4)|*.mp4|WAV files (*.wav)|*.wav|WMA files (*.wma)|*.wma";
            soundFile.Filter = "Music Formats|" +
                    "*.mp3;*.ram;*.rm;*.wav;*.wma;*.mid;*.mp4|" +
                    "mp3 (*.mp3)|*.mp3|ram (*.ram)|*.ram|rm (*.rm)|*.rm|" +
                    "wav (*.wav)|*.wav|wma (*.wma)|*.wma|mid (*.mid)|*.mid|" +
                    "mp4 (*.mp4)|*.mp4";
            if (soundFile.ShowDialog() == DialogResult.OK)
            {
                string filename = soundFile.FileName;
                Control soundButton = (Control)sender;
                Label soundPath = (Label)soundButton.Parent.Controls[1];
                soundPath.Text = filename;
            }
        }
        void imageChooser_Click(object sender, EventArgs e)
        {
            OpenFileDialog imageFile = new OpenFileDialog();
            //If a default folder has been defined in Settings, start the dialog in that folder
            if (Settings.Default.defaultScreenshotFolder != String.Empty)
                imageFile.InitialDirectory = Settings.Default.defaultScreenshotFolder;
            imageFile.Filter = "JPG files (*.jpg)|*.jpg|PNG files (*.png)|*.png|BMP files (*.bmp)|*.bmp";
            if (imageFile.ShowDialog() == DialogResult.OK)
            {
                string filename = imageFile.FileName;
                Control imageButton = (Control)sender;
                Label imagePath = (Label)imageButton.Parent.Controls[1];
                imagePath.Text = filename;
                Image image = Image.FromFile(filename);
                imagePath.Image = image.GetThumbnailImage(200, 40, null, new System.IntPtr());
            }
        }

        private void genBtn_Click(object sender, EventArgs e)
        {
            selectedType = (string)gameType.SelectedItem;
            numQuest = Convert.ToInt32(numQuestions.Text);
            reinitializeGameBoard();
        }
        private void clearBtn_Click(object sender, EventArgs e)
        {
            //If there are unsaved changes, prompt to save.
            if (isSafeOverwrite("This will erase any unsaved changes. Would you like to save?"))
            {
                //Then clear the game board and reset saveState
                //Also chear the currentFile setting to effectively start a new file
                clearPanel();
                Settings.Default.saveState = true;
                Settings.Default.currentFile = String.Empty;
                setAddRemoveGenBtn(false);
            }
        }
        private void addBtn_Click(object sender, EventArgs e)
        {
            switch (selectedType)
            {
                case "Question":
                    addQuestion();
                    break;
                case "Music":
                    addMusic();
                    break;
                case "Screenshot":
                    addScreenshot();
                    break;
            }
        }
        private void removeBtn_Click(object sender, EventArgs e)
        {
            //Remove the last Question
            gamePanel.Controls[gamePanel.Controls.Count - 1].Dispose();
        }
        private void uncheckBtn_Click(object sender, EventArgs e)
        {
            //If a checkbox is checked, uncheck it.
            for (int i = 0; i < gamePanel.Controls.Count; i++)
            {
                CheckBox testBox = (CheckBox)gamePanel.Controls[i].Controls[6];
                if (testBox.Checked) testBox.Checked = false;
            }
        }
        private void num_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                e.Handled = true;
        }

        private void saveAsBtn_Click(object sender, EventArgs e)
        {
            saveAsBehavior();
        }
        private void saveBtn_Click(object sender, EventArgs e)
        {
            saveBehavior();
        }
        private void loadBtn_Click(object sender, EventArgs e)
        {
            if (!isSafeOverwrite("There are unsaved changes. Do you want to save them before loading another game?"))
                return;
            if (gameLoad.ShowDialog() == DialogResult.OK)
            {
                Settings.Default.currentFile = gameLoad.FileName;
                loadBehavior();
                updateRecentFiles();
            }           
        }
        private void cancelBtn_Click(object sender, EventArgs e)
        {
            if (isSafeOverwrite("There are unsaved changes. Do you want to save them before closing this form?"))
            {
                Settings.Default.saveState = true;
                this.Close();
            }
        }

        private void GameEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            isSafeOverwrite("Really quit? How about saving unsaved changes?", MessageBoxButtons.YesNo);
            Settings.Default.saveState = true;
        }

        #endregion

        
    }
}
