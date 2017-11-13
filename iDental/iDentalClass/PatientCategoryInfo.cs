namespace iDental.iDentalClass
{
    public class PatientCategoryInfo : ViewModels.ViewModelBase.PropertyChangedBase
    {
        public int PatientCategory_ID { get; set; }
        public string PatientCategory_Title { get; set; }

        public int PatientCategory_SeqNo { get; set; }

        private bool isChecked = false;
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                if (value != isChecked)
                {
                    isChecked = value;
                    OnPropertyChanged("IsChecked");
                }
            }
        }
    }
}
