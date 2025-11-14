using System.Collections.Generic;
using System.Windows;

namespace QuizGameJosef2025
{
    public partial class CreateQuizWindow : Window
    {
        private readonly List<Question> questions = new List<Question>();

        public CreateQuizWindow()
        {
            InitializeComponent();
            CorrectIndexComboBox.SelectedIndex = 0;
            UpdateQuestionsList();
        }

        private void AddQuestion_Click(object sender, RoutedEventArgs e)
        {
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

            var q = new Question(QuestionTextBox.Text, answers, correctIndex);
            questions.Add(q);

            QuestionTextBox.Text = "";
            Answer1TextBox.Text = "";
            Answer2TextBox.Text = "";
            Answer3TextBox.Text = "";
            CorrectIndexComboBox.SelectedIndex = 0;

            UpdateQuestionsList();
        }

        private void UpdateQuestionsList()
        {
            QuestionsListBox.Items.Clear();
            for (int i = 0; i < questions.Count; i++)
            {
                QuestionsListBox.Items.Add($"{i + 1}. {questions[i].Text}");
            }
        }

        private void SaveQuiz_Click(object sender, RoutedEventArgs e)
        {
            string name = string.IsNullOrWhiteSpace(QuizNameTextBox.Text)
                ? "NyttQuiz"
                : QuizNameTextBox.Text;

            if (questions.Count == 0)
            {
                MessageBox.Show("Lägg till minst en fråga.");
                return;
            }

            var quiz = new Quiz(name, questions);
            QuizFileHandler.SaveQuiz(quiz);
            MessageBox.Show("Quiz sparat.");
            Close();
        }
    }
}
