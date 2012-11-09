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
            if (Settings.Default.reloadPrevious && Settings.Default.currentFile != String.Empty && loadFileBehavior(Settings.Default.currentFile))
                loadGameBehavior();
        }
        
        #region Behaviors
        private void saveData(string filename)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            Type[] newTypes = new Type[2];
            newTypes[0] = typeof(Question);
            newTypes[1] = typeof(ArrayList);

            XmlSerializer serializer = new XmlSerializer(typeof(QuestionSet), newTypes);
            TextWriter stream = new StreamWriter(filename);
            serializer.Serialize(stream, questionSet, ns);
            stream.Close();
            Settings.Default.saveState = true;
        }
        private void saveAsBehavior()
        {
            SaveFileDialog gameSave = new SaveFileDialog();
            if (Settings.Default.defaultFolder != String.Empty) gameSave.InitialDirectory = Settings.Default.defaultFolder;
            gameSave.Filter = "XML Files (*.xml)|*.xml";
            gameSave.DefaultExt = "xml";
            if (gameSave.ShowDialog() == DialogResult.OK)
            {
                Settings.Default.currentFile = gameSave.FileName;
                saveData(Settings.Default.currentFile);
            }
        }
        //If there are no currentFile, prompt for SaveAs
        public void saveBehavior()
        {
            if (Settings.Default.currentFile == String.Empty) saveAsBehavior();
            else saveData(Settings.Default.currentFile);
            //updateRecentFiles();
        }
        public bool loadFileBehavior(string filename)
        {
            //Clear the game board
            clearGamePanel();
            //if (!Settings.Default.saveState &&
            //    isSafeOverwrite(CultureInfo.CurrentUICulture.Name.Equals("ja-JP") ? "本ゲームは行い中です。新しいゲームをロードする前に保存しますか？" : "There is a game currently loaded. Save it before loading another one?"))
            if (!Settings.Default.saveState && isSafeOverwrite("There is a game currently loaded. Save it before loading another one?"))
                while (gamePanel.Controls.Count > 0) gamePanel.Controls.Clear();
            else if (Settings.Default.saveState) while (gamePanel.Controls.Count > 0) gamePanel.Controls.Clear();
            try
            {
                Settings.Default.currentFile = filename;
                XmlSerializer serializer = new XmlSerializer(typeof(QuestionSet));
                FileStream ReadFileStream = new FileStream(Settings.Default.currentFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                questionSet = (QuestionSet)serializer.Deserialize(ReadFileStream);
                ReadFileStream.Close();
                //updateRecentFiles();
                return true;
            }
            catch (Exception e)
            {
                //TODO: for some reason this always throws an exception but works fine
                //MessageBox.Show(e.Message);
                return true;
            }
        }
        public void loadGameBehavior()
        {
            gamePanel.Location = new Point(12, 75);
            gamePanel.AutoScroll = true;
            gamePanel.Width = ClientRectangle.Width - 20;
            gamePanel.Height = ClientRectangle.Height - 60;
            Controls.Add(gamePanel);

            //Add a button to save game
            /*Button saveGameBtn = new Button();
            saveGameBtn.Text = "Save";
            saveGameBtn.Location = new Point(12, 38);
            saveGameBtn.Click += new EventHandler(saveGameBtn_Click);
            Controls.Add(saveGameBtn);

            //Add a button to refresh game
            Button refreshGameBtn = new Button();
            refreshGameBtn.Text = "Refresh";
            refreshGameBtn.Location = new Point(100, 38);
            refreshGameBtn.Click += new EventHandler(refreshGameBtn_Click);
            Controls.Add(refreshGameBtn);*/

            //Add a button for a random Question picker
            /*Button randomPickBtn = new Button();
            //if (CultureInfo.CurrentUICulture.Name.Equals("ja-JP")) randomPickBtn.Text = "ランダム";
            //else randomPickBtn.Text = "Random";
            randomPickBtn.Text = "Random";
            randomPickBtn.Location = new Point(12, 38);
            randomPickBtn.Click += new EventHandler(randomPickBtn_Click);
            Controls.Add(randomPickBtn);*/

            //Load the actual question labels
            loadLabelBehavior();
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
            Settings.Default.tempQuestion = questionSet[index].question;
            Settings.Default.tempType = questionSet[index].type;

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
        private bool isSafeOverwrite(string message)
        {
            if (!Settings.Default.saveState)
            {
                DialogResult confirm = new DialogResult();
                confirm = MessageBox.Show(message, "Unsaved changes", MessageBoxButtons.YesNoCancel);
                if (confirm == DialogResult.Cancel) return false;
                else if (confirm == DialogResult.Yes) saveBehavior();
            }
            return true;
        }
        #endregion
        
        #region EventHandlers

        void questionForm_FormClosed(object sender, FormClosedEventArgs e, bool answered, int index)
        {
            //After the question has been answered, get information
            Settings.Default.saveState = false;
            questionSet[index].answered = answered;
            //Autosave
            saveBehavior();
            //Reload the form
            loadLabelBehavior();
        }
        /*void randomPickBtn_Click(object sender, EventArgs e)
        {
            //Check if there are still unansweredQuestions.
            //If there aren't any, return.
            int counter = 0;
            for (int i = 0; i < questionSet.Count; i++)
                if (!questionSet[i].answered) counter++;
            if (counter == 0)
            {
                if (CultureInfo.CurrentUICulture.Name.Equals("ja-JP"))
                    MessageBox.Show("エラー：答えてない質問は一つも残っていない。\nβακα…_〆(゜▽゜*)(((;゜Д゜))", "エラー", MessageBoxButtons.OK);
                else MessageBox.Show("FAIL: There are no more unanswered question!", "Epic Fail", MessageBoxButtons.OK);
                return;
            }
            
            //Pick a random index to open.
            Random rand = new Random();
            int selectedIndex = rand.Next(0, questionSet.Count);
            while (questionSet[selectedIndex].answered)
            {
                selectedIndex = rand.Next(0, questionSet.Count);
            }
            openQuestion(selectedIndex);
        }*/
        /*void saveGameBtn_Click(object sender, EventArgs e)
        {
            saveBehavior();
        }
        //Note: this method has a bug where if we came back from the game editor and press the refresh button,
        // it will save the current game, overwriting the edited version.
        void refreshGameBtn_Click(object sender, EventArgs e)
        {
            saveBehavior();
            loadLabelBehavior();
        }*/
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
            //if (CultureInfo.CurrentUICulture.Name.Equals("ja-JP")) isSafeOverwrite("本ゲームはセブされていない。ゲームデータをセブしますか？");
            //else isSafeOverwrite("There are unsaved changes. Do you want to save them before closing?");
            isSafeOverwrite("There are unsaved changes. Do you want to save them before closing?");
        }
    }
}
