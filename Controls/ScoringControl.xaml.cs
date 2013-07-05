﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Anime_Quiz_3.Controls
{
    /// <summary>
    /// Interaction logic for ScoringControl.xaml
    /// </summary>
    public partial class ScoringControl : UserControl
    {
        private bool isTeam = false;
        // Declare delegates for the add/remove button clicked
        //public delegate void AddClickedHandler();
        //public delegate void RemoveClickedHandler();

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
                    Label teamLabel = new Label();
                    teamLabel.Content = "Team ";
                    scoringControlGrid.Children.Insert(0, teamLabel);
                    nameLabel.Margin = new Thickness(35, 0, 0, 0);
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
