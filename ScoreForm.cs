using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Anime_Quiz.DataModel;
using Anime_Quiz.Properties;

namespace Anime_Quiz
{
    public partial class ScoreForm : Form
    {
        ScoreSet[] scoreSet;
        public ScoreForm()
        {
            InitializeComponent();
            scoreSet = Settings.Default.scoreSet;
            //test only
            loadTempData();
            try
            {
                loadScores();
            }
            catch (Exception)
            {
            }
        }
        private void loadScores()
        {
            Label gameTableLabel = new Label();
            gameTableLabel.Text = "Game";
            scoreTable.Controls.Add(gameTableLabel, 0, 0);
            for (int i = 0; i < scoreSet.Length; i++)
            {
                Label teamNameLabel = new Label();
                teamNameLabel.Text = scoreSet[i].getTeamName();
                scoreTable.Controls.Add(teamNameLabel, i + 1, 0);
                Score[] scores = scoreSet[i].getScores();
                for (int j = 0; j < scores.Length; j++)
                {
                    Label gameIdLabel = new Label();
                    gameIdLabel.Text = "Game " + scores[j].getGameId();
                    if(!scoreTable.Controls.Contains(gameIdLabel))
                        scoreTable.Controls.Add(gameIdLabel, 0, j+1);   //Try to add the gameID label if not there
                    Label gameScoreLabel = new Label();
                    gameScoreLabel.Text = scores[j].getScore().ToString();
                    if (scores[j].isGameWon())
                        gameScoreLabel.ForeColor = Color.Red;
                    scoreTable.Controls.Add(gameScoreLabel, i + 1, j + 1);
                }
            }
        }
        private void loadTempData()
        {
            scoreSet = new ScoreSet[4];
            ScoreSet team1 = new ScoreSet("test");
            Score score1 = new Score(1);
            score1.addScore(2000);
            score1.setGameWon();

            Score score2 = new Score(2);
            score2.addScore(3040);
            //score1.setGameWon();

            Score score3 = new Score(3);
            score3.addScore(1000);

            Score score4 = new Score(4);
            score4.addScore(6000);
            team1.Add(score1);
            team1.Add(score2);
            team1.Add(score3);
            team1.Add(score4);

            ScoreSet team2 = new ScoreSet("good");
            team2.Add(score1);
            team2.Add(score2);
            team2.Add(score3);
            team2.Add(score4);
            ScoreSet team3 = new ScoreSet("bad");
            team3.Add(score1);
            team3.Add(score2);
            team3.Add(score3);
            team3.Add(score4);
            ScoreSet team4 = new ScoreSet("test2");
            team4.Add(score1);
            team4.Add(score2);
            team4.Add(score3);
            team4.Add(score4);
        }
    }
}
