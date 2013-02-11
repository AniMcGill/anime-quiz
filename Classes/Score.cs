using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anime_Quiz.Classes
{
    public class ScoreSet : ICollection
    {
        private string _teamName;
        private ArrayList setArray = new ArrayList();

        public ScoreSet(string teamName)
        {
            this._teamName = teamName;
        }

        public Score this[int index]
        {
            get { return (Score)setArray[index]; }
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
        public void Add(Score newScore)
        {
            setArray.Add(newScore);
        }
        public string getTeamName()
        {
            return this._teamName;
        }
        public void setTeamName(string newName)
        {
            this._teamName = newName;
        }
        public Score[] getScores()
        {
            return (Score[]) setArray.ToArray();
        }
        public int getTotalScore()
        {
            int totalScore = 0;
            foreach (Score score in setArray)
            {
                totalScore += score.getScore();
            }
            return totalScore;
        }
    }
    public class Score
    {
        private int _gameId;
        private int _score;
        private bool _isGameWon;

        public Score(int gameId)
        {
            this._gameId = gameId;
            this._score = 0;
            this._isGameWon = false;
        }

        
        public void addScore(int score)
        {
            this._score += score;
        }

        public int getScore()
        {
            return this._score;
        }

        public int getGameId()
        {
            return this._gameId;
        }

        public void setGameWon()
        {
            this._isGameWon = true;
        }

        public bool isGameWon()
        {
            return this._isGameWon;
        }
    }
}
