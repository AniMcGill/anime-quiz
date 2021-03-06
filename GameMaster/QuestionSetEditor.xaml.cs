﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Anime_Quiz_3.Classes;
using Anime_Quiz_3.Properties;
using GameContext;

namespace Anime_Quiz_3.GameMaster
{
    /// <summary>
    /// Interaction logic for QuestionSetEditor.xaml
    /// </summary>
    public partial class QuestionSetEditor : Page
    {
        static IQueryable<Questions> questions;

        public QuestionSetEditor()
        {
            InitializeComponent();
            
            populateQuestionSetSelector();
            populateTypeComboBox();
            this.GotFocus += QuestionSetEditor_GotFocus;
        }

        #region QuestionSets
        void populateTypeComboBox()
        {
            questionSetTypeComboBox.Items.Add(Types.Question.ToString());
            questionSetTypeComboBox.Items.Add(Types.Music.ToString());
            questionSetTypeComboBox.Items.Add(Types.Screenshot.ToString());
        }
        void populateQuestionSetSelector()
        {
            var questionSetList =
                from questionSet in App.questionSets
                select questionSet.Name;
            questionSetComboBox.ItemsSource = questionSetList;
            if (CurrentQuestionSet.getInstance() != null)
                questionSetComboBox.SelectedItem = CurrentQuestionSet.getInstance().Name;
        }

        void populateQuestionSetDataGrid()
        {
            questions = from question in App.db.GetTable<Questions>()
                        where question.QuestionSets.Name.Equals(CurrentQuestionSet.getInstance().Name)
                        select question;
            questionSetDataGrid.ItemsSource = questions;
            questionSetDataGrid.Visibility = System.Windows.Visibility.Visible;
        }
        private void questionSetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            saveQuestions();
            if ((sender as ComboBox).SelectedIndex > -1)
            {
                CurrentQuestionSet.setInstance((from questionSet in App.questionSets
                                                where questionSet.Name.Equals((sender as ComboBox).SelectedValue.ToString())
                                                select questionSet).Single());
                populateQuestionSetDataGrid();
                setDelUncheckButtons(true);

                loadEasterEggMedia((Types)CurrentQuestionSet.getInstance().Type);
            }
            else
            {
                CurrentQuestionSet.setInstance(null);
                questionSetDataGrid.Visibility = System.Windows.Visibility.Hidden;
                setDelUncheckButtons(false);
            }
        }

