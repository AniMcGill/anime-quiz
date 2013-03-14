using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anime_Quiz.Classes
{
    class EmptyGame
    {
        private static Game emptyGame = new Game(String.Empty);
        public static Game getInstance()
        {
            return emptyGame;
        }
    }
}
