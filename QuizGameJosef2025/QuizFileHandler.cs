using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace QuizGameJosef2025
{
    public static class QuizFileHandler
    {
        private static string GetFolder()
        {
            string folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "QuizGameJosef2025");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            return folder;
        }

        private static string GetPath(string quizName)
        {
            return Path.Combine(GetFolder(), quizName + ".json");
        }

        public static void SaveQuiz(Quiz quiz)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(quiz, options);
            File.WriteAllText(GetPath(quiz.Name), json);
        }

        public static Quiz LoadQuiz(string quizName)
        {
            string path = GetPath(quizName);
            if (!File.Exists(path)) return null;

            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Quiz>(json);
        }
    }
}
