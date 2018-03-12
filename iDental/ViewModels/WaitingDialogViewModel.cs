namespace iDental.ViewModels
{
    public class WaitingDialogViewModel : ViewModelBase.PropertyChangedBase
    {
        private string wText;
        public string WText
        {
            get { return wText; }
            set
            {
                wText = value;
                OnPropertyChanged("WText");
            }
        }

        private string wDetail;
        public string WDetail
        {
            get { return wDetail; }
            set
            {
                wDetail = value;
                OnPropertyChanged("WDetail");
            }
        }
    }
}
