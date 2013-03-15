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

namespace Anime_Quiz
{
    public partial class TeamEditor : Form
    {
        // For the time being, we have to load the scoreset array first and rewrite it.
        private ScoreSet[] scoreSet;
        public TeamEditor()
        {
            InitializeComponent();
            if (Settings.Default.scoreSet != null)
            {
                scoreSet = Settings.Default.scoreSet;
                loadTeamNames();
            }
            else
                scoreSet = new ScoreSet[4];
        }

        private void loadTeamNames()
        {
            foreach (TextBox textBox in this.Controls.OfType<TextBox>())
            {
                int index = Int32.Parse(textBox.Name.Substring(textBox.Name.Length - 1, 1));
                textBox.Text = scoreSet[index - 1].getTeamName();
            }
        }

        // TODO: there is a bug where the settings aren't saved.
        private bool saveTeams()
        {
            Settings.Default.scoreSet = new ScoreSet[4];
            foreach (TextBox textBox in this.Controls.OfType<TextBox>())
            {
                if (textBox.Text == string.Empty)
                {
                    MessageBox.Show("Herp derp there is a team without a name.", "Epic fail", MessageBoxButtons.OK);
                    return false;
                }
                else
                {
                    int index = Int32.Parse(textBox.Name.Substring(textBox.Name.Length - 1, 1));
                    if (scoreSet[index - 1] != null)
                        scoreSet[index - 1].setTeamName(textBox.Text);
                    else 
                        scoreSet[index - 1] = new ScoreSet(textBox.Text);
                }
            }
            Settings.Default.scoreSet = scoreSet;
            return true;
        }

        private void clearTextBoxes()
        {
            foreach (TextBox textBox in this.Controls.OfType<TextBox>())
            {
                textBox.Text = string.Empty;
            }
            // Delete the team entries from memory
            Settings.Default.scoreSet = null;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            if (true)
                this.Close();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            saveTeams();
            this.Close();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            if (true)
                clearTextBoxes();
        }

    }
}
