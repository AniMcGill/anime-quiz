using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Anime_Quiz.Classes;
using Anime_Quiz.Properties;
using Anime_Quiz.Team;

namespace Anime_Quiz
{
    public partial class TeamEditor : Form
    {
        TableLayoutPanel teamTable;

        public TeamEditor()
        {
            InitializeComponent();
            if (CurrentTeams.getInstance() != null)
                loadTeamNames();
            else
            {
                TeamSelector teamSelector = new TeamSelector();
                teamSelector.FormClosed += teamSelector_FormClosed;
                teamSelector.TopMost = true;
                teamSelector.Show();
            }

        }

        #region Controls
        void addTeamTable()
        {
            teamTable = new TableLayoutPanel();
            teamTable.Location = new Point(12, 12);
            teamTable.AutoScroll = true;
            teamTable.AutoSize = true;
            teamTable.ColumnCount = 2;
            teamTable.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            Controls.Add(teamTable);
        }
        #endregion

        private void loadTeamNames()
        {
            Controls.Remove(teamTable);
            addTeamTable();
            String[] teams = CurrentTeams.getInstance().teams;
            for (int i = 0; i < teams.Length; i++)
            {
                Label teamNameLabel = new Label();
                teamNameLabel.Text = "Team " + (i + 1);
                teamNameLabel.Font = new Font("Microsoft Sans Serif", 16); 
                TextBox teamNameTextBox = new TextBox();
                teamNameTextBox.Font = new Font("Microsoft Sans Serif", 14);
                teamNameTextBox.Text = teams[i];
                teamNameTextBox.Name = teams[i];    // trick to store old name here.

                teamTable.Controls.Add(teamNameLabel, 0, i);
                teamTable.Controls.Add(teamNameTextBox, 1, i);
            }
        }

        private bool saveTeams()
        {
            String[] newTeamNames = new String[CurrentTeams.getInstance().teams.Length];
            for (int i = 0; i < newTeamNames.Length; i++)
            {
                TextBox textbox = teamTable.GetControlFromPosition(1,i) as TextBox;
                if (textbox.Text != String.Empty)
                    newTeamNames[i] = textbox.Text;
                else
                    newTeamNames[i] = textbox.Name;
            }
            return CurrentTeams.getInstance().renameTeams(newTeamNames);
        }
        #region Event Handlers
        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if(saveTeams()
                || SoundMessageBox.Show("There was an error renaming the teams. Close anyways?", 
                "Fail", MessageBoxButtons.YesNo, Anime_Quiz.Properties.Resources.Muda) == DialogResult.Yes)
                this.Close();
        }
        void teamSelector_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (CurrentTeams.getInstance() != null)
                loadTeamNames();
        }
        #endregion

    }
}
