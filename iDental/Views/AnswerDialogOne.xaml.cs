using iDental.ViewModels;
using System.Windows;

namespace iDental.Views
{
    /// <summary>
    /// AnswerDialogOne.xaml 的互動邏輯
    /// </summary>
    public partial class AnswerDialogOne : Window
    {
        public string Answer { get; private set; }

        private AnswerDialogOneViewModel adovm;
        public AnswerDialogOne(string question, string mode)
        {
            InitializeComponent();
            
            //QUESTION
            Question.Text = question;

            adovm = new AnswerDialogOneViewModel(mode);

            DataContext = adovm;
        }

        private void Button_Sure_Click(object sender, RoutedEventArgs e)
        {
            Answer = adovm.Answer;
            DialogResult = true;
        }
    }
}
