using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Anime_Quiz_3.Classes;
using Devart.Data.Linq;
using GameContext;

namespace Anime_Quiz_3.GameMaster
{
    /// <summary>
    /// Interaction logic for GameEditor.xaml
    /// </summary>
    public partial class GameEditor : Page
    {
        static GameDataContext db;
        static Table<Games> games;
        static Table<QuestionSets> questionSets;

        public GameEditor()
        {
            InitializeComponent();
            db = new GameDataContext();
            questionSets = db.GetTable<QuestionSets>();

            populateComboBox();
            populateStackPanel();
        }
        private void populateComboBox()
        {
            games = db.GetTable<Games>();
            IEnumerable<String> gameNames = from game in games select game.Name;
            gameComboBox.ItemsSource = gameNames;
        }
        private void populateStackPanel()
        {
            var questionSetList = from questionSet in questionSets
                                  select questionSet;
            foreach (var questionSet in questionSetList)
            {
                CheckBox questionSetCheckBox = new CheckBox();
                questionSetCheckBox.Name = questionSet.Name;
                questionSetCheckBox.Content = questionSet.Name;
                questionSetCheckBox.Checked += questionSetCheckBox_Checked;
                questionSetCheckBox.Unchecked += questionSetCheckBox_Unchecked;
                questionSetStackPanel.Children.Add(questionSetCheckBox);
            }
        }

        #region Event Handlers
        private void gameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            questionSetStackPanel.Visibility = System.Windows.Visibility.Visible;
            CurrentGame.setInstance((from game in games 
                                     where game.Name.Equals((sender as ComboBox).SelectedValue.ToString()) 
                                     select game).Single());
            foreach (CheckBox checkbox in questionSetStackPanel.Children.OfType<CheckBox>())
            {
                var questionSet = questionSets.Single(q => q.Name.Equals(checkbox.Name));
                checkbox.IsChecked = questionSet.Games != null
                    && questionSet.Games.Name.Equals(CurrentGame.getInstance().Name);
            }
            delBtn.IsEnabled = true;
        }

        void questionSetCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox senderCheckBox = sender as CheckBox;
            questionSetCheckBox_Toggle(senderCheckBox, true);
        }
        void questionSetCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox senderCheckBox = sender as CheckBox;
            questionSetCheckBox_Toggle(senderCheckBox, false);
        }
        void questionSetCheckBox_Toggle(CheckBox sender, bool check)
        {
            var questionSet = questionSets.Single(q => q.Name.Equals(sender.Content));
            if (check)
                questionSet.Games = db.Games.Single(g => g.Name.Equals(gameComboBox.SelectedValue.ToString()));
            else
            {
                try
                {
                    questionSet.GameId = (int?)null;
                }
                catch
                {
                }
            }
            db.SubmitChanges();
        }

        private void newGameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            addBtn.IsEnabled = (sender as TextBox).Text.Length > 0;
        }

        private void renameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            renameBtn.IsEnabled = (sender as TextBox).Text.Length > 0 && gameComboBox.SelectedIndex > -1;
        }
        #endregion

        #region Buttons
        private void delBtn_Click(object sender, RoutedEventArgs e)
        {
            var gameToDelete = games.Single(g => g.Name.Equals(gameComboBox.SelectedValue.ToString()));
            games.DeleteOnSubmit(gameToDelete);
            try { db.SubmitChanges(); }
            catch { }
            populateComboBox();

            delBtn.IsEnabled = false;
        }

        private void renameBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentGame.getInstance().Name = renameTextBox.Text;
            db.SubmitChanges();

            populateComboBox();
            gameComboBox.SelectedItem = renameTextBox.Text;
            renameTextBox.Text = String.Empty;
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            Games newGame = new Games();
            newGame.Name = newGameTextBox.Text;
            games.InsertOnSubmit(newGame);

            db.SubmitChanges();

            populateComboBox();
            gameComboBox.SelectedItem = newGame.Name;
            newGameTextBox.Text = String.Empty;
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            db.SubmitChanges();
            this.NavigationService.GoBack();
        }
        #endregion
    }
}
