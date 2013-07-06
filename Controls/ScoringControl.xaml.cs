using System;
using System.Windows;
using System.Windows.Controls;

namespace Anime_Quiz_3.Controls
{
    /// <summary>
    /// Interaction logic for ScoringControl.xaml
    /// </summary>
    public partial class ScoringControl : UserControl
    {
        private bool isTeam = false;

        public ScoringControl()
        {
            InitializeComponent();
            addPointBtn.Click += addPointBtn_Click;
            removePointBtn.Click += removePointBtn_Click;
        }

        public object Text
        {
            get { return nameLabel.Content; }
            set { nameLabel.Content = value; } 
        }
        public bool IsTeam
        {
            get { return isTeam; }
            set 
            { 
                isTeam = value;
                if (isTeam)
                {
                    scoringControlGrid.Children.Remove(addPointBtn);
                    nameLabel.FontWeight = FontWeights.Bold;
                }
            }
        }

        // Events
        void addPointBtn_Click(object sender, RoutedEventArgs e)
        {
            OnAddButtonClicked(EventArgs.Empty);
        }
        void removePointBtn_Click(object sender, RoutedEventArgs e)
        {
            OnRemoveButtonClicked(EventArgs.Empty);
        }

        public event EventHandler AddButtonClicked;
        public event EventHandler RemoveButtonClicked;

        protected virtual void OnAddButtonClicked(EventArgs e)
        {
            EventHandler handler = AddButtonClicked;
            if (handler != null)
                handler(this, e);
        }
        protected virtual void OnRemoveButtonClicked(EventArgs e)
        {
            EventHandler handler = RemoveButtonClicked;
            if (handler != null)
                handler(this, e);
        }
    }
}
