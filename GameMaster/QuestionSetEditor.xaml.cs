using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
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
            questionSetDataGrid.CellEditEnding += HandleEditEnding;
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
            if (CurrentQuestionSet.getInstance() != null)
                questionSetComboBox.SelectedItem = CurrentQuestionSet.getInstance().Name;
        }
        void populateQuestionSetDataGrid()
        {
            questions = from question in db.GetTable<Questions>()
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
                CurrentQuestionSet.setInstance((from questionSet in questionSets
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
        void cleanRows()
        {
            if (db.HasErrors)
            {
                questionSetDataGrid.Items.RemoveAt(questionSetDataGrid.Items.Count - 1);
            }
        }
        String pickFile()
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
            if (openFileDialog.ShowDialog() ?? false)
                return openFileDialog.FileName;
            else
                return String.Empty;
        }

        // to remove

        /// <summary>
        ///     Get the current cell
        /// </summary>
        /// <see cref="http://stackoverflow.com/questions/1764498/wpf-datagrid-programmatically-editing-a-cell"/>
        static DataGridCell GetCurrentCell(DataGrid grid, DataGridCellInfo cellInfo)
        {
            DataGridRow currentRow = (DataGridRow)grid.ItemContainerGenerator.ContainerFromItem(cellInfo.Item);
            DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(currentRow);
            int columnIndex = grid.Columns.IndexOf(cellInfo.Column);
            return presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex) as DataGridCell;
        }
        /// <summary>
        ///     
        /// </summary>
        /// <see cref="http://wpf.codeplex.com/discussions/34542"/>
        static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
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
            /*
            if (questionSetDataGrid.CurrentColumn.Header.Equals("Question") &&
                CurrentQuestionSet.getInstance().Type != (int)Types.Question)
            {
                hideMedia();
                
                DataGridCell currentCell = GetCurrentCell(questionSetDataGrid, questionSetDataGrid.CurrentCell);

                if ((currentCell.Content as TextBlock).Text == String.Empty)
                {
                    TextBlock filename = new TextBlock();
                    filename.Text = pickFile();
                    currentCell.Content = filename;
                    questionSetDataGrid.CommitEdit();
                    questionSetDataGrid.Items.Refresh();
                }
                else
                    displayMedia(currentCell.Content.ToString());
            }*/

            if (e.AddedCells.Count != 1)
                return;
            //MessageBox.Show((e.AddedCells[0].Column.GetCellContent(e.AddedCells[0].Item) as TextBlock).Text);
            if (e.AddedCells[0].Column.Header.Equals("Question") &&
                CurrentQuestionSet.getInstance().Type != (int)Types.Question)
            {
                hideMedia();

                TextBlock currentCell = e.AddedCells[0].Column.GetCellContent(e.AddedCells[0].Item) as TextBlock;
                if (currentCell.Text == String.Empty)
                {
                    currentCell.Text = pickFile(); //string doesn't register
                    //questionSetDataGrid.GetBindingExpression(DataGrid.ItemsSourceProperty).UpdateTarget();
                    //BindingOperations.GetBindingExpressionBase(questionSetDataGrid, DataGrid.ItemsSourceProperty).UpdateSource();
                }
                else
                    displayMedia(currentCell.Text.ToString());
            }
        }
        private bool isManualEditCommit;
        void HandleEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (!isManualEditCommit)
            {
                isManualEditCommit = true;
                DataGrid grid = (DataGrid)sender;
                grid.CommitEdit(DataGridEditingUnit.Row, true);
                isManualEditCommit = false;
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
            if (this.NavigationService.CanGoBack)
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
