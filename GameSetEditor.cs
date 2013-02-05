﻿using System;
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
using System.Reflection;
//using System.Globalization;
using System.Data.SQLite;
using Microsoft.VisualBasic;

namespace Anime_Quiz
{
    public partial class GameSetEditor : Form
    {
        //new gameset
        SQLiteDatabase sqlDB = new SQLiteDatabase();

        ComboBox questionSetList;
        DataGridView questionGridView;

        //QuestionSet questionSet;
        Question blankQuestion = new Question();
        Types selectedType;

        FlowLayoutPanel gamePanel = new FlowLayoutPanel();  //to deprecate?

        public GameSetEditor()
        {
            InitializeComponent();
            loadQuestionSets();
            if (Settings.Default.currentSet != null)
            {
                CurrentQuestionSet.setInstance(Settings.Default.currentSet);
                loadQuestions();
            }
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
        private void loadQuestions()
        {
            Controls.Remove(questionGridView);
            QuestionSet instance = CurrentQuestionSet.getInstance();

            ArrayList questionsArray = new ArrayList();
            questionsArray = instance.getQuestions();
            
            //DataTable queryData = instance.getQuestionDataTable();
            questionGridView = new DataGridView();
            questionGridView.Location = new Point(12, 75);
            questionGridView.Width = 1024;  //todo: auto-size
            //questionGridView.DataSource = queryData;
            questionGridView.DataSource = questionsArray;
            //questionGridView.CellFormatting += questionGridView_CellFormatting;
            Controls.Add(questionGridView);
            
            clrBtn.Enabled = true;
            delBtn.Enabled = true;
        }
        #endregion

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
                sqlDB.ClearTable(CurrentQuestionSet.getInstance().name);
                clrBtn.Enabled = false;
                loadQuestionSets();
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
            Types newSetType = (Types)(gameType.SelectedIndex + 1);
            Dictionary<String, String> data = new Dictionary<string, string>();
            data.Add("name", newSetName);
            data.Add("type", ((int)newSetType).ToString());
            if (sqlDB.Insert("QuestionSets", data))
            {
                loadQuestionSets();
                CurrentQuestionSet.getInstance().saveQuestions();
                CurrentQuestionSet.setInstance(new QuestionSet(newSetName, newSetType));
                loadQuestions();
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
                    loadQuestionSets();
                }
                else
                    throw new ArgumentNullException("Name cannot be blank.");
            }
            catch (Exception crap)
            {
                MessageBox.Show(crap.Message);
            }
        }

        private void uncheckBtn_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        private void cancelBtn_Click(object sender, EventArgs e)
        {
            if(!CurrentQuestionSet.getInstance().saveQuestions())
            {
                if (MessageBox.Show("There was an error saving to the database. Quit anyways?", "Save Error", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
            }
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
            // Save the current selection
            ComboBox senderComboBox = sender as ComboBox;
            String questionSetName = senderComboBox.SelectedItem.ToString();
            Types questionSetType = sqlDB.getQuestionSetType(questionSetName);
            CurrentQuestionSet.setInstance(new QuestionSet(questionSetName, questionSetType));

            // Load the Questions
            loadQuestions();
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
        
        private void num_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                e.Handled = true;
        }    

        private void GameEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.saveState = true;
        }
        /*
        /// <summary>
        ///     Format the cell for the question column, which is of type blob
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void questionGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((sender as DataGridView).Columns[e.ColumnIndex].Name == "question")
            {
                if (e.Value != null)
                {
                    e.Value = e.Value.ToString();
                }
            }
        }*/

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
        #endregion  
    }
}