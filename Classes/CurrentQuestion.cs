using GameContext;

namespace Anime_Quiz_3.Classes
{
    class CurrentQuestion
    {
        private static Questions currentQuestion;
        public static Questions getInstance()
        {
            return currentQuestion;
        }
        public static void setInstance(Questions question)
        {
            CurrentQuestion.currentQuestion = question;
        }
    }
}
