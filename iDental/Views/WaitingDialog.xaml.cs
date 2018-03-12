using iDental.ViewModels;
using System.Windows;

namespace iDental.Views
{
    /// <summary>
    /// WaitingDialog.xaml 的互動邏輯
    /// </summary>
    public partial class WaitingDialog : Window
    {
        public string WText
        {
            get { return waitingDialog.WText; }
            set { waitingDialog.WText = value; }
        }

        public string WDetail
        {
            get { return waitingDialog.WDetail; }
            set { waitingDialog.WDetail = value; }
        }

        private WaitingDialogViewModel waitingDialog;
        public WaitingDialog()
        {
            InitializeComponent();

            waitingDialog = new WaitingDialogViewModel();

            DataContext = waitingDialog;
        }
    }
}
