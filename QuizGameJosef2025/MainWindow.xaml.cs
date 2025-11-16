using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace QuizGameJosef2025
{
    public partial class MainWindow : Window
    {
        private Quiz currentQuiz;
        private int index;
        private int correct;
        private readonly System.Random rng = new System.Random();

        public MainWindow()
        {
            InitializeComponent();

            currentQuiz = new Quiz(
                "Standardquiz",
                new List<Question>
                {
                    new Question(
                        "Vem kallas ofta 'kungen av pop'?",
                        new List<string> { "Michael Jackson", "Elvis Presley", "Prince" },
                        0),
                    new Question(
                        "Har en svensk någonsin varit i rymden?",
                        new List<string> { "Ja", "Nej", "Bara i filmer" },
                        0),
                    new Question(
                        "Vad är ungefär roten ur pi?",
                        new List<string> { "1,77", "3,14", "1,41" },
                        0),
                    new Question(
                        "Vad heter vår galax?",
                        new List<string> { "Vintergatan", "Andromedagalaxen", "Orionbältet" },
                        0),
                    new Question(
                        "Vilken planet kallas den röda planeten?",
                        new List<string> { "Mars", "Jupiter", "Venus" },
                        0),
                    new Question(
                        "Vilken stad är Sveriges huvudstad?",
                        new List<string> { "Stockholm", "Göteborg", "Malmö" },
                        0),
                    new Question(
                        "Vad heter Sveriges högsta fjäll?",
                        new List<string> { "Kebnekaise", "Mount Everest", "Åreskutan" },
                        0),
                    new Question(
                        "Vilken färg har det gula i svenska flaggan?",
                        new List<string> { "Gul", "Vit", "Orange" },
                        0),
                    new Question(
                        "I vilken stad utspelar sig tv-serien 'Friends'?",
                        new List<string> { "New York", "Los Angeles", "London" },
                        0),
                    new Question(
                        "Vad heter den lilla gröna jedimästaren i Star Wars?",
                        new List<string> { "Yoda", "Grogu", "Luke" },
                        0),
                    new Question(
                        "Vad heter trollkarlsskolan i Harry Potter?",
                        new List<string> { "Hogwarts", "Durmstrang", "Beauxbatons" },
                        0),
                    new Question(
                        "Vilken streamingtjänst är känd för serien 'Stranger Things'?",
                        new List<string> { "Netflix", "Disney+", "HBO Max" },
                        0)
                }
            );

            QuizNameTextBox.Text = currentQuiz.Name;

            currentQuiz.Questions = currentQuiz.Questions
                .OrderBy(q => rng.Next())
                .ToList();

            index = 0;
            correct = 0;

            ShowQuestion();
        }

        private void ShowQuestion()
        {
            var q = currentQuiz.Questions[index];

            QuestionText.Text = q.Text;
            A1.Content = q.Answers[0];
            A2.Content = q.Answers[1];
            A3.Content = q.Answers[2];

            A1.IsChecked = false;
            A2.IsChecked = false;
            A3.IsChecked = false;

            ScoreText.Text = $"Rätt: {correct} / {index} ({GetPercent():0}%)";
        }

        private double GetPercent()
        {
            if (index == 0) return 0;
            return 100.0 * correct / index;
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            int selected = -1;

            if (A1.IsChecked == true) selected = 0;
            else if (A2.IsChecked == true) selected = 1;
            else if (A3.IsChecked == true) selected = 2;

            if (selected == -1)
            {
                MessageBox.Show("Välj ett svar.");
                return;
            }

            var q = currentQuiz.Questions[index];

            if (selected == q.CorrectIndex)
                correct++;

            index++;

            if (index < currentQuiz.Questions.Count)
            {
                ShowQuestion();
            }
            else
            {
                double percent = 100.0 * correct / currentQuiz.Questions.Count;
                MessageBox.Show($"Klart! Du fick {correct} av {currentQuiz.Questions.Count} ({percent:0}%).");
                NextButton.IsEnabled = false;
            }
        }

        private async void SaveQuiz_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(QuizNameTextBox.Text))
                currentQuiz.Name = QuizNameTextBox.Text;

            await QuizFileHandler.SaveQuizAsync(currentQuiz);
            MessageBox.Show("Quiz sparat som: " + currentQuiz.Name);
        }

        private async void LoadQuiz_Click(object sender, RoutedEventArgs e)
        {
            string name = QuizNameTextBox.Text;

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show(
                    "För att ladda ett quiz måste du skriva in quiz-namnet i rutan högst upp.\n\n" +
                    "Exempel: Om quizet sparades som 'MittQuiz', skriv in exakt det namnet."
                );
                return;
            }

            var loaded = await QuizFileHandler.LoadQuizAsync(name);

            if (loaded == null)
            {
                MessageBox.Show(
                    "Kunde inte hitta quiz med namnet: " + name +
                    "\n\nKontrollera stavningen och försök igen."
                );
                return;
            }

            currentQuiz = loaded;
            index = 0;
            correct = 0;
            NextButton.IsEnabled = true;
            ShowQuestion();
        }

        private void CreateQuiz_Click(object sender, RoutedEventArgs e)
        {
            var window = new CreateQuizWindow();
            window.Show();
        }

        private void EditQuiz_Click(object sender, RoutedEventArgs e)
        {
            var window = new EditQuizWindow();
            window.Show();
        }
    }
}
