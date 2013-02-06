using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Drawing;
using System.Data;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Anime_Quiz.DataModel
{
    public enum Types { Question, Music, Screenshot };

    public class QuestionSet:ICollection
    {
        private string _name;
        private Types _type;

        private ArrayList setArray = new ArrayList();

        public QuestionSet(String name, Types type)
        {
            this._name = name;
            this._type = type;
        }

        public Question this[int index]
        {
            get { return (Question)setArray[index]; }
        }
        #region Collection
        public void CopyTo(Array a, int index)
        {
            setArray.CopyTo(a, index);
        }
        public int Count
        {
            get { return setArray.Count; }
        }
        public object SyncRoot
        {
            get { return this; }
        }
        public bool IsSynchronized
        {
            get { return false; }
        }
        public IEnumerator GetEnumerator()
        {
            return setArray.GetEnumerator();
        }
        #endregion
        public void Add(Question newQuestion)
        {
            setArray.Add(newQuestion);
        }

        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        public Types type
        {
            get { return _type; }
            set { _type = value; }
        }
        /// <summary>
        ///     Gets the Questions DataTable for the current QuestionSet
        /// </summary>
        /// <returns>A DataTable</returns>
        public DataTable getQuestionDataTable()
        {
            String query = String.Format("select * from Questions where questionSet = '{0}'", _name);
            SQLiteDatabase sqlDB = new SQLiteDatabase();
            return sqlDB.getDataTable(query);
        }

        /// <summary>
        ///     Gets the Questions for the current QuestionSet as an ArrayList
        /// </summary>
        /// <returns>An ArrayList of Questions</returns>
        public ArrayList getQuestions()
        {
            DataTable data = getQuestionDataTable();
            if (data == null)
            {
                return null;
            }
            foreach (DataRow row in data.Rows)
            {
                Question question = new Question(Convert.ToInt32(row["id"]), row["question"].ToString(), row["answer"].ToString(), Convert.ToInt32(row["points"]), Convert.ToBoolean(row["answered"]), name);
                setArray.Add(question);
            }
            return setArray;
        }
        public bool saveQuestions()
        {
            foreach (Question question in setArray)
            {
                SQLiteDatabase sqlDb = new SQLiteDatabase();
                Dictionary<String, String> data = new Dictionary<string, string>();
                data.Add("id", question.questionID.ToString());
                data.Add("question", question.question);
                data.Add("answer", question.answer);
                data.Add("points", question.points.ToString());
                data.Add("answered", question.answered.ToString());
                data.Add("questionSet", this._name);
                if (!sqlDb.InsertOrReplace("Questions", data))
                    return false;
            }
            return true;
        }
    }

    public class Question:INotifyPropertyChanged
    {
        private int _questionID;
        private string _question;
        private string _answer;
        private int _points;
        private bool _answered;
        private string _questionSet;
        [DisplayName("Question ID")]
        public int questionID
        {
            get { return _questionID; }
            set { _questionID = value; }
        }
        [DisplayName("Question")]
        public string question
        {
            get { return _question; }
            set { _question = value; }
        }
        [DisplayName("Answer")]
        public string answer
        {
            get { return _answer; }
            set { _answer = value; }
        }       
        [DisplayName("Points")]
        public int points
        {
            get { return _points; }
            set { _points = value; }
        }
        [DisplayName("Answered")]
        public bool answered
        {
            get { return _answered; }
            set { _answered = value; }
        }
        [DisplayName("QuestionSet")]
        public string questionSet
        {
            get { return _questionSet; }
            set { _questionSet = value; }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public Question()
        {
            this._question = null;
            this.answer = String.Empty;
            this._points = 0;
            this._answered = false;
        }
        public Question(int id, string q, string a, int p, bool state, string questionSet)
        {
            this.childElementsValue.CollectionChanged += OnCollectionChanged;
            this._questionID = id;
            this._question = q;
            this._answer = a;
            this._points = p;
            this._answered = state;
            this._questionSet = questionSet;
        }

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Question item in e.NewItems)
                    {
                        ((INotifyPropertyChanged)item).PropertyChanged += new PropertyChangedEventHandler(OnPropertyChanged);
                    }
                    break;
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        private ObservableCollection<Question> childElementsValue = new ObservableCollection<Question>();
        public ObservableCollection<Question> ChildElements
        {
            get { return childElementsValue; }
            set { childElementsValue = value; }
        }
    }
}
