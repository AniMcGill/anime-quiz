using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Anime_Quiz.Classes;
using Anime_Quiz.Properties;
//using System.Globalization;
//using System.Threading;

namespace Anime_Quiz
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Settings.Default.Upgrade();
            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
            if (Settings.Default.currentSet != null)
                CurrentQuestionSet.setInstance(Settings.Default.currentSet);
            Application.Run(new GameBoard());
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
        }

        static void Application_ApplicationExit(object sender, EventArgs e)
        {
            Settings.Default.currentSet = CurrentQuestionSet.getInstance();
            Settings.Default.Save();
        }
    }
}
