using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Anime_Quiz_3.Classes;
using Anime_Quiz_3.Properties;
using Devart.Data.Linq;
using GameContext;

namespace Anime_Quiz_3.GameMaster
{
    /// <summary>
    /// Interaction logic for QuestionSetEditor.xaml
    /// </summary>
    public partial class QuestionSetEditor : Page
    {
        // Data
        static GameDataContext db;
        static Table<QuestionSets> questionSets;
        static IQueryable<Questions> questions;

        public QuestionSetEditor()
        {
            InitializeComponent();
            db = new GameDataContext();
            
            populateQuestionSetSelector();
            populateTypeComboBox();
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
            questionSets = db.GetTable<QuestionSets>();
            var questionSetList =
                from questionSet in questionSets
                select questionSet.Name;
            questionSetComboBox.ItemsSource = questionSetList;
        }     
        private void questionSetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            saveQuestions();
            if ((sender as ComboBox).SelectedIndex > -1)
            {
                CurrentQuestionSet.setInstance((from questionSet in questionSets
                                                where questionSet.Name.Equals((sender as ComboBox).SelectedValue.ToString())
                                                select questionSet).Single());
                questions = from question in db.GetTable<Questions>()
                            where question.QuestionSets.Name.Equals(CurrentQuestionSet.getInstance().Name)
                            select question;
                questionSetDataGrid.ItemsSource = questions;
                questionSetDataGrid.Visibility = System.Windows.Visibility.Visible;

                loadEasterEggMedia((Types) CurrentQuestionSet.getInstance().Type);
            }
            else
                CurrentQuestionSet.setInstance(null);

            setDelUncheckButtons(true);
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
                db.SubmitChanges();
                var changedQuestions = from question in db.GetTable<Questions>()
                                       where question.QuestionSetId == 0
                                       select question;
                foreach (Questions changedQuestion in changedQuestions)
                {
                    changedQuestion.QuestionSetId = CurrentQuestionSet.getInstance().QuestionSetId;
                    changedQuestion.QuestionSets = CurrentQuestionSet.getInstance();
                }
                db.SubmitChanges();
            }
        }

        void openFilePicker(TextBox targetCell)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            switch (CurrentQuestionSet.getInstance().Type)
            {
                case (int) Types.Music:
                    if (Settings.Default.defaultMusicFolder != String.Empty)
                        openFileDialog.InitialDirectory = Settings.Default.defaultMusicFolder;
                    openFileDialog.Filter = "Music Formats|" +
                        "*.mp3;*.ram;*.rm;*.wav;*.wma;*.mid;*.mp4|" +
                        "mp3 (*.mp3)|*.mp3|ram (*.ram)|*.ram|rm (*.rm)|*.rm|" +
                        "wav (*.wav)|*.wav|wma (*.wma)|*.wma|mid (*.mid)|*.mid|" +
                        "mp4 (*.mp4)|*.mp4";
                    break;
                case (int) Types.Screenshot:
                    if (Settings.Default.defaultScreenshotFolder != String.Empty)
                        openFileDialog.InitialDirectory = Settings.Default.defaultScreenshotFolder;
                    openFileDialog.Filter = "JPG files (*.jpg)|*.jpg|PNG files (*.png)|*.png|BMP files (*.bmp)|*.bmp";
                    break;
            }
            if (openFileDialog.ShowDialog() ?? false)
                targetCell.Text = openFileDialog.FileName;
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
        private void questionSetDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            // Stop whatever preview has been running
            try { musicPreview.Stop(); }
            catch { }
            musicPreview.Visibility = System.Windows.Visibility.Collapsed;
            screenshotPreviewImage.Visibility = System.Windows.Visibility.Collapsed;

            if (e.AddedCells.Count != 1)
                return;

            if (e.AddedCells[0].Column.Header.Equals("Question")
                && CurrentQuestionSet.getInstance().Type != (int)Types.Question)
            {
                TextBox currentCell = e.AddedCells[0].Column.GetCellContent(e.AddedCells[0].Item) as TextBox;
                if (currentCell.Text == String.Empty)
                    openFilePicker(currentCell);
                else
                    displayMedia(currentCell.Text);
            }
        }

        #endregion

        #region Buttons
        private void delBtn_Click(object sender, RoutedEventArgs e)
        {
            db.QuestionSets.DeleteOnSubmit(CurrentQuestionSet.getInstance());
            var questionsToDelete = from question in questions where question.QuestionId == CurrentQuestionSet.getInstance().QuestionSetId select question;
            db.Questions.DeleteAllOnSubmit(questionsToDelete);
            CurrentQuestionSet.setInstance(null);

            db.SubmitChanges();
            populateQuestionSetSelector();
            questionSetDataGrid.Visibility = System.Windows.Visibility.Hidden;

            setDelUncheckButtons(false);
        }

        private void renameBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentQuestionSet.getInstance().Name = renameTextBox.Text;
            db.SubmitChanges();

            populateQuestionSetSelector();
            questionSetComboBox.SelectedItem = renameTextBox.Text;
            renameTextBox.Text = String.Empty;
        }

        private void uncheckBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (Questions question in questions)
                question.Answered = false;
            db.SubmitChanges();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            QuestionSets newQuestionSet = new QuestionSets();
            newQuestionSet.Name = questionSetTextBox.Text;
            newQuestionSet.Type = questionSetTypeComboBox.SelectedIndex;
            db.QuestionSets.InsertOnSubmit(newQuestionSet);
            db.SubmitChanges();

            populateQuestionSetSelector();
            questionSetComboBox.SelectedItem = newQuestionSet.Name;

            questionSetTextBox.Text = String.Empty;
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
