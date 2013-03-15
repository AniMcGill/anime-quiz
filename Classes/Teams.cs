using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Anime_Quiz.Classes
{
    class Teams
    {
        //Data
        SQLiteDatabase sqlDB = new SQLiteDatabase();

        private String[] _teams;

        public Teams(int number)
        {
            _teams = new String[number];
        }
        public String[] teams
        {
            get { return this._teams; }
            set { this._teams = value; }
        }

        /// <summary>
        ///     Renames the teams
        /// </summary>
        /// <param name="newTeamNames">An array containing the new team names</param>
        /// <returns>true if the operation was successful; false otherwise</returns>
        public bool renameTeams(String[] newTeamNames)
        {
            try
            {
                for (int i = 0; i < newTeamNames.Length; i++)
                {
                    Dictionary<String, String> data = new Dictionary<string, string>();
                    data.Add("name", newTeamNames[i]);
                    String updateCmd = String.Format("name = '{0}'", _teams[i]);
                    if (sqlDB.Update("Teams", data, updateCmd))
                        _teams[i] = newTeamNames[i];
                    else
                        throw new ArgumentException("There was an error saving the team to the database. You should question your conscience.");
                }
                return true;
            }
            catch (Exception crap)
            {
                SoundMessageBox.Show(crap.Message, "Fail", System.Windows.Forms.MessageBoxButtons.OK, Anime_Quiz.Properties.Resources.Muda);
                return false;
            }
        }

        /// <summary>
        ///     Saves the teams to the database.
        /// </summary>
        /// <returns>true if all teams have been successfully saved; false otherwise</returns>
        public bool saveTeams()
        {
            bool success = true;
            foreach (String team in _teams)
            {
                Dictionary<String, String> data = new Dictionary<string, string>();
                data.Add("id", String.Format("(select id from Teams where name = '{0}')", team));
                data.Add("name", String.Format("'{0}'", team));
                success &= sqlDB.UnquotedInsertOrReplace("Teams", data);
            }
            return success;
        }

        private DataTable _allTeams;
        public DataTable getAllTeams()
        {
            String query = "select * from Teams order by id";
            _allTeams = sqlDB.getDataTable(query);
            return _allTeams;
        }
        private ComboBox _allTeamsSelector;
        public ComboBox getAllTeamsSelector()
        {
            getAllTeams();
            _allTeamsSelector = new ComboBox();
            _allTeamsSelector.Size = new Size(163, 21);
            _allTeamsSelector.Text = "Select an existing Team";
            foreach (DataRow team in _allTeams.Rows)
            {
                _allTeamsSelector.Items.Add(team["name"]);
            }
            return _allTeamsSelector;
        }
    }
}
