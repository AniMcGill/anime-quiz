using GameContext;

namespace Anime_Quiz_3.Classes
{
    class CurrentGame
    {
        private static Games currentGame;
        public static Games getInstance()
        {
            return currentGame;
        }
        public static void setInstance(Games game)
        {
            CurrentGame.currentGame = game;
        }
    }
}
