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

namespace Anime_Quiz
{
    public partial class QuestionSetSelector : Form
    {
        //Data
        SQLiteDatabase sqlDB = new SQLiteDatabase();
        DataSet questionDataSet;

        public QuestionSetSelector()
        {
            InitializeComponent();
            loadQuestionSetList();
        }
        private void loadQuestionSetList()
        {
            String query = "select * from QuestionSets";
            DataTable queryData = sqlDB.getDataTable(query);
            foreach (DataRow row in queryData.Rows)
            {
                questionSetList.Items.Add(row["name"]);
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void loadBtn_Click(object sender, EventArgs e)
        {
            try
            {
                String questionSetName = questionSetList.SelectedItem.ToString();
                Types questionSetType = sqlDB.getQuestionSetType(questionSetName);
                CurrentQuestionSet.setInstance(new QuestionSet(questionSetName, questionSetType));
                this.Close();
            }
            catch (Exception crap)
            {
                MessageBox.Show("You have not selected a Question Set.", "Fail");
            }
        }
    }
}
