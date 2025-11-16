using System.Collections.Generic;

namespace QuizGameJosef2025
{
    public class Quiz
    {
        public string Name { get; set; }
        public List<Question> Questions { get; set; }

        public Quiz()
        {
            Questions = new List<Question>();
        }

        public Quiz(string name, List<Question> questions)
        {
            Name = name;
            Questions = questions;
        }
    }
}
