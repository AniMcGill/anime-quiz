using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anime_Quiz
{
    class GlobalVars
    {
        private static string currentFile;
        public static string CurrentFile
        {
            get { return currentFile; }
            set { currentFile = value; }
        }
    }
}
