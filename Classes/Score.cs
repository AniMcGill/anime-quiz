using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Anime_Quiz.Classes
{
    public class Score
    {
        static SQLiteDatabase sqlDB = new SQLiteDatabase();

        public static int getScore(String teamName)
        {
            int teamId = CurrentTeams.getInstance().getTeamId(teamName);
            String gameId = CurrentGame.getInstance().name;
            String query = String.Format("select score from Scores where gameId = '{0}' and teamId = {1}", gameId, teamId);
            DataTable data = sqlDB.getDataTable(query);
            try
            {
                return Convert.ToInt32(data.Rows[0]["score"]);
            }
            catch (Exception crap)
            {
                return -1;
            }
        }
    }
}
