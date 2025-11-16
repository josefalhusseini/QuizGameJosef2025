using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace QuizGameJosef2025
{
    public partial class EditQuizWindow : Window
    {
        private Quiz currentQuiz;

        public EditQuizWindow()
        {
            InitializeComponent();
            CorrectIndexComboBox.SelectedIndex = 0;
        }

        private async void Load_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(QuizNameTextBox.Text))
            {
                MessageBox.Show("Skriv in ett quiz-namn.");
                return;
            }

            currentQuiz = await QuizFileHandler.LoadQuizAsync(QuizNameTextBox.Text);
            if (currentQuiz == null)
            {
                MessageBox.Show("Hittade inget quiz med det namnet.");
                return;
            }

            UpdateQuestionsList();
            StatusText.Text = $"Laddat quiz: {currentQuiz.Name}";
        }

        private void UpdateQuestionsList()
        {
            QuestionsListBox.Items.Clear();
            if (currentQuiz == null) return;

            for (int i = 0; i < currentQuiz.Questions.Count; i++)
            {
                QuestionsListBox.Items.Add($"{i + 1}. {currentQuiz.Questions[i].Text}");
            }
        }

        private void QuestionsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (currentQuiz == null) return;
            int index = QuestionsListBox.SelectedIndex;
            if (index < 0 || index >= currentQuiz.Questions.Count) return;

            var q = currentQuiz.Questions[index];
            QuestionTextBox.Text = q.Text;
            Answer1TextBox.Text = q.Answers[0];
            Answer2TextBox.Text = q.Answers[1];
            Answer3TextBox.Text = q.Answers[2];
            CorrectIndexComboBox.SelectedIndex = q.CorrectIndex;
        }

        private void SaveQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (currentQuiz == null) return;
            int index = QuestionsListBox.SelectedIndex;
            if (index < 0 || index >= currentQuiz.Questions.Count) return;

            if (string.IsNullOrWhiteSpace(QuestionTextBox.Text)) return;
            if (string.IsNullOrWhiteSpace(Answer1TextBox.Text)) return;
            if (string.IsNullOrWhiteSpace(Answer2TextBox.Text)) return;
            if (string.IsNullOrWhiteSpace(Answer3TextBox.Text)) return;

            var answers = new List<string>
            {
                Answer1TextBox.Text,
                Answer2TextBox.Text,
                Answer3TextBox.Text
            };

            int correctIndex = CorrectIndexComboBox.SelectedIndex;
            var q = currentQuiz.Questions[index];
            q.Text = QuestionTextBox.Text;
            q.Answers = answers;
            q.CorrectIndex = correctIndex;

            UpdateQuestionsList();
            QuestionsListBox.SelectedIndex = index;
            StatusText.Text = "Frågan uppdaterad.";
        }

        private async void SaveQuiz_Click(object sender, RoutedEventArgs e)
        {
            if (currentQuiz == null)
            {
                MessageBox.Show("Inget quiz laddat.");
                return;
            }

            if (!string.IsNullOrWhiteSpace(QuizNameTextBox.Text))
            {
                currentQuiz.Name = QuizNameTextBox.Text;
            }

            await QuizFileHandler.SaveQuizAsync(currentQuiz);
            MessageBox.Show("Quiz sparat.");
        }

    }
}
