using System.Linq;
using System.Windows;
using GameContext;

namespace Anime_Quiz_3
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static GameDataContext db;
        public static IQueryable<QuestionSets> questionSets;
        public static IQueryable<Teams> teams;
        public App()
        {
            db = new GameDataContext();
            questionSets = db.GetTable<QuestionSets>();
            teams = db.GetTable<Teams>();
        }

        public static void refreshDb(object entity)
        {
            try
            {
                db.Refresh(Devart.Data.Linq.RefreshMode.KeepChanges, entity);
            }
            catch (System.InvalidOperationException crap)
            {
                // Ugly hack
                questionSets = db.GetTable<QuestionSets>();
                teams = db.GetTable<Teams>();
            }
        }
    }
}
