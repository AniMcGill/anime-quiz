using GameContext;

namespace Anime_Quiz_3.Classes
{
    class CurrentQuestionSet
    {
        private static QuestionSets currentQuestionSet;
        public static QuestionSets getInstance()
        {
            return currentQuestionSet;
        }
        public static void setInstance(QuestionSets questionSet)
        {
            CurrentQuestionSet.currentQuestionSet = questionSet;
        }
    }
}
