using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anime_Quiz.Classes
{
    public class CurrentQuestionSet
    {
        private static QuestionSet questionSet;

        public static QuestionSet getInstance()
        {
            return questionSet;
        }
        public static void setInstance(QuestionSet q)
        {
            CurrentQuestionSet.questionSet = q;
        }
    }
}
