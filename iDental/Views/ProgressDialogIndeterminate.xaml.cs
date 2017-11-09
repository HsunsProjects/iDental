using iDental.ViewModels;
using System.Windows;

namespace iDental.Views
{
    /// <summary>
    /// ProgressDialogIndeterminate.xaml 的互動邏輯
    /// </summary>
    public partial class ProgressDialogIndeterminate : Window
    {
        public string DTitle
        {
            get { return progressDialogIndeterminateViewModel.DTitle; }
            set { progressDialogIndeterminateViewModel.DTitle = value; }
        }
        public string PText
        {
            get { return progressDialogIndeterminateViewModel.PText; }
            set { progressDialogIndeterminateViewModel.PText = value; }
        }
        public bool PIsIndeterminate
        {
            get { return progressDialogIndeterminateViewModel.PIsIndeterminate; }
            set { progressDialogIndeterminateViewModel.PIsIndeterminate = value; }
        }
        public int PMinimum
        {
            get { return progressDialogIndeterminateViewModel.PMinimum; }
            set { progressDialogIndeterminateViewModel.PMinimum = value; }
        }

        public int PMaximum
        {
            get { return progressDialogIndeterminateViewModel.PMaximum; }
            set { progressDialogIndeterminateViewModel.PMaximum = value; }
        }

        public int PValue
        {
            get { return progressDialogIndeterminateViewModel.PValue; }
            set { progressDialogIndeterminateViewModel.PValue = value; }
        }

        public string ButtonContent
        {
            get { return progressDialogIndeterminateViewModel.ButtonContent; }
            set { progressDialogIndeterminateViewModel.ButtonContent = value; }
        }
        public Visibility ButtonContentVisibility
        {
            get { return progressDialogIndeterminateViewModel.ButtonContentVisibility; }
            set { progressDialogIndeterminateViewModel.ButtonContentVisibility = value; }
        }

        /// <summary>
        /// 委派回傳ProcessingDialog 
        /// </summary>
        /// <param name="isDetecting">true:停止/falae:跳過</param>
        public delegate void ReturnValueDelegate(bool isDetecting);

        public event ReturnValueDelegate ReturnValueCallback;

        private ProgressDialogIndeterminateViewModel progressDialogIndeterminateViewModel;
        public ProgressDialogIndeterminate()
        {
            InitializeComponent();

            progressDialogIndeterminateViewModel = new ProgressDialogIndeterminateViewModel();

            DataContext = progressDialogIndeterminateViewModel;

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ReturnValueCallback(false);
        }

        private void Button_Stop_Click(object sender, RoutedEventArgs e)
        {
            ReturnValueCallback(true);
        }
    }
}
