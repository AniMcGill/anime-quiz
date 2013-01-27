using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Drawing;

namespace Anime_Quiz.DataModel
{
    public enum Types { Question, Music, Screenshot };

    public class QuestionSet:ICollection
    {
        private string _name;
        private Types _type;

        private ArrayList setArray = new ArrayList();
        public Question this[int index]
        {
            get { return (Question)setArray[index]; }
        }
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
    }

    public class Question
    {
        private int _questionID;
        private byte[] _question;
        private string _answer;
        //private Types _type;
        private int _points;
        private bool _answered;

        public int questionID
        {
            get { return _questionID; }
            set { _questionID = value; }
        }
        public byte[] question
        {
            get { return _question; }
            set { _question = value; }
        }
        public string answer
        {
            get { return _answer; }
            set { _answer = value; }
        }
        /*
        public Types type
        {
            get { return _type; }
            set { _type = value; }
        }*/
        public int points
        {
            get { return _points; }
            set { _points = value; }
        }
        public bool answered
        {
            get { return _answered; }
            set { _answered = value; }
        }

        public Question()
        {
        }
        public Question(byte[] q, string a, int p, bool state)
        {
            this._question = q;
            this._answer = a;
            this._points = p;
            this._answered = state;
        }
        /*
        public Question(string q, string a, string  type, int p, bool state)
        {
            this._question = q;
            this._answer = a;
            this._type = type;
            this._points = p;
            this._answered = state;
        }
        */
    }
}
