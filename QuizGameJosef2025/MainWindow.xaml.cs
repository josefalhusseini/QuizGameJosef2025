using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuizGameJosef2025
{
    public partial class MainWindow : Window
    {
        private List<Question> questions;
        private int index;
        private int correct;
        private readonly Random rng; // Changed from object to Random

        public MainWindow()
        {
            InitializeComponent();

            rng = new Random(); // Initialize rng

            questions = new List<Question>
    {
        new Question(
            "Vad är 2 + 2?",
            new List<string> { "3", "4", "5" },
            1),
        new Question(
            "Vilken färg har himlen oftast?",
            new List<string> { "Blå", "Grön", "Röd" },
            0)
    };

            questions = questions.OrderBy(q => rng.Next()).ToList();

            index = 0;
            correct = 0;

            ShowQuestion();
        }

        private double GetPercent()
        {
            if (index == 0) return 0;
            return 100.0 * correct / index;
        }


        private void ShowQuestion()
        {
            var q = questions[index];

            QuestionText.Text = q.Text;
            A1.Content = q.Answers[0];
            A2.Content = q.Answers[1];
            A3.Content = q.Answers[2];

            A1.IsChecked = false;
            A2.IsChecked = false;
            A3.IsChecked = false;

            ScoreText.Text = $"Rätt: {correct} / {index} ({GetPercent():0}%)";

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

            var q = questions[index];

            if (selected == q.CorrectIndex)
                correct++;

            index++;

            if (index < questions.Count)
            {
                ShowQuestion();
            }
            else
            {
                double percent = 100.0 * correct / questions.Count;
                MessageBox.Show($"Grattis! Du fick {correct} av {questions.Count} ({percent:0}%).");
                NextButton.IsEnabled = false;
            }


        }
    }
}
