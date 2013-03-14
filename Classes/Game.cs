using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Anime_Quiz.Classes
{
    class Game
    {
        //Data
        SQLiteDatabase sqlDB = new SQLiteDatabase();

        public Game(String name)
        {
            this._name = name;
        }

        private String _name;
        public String name
        {
            get { return this._name; }
            set { this._name = value; }
        }
        
        private ComboBox _gameSelector;
        public ComboBox getGameSelector()
        {
            String query = "select distinct name from Games";
            DataTable queryData = sqlDB.getDataTable(query);
            _gameSelector = new ComboBox();
            _gameSelector.Size = new Size(163, 21);
            _gameSelector.Text = "Select game to load";
            foreach (DataRow row in queryData.Rows)
            {
                _gameSelector.Items.Add(row["name"]);
            }
            return _gameSelector;
        }
        private DataTable _allQuestionSets;
        public DataTable getAllQuestionSets()
        {
            String query = "select name from QuestionSets";
            _allQuestionSets = sqlDB.getDataTable(query);
            return _allQuestionSets;
        }
        private DataTable _gameQuestionSets;
        public DataTable getGameQuestionSets()
        {
            String query = String.Format("select distinct questionSetId from Games where name='{0}'", this._name);
            _gameQuestionSets = sqlDB.getDataTable(query);
            return _gameQuestionSets;
        }

        /// <summary>
        ///     Adds the question sets as checkboxes to the layout
        /// </summary>
        /// <returns>A FlowLayoutPanel containing the checkboxes</returns>
        private FlowLayoutPanel _questionSetFlowPanel;
        public FlowLayoutPanel addQuestionSetsCheckboxes()
        {
            getAllQuestionSets();
            _questionSetFlowPanel = new FlowLayoutPanel();
            _questionSetFlowPanel.Size = new Size(400, 400);
            _questionSetFlowPanel.AutoSize = true;
            _questionSetFlowPanel.FlowDirection = FlowDirection.TopDown;
            _questionSetFlowPanel.AutoScroll = true;

            foreach (DataRow row in _allQuestionSets.Rows)
            {
                CheckBox checkbox = new CheckBox();
                checkbox.Name = row["name"].ToString();
                checkbox.Text = row["name"].ToString();
                _questionSetFlowPanel.Controls.Add(checkbox);
            }
            return _questionSetFlowPanel;
        }

        /// <summary>
        ///     Set question sets contained in the current game as checked.
        /// </summary>
        public void setCheckboxes()
        {
            getGameQuestionSets();
            foreach (DataRow row in _gameQuestionSets.Rows)
            {
                CheckBox checkbox = _questionSetFlowPanel.Controls[row["questionSetId"].ToString()] as CheckBox;
                checkbox.Checked = true;
            }
        }

        /// <summary>
        ///     Clears the question sets checkboxes
        /// </summary>
        public void clearQuestionSetsCheckboxes()
        {
            try
            {
                foreach (CheckBox checkbox in _questionSetFlowPanel.Controls.OfType<CheckBox>())
                {
                    checkbox.Checked = false;
                }
            }
            catch (NullReferenceException)
            {
                return;
            }
        }

        public bool saveGame()
        {
            try
            {
                foreach (CheckBox checkbox in _questionSetFlowPanel.Controls.OfType<CheckBox>())
                {
                    Dictionary<String, String> data = new Dictionary<string, string>();
                    if (checkbox.Checked)
                    {
                        data.Add("name", _name);
                        data.Add("questionSetId", checkbox.Text);
                        sqlDB.InsertOrReplace("Games", data);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
