namespace iDental.ViewModels
{
    public class ProgressDialogViewModel :ViewModelBase.PropertyChangedBase
    {
        private string dTitle;
        public string DTitle
        {
            get { return dTitle; }
            set
            {
                dTitle = value;
                OnPropertyChanged("DTitle");
            }
        }

        private string pText;
        public string PText
        {
            get { return pText; }
            set
            {
                pText = value;
                OnPropertyChanged("PText");
            }
        }
        private bool pIsIndeterminate = false;

        public bool PIsIndeterminate
        {
            get { return pIsIndeterminate; }
            set
            {
                pIsIndeterminate = value;
                OnPropertyChanged("PIsIndeterminate");
            }
        }

        private int pMinimum = 0;
        public int PMinimum
        {
            get { return pMinimum; }
            set
            {
                pMinimum = value;
                OnPropertyChanged("PMinimum");
            }
        }

        private int pMaximum;
        public int PMaximum
        {
            get { return pMaximum; }
            set
            {
                pMaximum = value;
                OnPropertyChanged("PMaximum");
            }
        }

        private int pValue;
        public int PValue
        {
            get { return pValue; }
            set
            {
                pValue = value;
                OnPropertyChanged("PValue");
            }
        }
    }
}
