using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

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

        public static async Task SaveQuizAsync(Quiz quiz)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(quiz, options);
            string path = GetPath(quiz.Name);
            await File.WriteAllTextAsync(path, json);
        }

        public static async Task<Quiz> LoadQuizAsync(string quizName)
        {
            string path = GetPath(quizName);
            if (!File.Exists(path)) return null;

            string json = await File.ReadAllTextAsync(path);
            return JsonSerializer.Deserialize<Quiz>(json);
        }
    }
}
