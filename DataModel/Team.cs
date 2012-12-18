using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/**
 * This might not be needed as we can simply use Score
 * 
 **/
namespace Anime_Quiz.DataModel
{
    class Team
    {
        private string _teamName;
        private ScoreSet _scoreList;
        private Score currentGameScore;

        public Team(string teamName)
        {
            this._teamName = teamName;
            //_scoreList = new ScoreSet();
        }

        public void startNewGame(int gameId)
        {
            currentGameScore = new Score(gameId);
        }
        
    }
}
