using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Anime_Quiz.Classes;

namespace Anime_Quiz.Team
{
    public partial class TeamSelector : Form
    {
        TableLayoutPanel teamTable;
        int numberOfTeams;
        
        public TeamSelector()
        {
            InitializeComponent();
            addTeamTable();
        }
        bool saveTeams()
        {
            List<String> registeredTeams = new List<string>();
            try
            {
                for (int i = 0; i < numberOfTeams; i++)
                {
                    ComboBox combobox = teamTable.GetControlFromPosition(1, i) as ComboBox;
                    if (combobox.Text != String.Empty && !combobox.Text.Equals("Select an existing Team"))
                        registeredTeams.Add(combobox.Text);
                    else
                        throw new NullReferenceException("There was a problem with one or more team. Question your conscience");
                }
                CurrentTeams.getInstance().teams = registeredTeams.ToArray();
                CurrentTeams.getInstance().saveTeams();
                return true;
            }
            catch (Exception crap)
            {
                SoundMessageBox.Show(crap.Message, "Fail", MessageBoxButtons.OK, Anime_Quiz.Properties.Resources.Muda);
                return false;
            }
        }

        #region Controls
        public void addTeamTable()
        {
            teamTable = new TableLayoutPanel();
            teamTable.Location = new Point(12, 58);
            teamTable.AutoScroll = true;
            teamTable.AutoSize = true;
            teamTable.ColumnCount = 2;
            teamTable.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            Controls.Add(teamTable);
        }
        
        public void addTeamPicker(int teamNumber)
        {
            Label teamNumberLabel = new Label();
            teamNumberLabel.Text = "Team " + teamNumber;
            teamNumberLabel.Anchor = AnchorStyles.Top;
            teamNumberLabel.Padding = new Padding(5);
            teamNumberLabel.AutoSize = true;

            ComboBox teamSelector = CurrentTeams.getInstance().getAllTeamsSelector();
            teamSelector.Dock = DockStyle.Fill;
            
            teamTable.Controls.Add(teamNumberLabel, 0, teamNumber-1);
            teamTable.Controls.Add(teamSelector, 1, teamNumber-1);
        }
        #endregion

        #region Event Handlers
        private void teamNumberBtn_Click(object sender, EventArgs e)
        {
            numberOfTeams = (int) teamNumberBox.Value;
            CurrentTeams.setInstance(new Teams(numberOfTeams));

            for(int i = 1; i <= numberOfTeams; i++)
            {
                addTeamPicker(i);
            }
        }
        private void registerBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (CurrentTeams.getInstance() != null
                && !saveTeams()
                && SoundMessageBox.Show("There was an error saving to the database. Close anyways?", "Database error", MessageBoxButtons.YesNo, Anime_Quiz.Properties.Resources.Muda) == DialogResult.No
                && !e.Cancel)
                e.Cancel = true;
            base.OnFormClosing(e);
        }
        #endregion
    }
}
