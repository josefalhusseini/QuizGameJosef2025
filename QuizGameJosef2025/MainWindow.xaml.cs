using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                        "Vad är 2 + 2?",
                        new List<string> { "3", "4", "5" },
                        1),
                    new Question(
                        "Vilken färg har himlen oftast?",
                        new List<string> { "Blå", "Grön", "Röd" },
                        0)
                }
            );

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
            await QuizFileHandler.SaveQuizAsync(currentQuiz);
            MessageBox.Show("Quiz sparat.");
        }

        private async void LoadQuiz_Click(object sender, RoutedEventArgs e)
        {
            var loaded = await QuizFileHandler.LoadQuizAsync(currentQuiz.Name);
            if (loaded == null)
            {
                MessageBox.Show("Ingen sparad quiz hittades.");
                return;
            }

            currentQuiz = loaded;
            index = 0;
            correct = 0;
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

