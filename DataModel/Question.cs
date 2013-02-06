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
                Byte[] questionData = GetBytes(row["question"].ToString()); //todo: use the right conversion depending on type
                Question question = new Question(Convert.ToInt32(row["id"]), questionData, row["answer"].ToString(), Convert.ToInt32(row["points"]), Convert.ToBoolean(row["answered"]), name);
                //Question question = new Question(Convert.ToInt32(row["id"]), row["answer"].ToString(), Convert.ToInt32(row["points"]), Convert.ToBoolean(row["answered"]));
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
                data.Add("question", GetString(question.question));
                data.Add("answer", question.answer);
                data.Add("points", question.points.ToString());
                data.Add("answered", question.answered.ToString());
                data.Add("questionSet", this._name);
                if (!sqlDb.InsertOrReplace("Questions", data))
                    return false;
            }
            return true;
        }
        #region Temp
        /// <summary>
        ///     Convert a string to a byte array http://stackoverflow.com/questions/472906/net-string-to-byte-array-c-sharp
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
        /// <summary>
        ///     Convert a byte array to a string http://stackoverflow.com/questions/472906/net-string-to-byte-array-c-sharp
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
        #endregion
    }

    public class Question
    {
        private int _questionID;
        private byte[] _question;
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
        public byte[] question
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

        public Question()
        {
            this._question = null;
            this.answer = String.Empty;
            this._points = 0;
            this._answered = false;
        }
        public Question(int id, byte[] q, string a, int p, bool state, string questionSet)
        {
            this._questionID = id;
            this._question = q;
            this._answer = a;
            this._points = p;
            this._answered = state;
            this._questionSet = questionSet;
        }
    }
}