        private void renameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            renameBtn.IsEnabled = (sender as TextBox).Text.Length > 0 && questionSetComboBox.SelectedIndex > -1;
        }
        private void setDelUncheckButtons(bool state)
        {
            delBtn.IsEnabled = state;
            uncheckBtn.IsEnabled = state;
        }   

        private void questionSetTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            addBtn.IsEnabled = (sender as TextBox).Text.Length > 0 && questionSetComboBox.SelectedIndex > -1;
        }
        private void questionSetTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!questionSetTextBox.Text.Equals(String.Empty))
                addBtn.IsEnabled = true;
        }
        #endregion

        #region Questions
        void saveQuestions()
        {
            if (CurrentQuestionSet.getInstance() != null)
            {
                cleanRows();
                App.db.SubmitChanges();

                var changedQuestions = from question in App.db.GetTable<Questions>()
                                       where question.QuestionSetId == 0
                                       select question;
                foreach (Questions changedQuestion in changedQuestions)
                {
                    changedQuestion.QuestionSetId = CurrentQuestionSet.getInstance().QuestionSetId;
                    changedQuestion.QuestionSets = CurrentQuestionSet.getInstance();
                }
                App.db.SubmitChanges();
            }
        }
        void cleanRows()
        {
            if (App.db.HasErrors)
            {
                questionSetDataGrid.Items.RemoveAt(questionSetDataGrid.Items.Count - 1);
            }
        }

        void pickFile()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            switch (CurrentQuestionSet.getInstance().Type)
            {
                case (int)Types.Music:
                    if (Settings.Default.defaultMusicFolder != String.Empty)
                        openFileDialog.InitialDirectory = Settings.Default.defaultMusicFolder;
                    openFileDialog.Filter = "Music Formats|" +
                        "*.mp3;*.ram;*.rm;*.wav;*.wma;*.mid;*.mp4|" +
                        "mp3 (*.mp3)|*.mp3|ram (*.ram)|*.ram|rm (*.rm)|*.rm|" +
                        "wav (*.wav)|*.wav|wma (*.wma)|*.wma|mid (*.mid)|*.mid|" +
                        "mp4 (*.mp4)|*.mp4";
                    break;
                case (int)Types.Screenshot:
                    if (Settings.Default.defaultScreenshotFolder != String.Empty)
                        openFileDialog.InitialDirectory = Settings.Default.defaultScreenshotFolder;
                    openFileDialog.Filter = "JPG files (*.jpg)|*.jpg|PNG files (*.png)|*.png|BMP files (*.bmp)|*.bmp";
                    break;
            }
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() ?? false)
            {
                Questions newQuestion = new Questions();
                newQuestion.Question = openFileDialog.FileName;
                newQuestion.Answer = "";

                App.db.Questions.InsertOnSubmit(newQuestion);
                App.db.SubmitChanges();
                
                saveQuestions();
                populateQuestionSetDataGrid();
            }
        }
        
        void displayMedia(String filename)
        {
            switch (CurrentQuestionSet.getInstance().Type)
            {
                case (int) Types.Music:
                    musicPreview.Source = new Uri(filename, UriKind.RelativeOrAbsolute);
                    musicPreview.Visibility = System.Windows.Visibility.Visible;
                    musicPreview.Play();
                    break;
                case (int) Types.Screenshot:
                    screenshotPreviewImage.Source = new BitmapImage(new Uri(filename, UriKind.RelativeOrAbsolute));
                    screenshotPreviewImage.Visibility = System.Windows.Visibility.Visible;
                    break;
            }
        }
        void hideMedia()
        {
            // Stop whatever preview has been running
            try { musicPreview.Stop(); }
            catch { }
            musicPreview.Visibility = System.Windows.Visibility.Collapsed;
            screenshotPreviewImage.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void questionSetDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count != 1)
                return;

            if (e.AddedCells[0].Column.Header.Equals("Question") &&
                CurrentQuestionSet.getInstance().Type != (int)Types.Question)
            {
                hideMedia();

                TextBlock currentCell = e.AddedCells[0].Column.GetCellContent(e.AddedCells[0].Item) as TextBlock;
                
                if (currentCell.Text == String.Empty)
                    pickFile();
                else
                    displayMedia(currentCell.Text.ToString());
            }
        }
        #endregion

        #region Buttons
        private void delBtn_Click(object sender, RoutedEventArgs e)
        {
            App.db.QuestionSets.DeleteOnSubmit(CurrentQuestionSet.getInstance());
            var questionsToDelete = from question in questions where question.QuestionId == CurrentQuestionSet.getInstance().QuestionSetId select question;
            App.db.Questions.DeleteAllOnSubmit(questionsToDelete);
            CurrentQuestionSet.setInstance(null);

            App.db.SubmitChanges();
            App.refreshDb(App.questionSets);
            populateQuestionSetSelector();
        }

        private void renameBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentQuestionSet.getInstance().Name = renameTextBox.Text;
            App.db.SubmitChanges();
            App.refreshDb(App.questionSets);

            populateQuestionSetSelector();
            questionSetComboBox.SelectedItem = renameTextBox.Text;
            renameTextBox.Text = String.Empty;
        }

        private void uncheckBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (Questions question in questions)
                question.Answered = false;
            App.db.SubmitChanges();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            QuestionSets newQuestionSet = new QuestionSets();
            newQuestionSet.Name = questionSetTextBox.Text;
            newQuestionSet.Type = questionSetTypeComboBox.SelectedIndex;
            App.db.QuestionSets.InsertOnSubmit(newQuestionSet);
            App.db.SubmitChanges();
            App.refreshDb(App.questionSets);

            populateQuestionSetSelector();
            questionSetComboBox.SelectedItem = newQuestionSet.Name;

            questionSetTextBox.Text = String.Empty;
        }

        void QuestionSetEditor_GotFocus(object sender, RoutedEventArgs e)
        {
            closeBtn.IsEnabled = this.NavigationService.CanGoBack;
        }
        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            saveQuestions();
            this.NavigationService.GoBack();
        }
        #endregion

        #region Easter Eggs
        void loadEasterEggMedia(Types questionType)
        {
            screenshotPreviewImage.Visibility = System.Windows.Visibility.Collapsed;
            if (questionType == Types.Music)
            {
                musicPreview.Source = new Uri("/Resources/wakamoto_hellonyan.wav", UriKind.Relative);
                musicPreview.Play();    // doesn't work yet
            }
            else if (questionType == Types.Screenshot)
            {
                screenshotPreviewImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/maedax.png"));
                screenshotPreviewImage.Visibility = System.Windows.Visibility.Visible;
            }
        }
        #endregion
    }
}
