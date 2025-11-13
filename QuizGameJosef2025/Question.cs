using System.Collections.Generic;

namespace QuizGameJosef2025
{
    public class Question
    {
        public string Text { get; set; }
        public List<string> Answers { get; set; }
        public int CorrectIndex { get; set; }

        public Question(string text, List<string> answers, int correctIndex)
        {
            Text = text;
            Answers = answers;
            CorrectIndex = correctIndex;
        }
    }
}
