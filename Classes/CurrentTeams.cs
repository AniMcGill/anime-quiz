using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anime_Quiz.Classes
{
    class CurrentTeams
    {
        private static Teams currentTeams;
        public static Teams getInstance()
        {
            return currentTeams;
        }
        public static void setInstance(Teams team)
        {
            CurrentTeams.currentTeams = team;
        }
    }
}
