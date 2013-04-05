using GameContext;

namespace Anime_Quiz_3.Classes
{
    class CurrentTeam
    {
        private static Teams currentTeam;
        public static Teams getInstance()
        {
            return currentTeam;
        }
        public static void setInstance(Teams team)
        {
            CurrentTeam.currentTeam = team;
        }
    }
}
