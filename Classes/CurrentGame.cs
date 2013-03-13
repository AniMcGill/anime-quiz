using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anime_Quiz.Classes
{
    class CurrentGame
    {
        private static String gameName;
        
        public static String getInstance()
        {
            return gameName;
        }
        public static void setInstance(String name)
        {
            CurrentGame.gameName = name;
        }
    }
}
