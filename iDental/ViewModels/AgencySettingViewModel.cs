namespace iDental.ViewModels
{
    public class AgencySettingViewModel : ViewModelBase.PropertyChangedBase
    {
        private string agency_ImagePath;
        public string Agency_ImagePath
        {
            get { return agency_ImagePath; }
            set
            {
                agency_ImagePath = value;
                OnPropertyChanged("Agency_ImagePath");
            }
        }

        private string agency_WifiCardPath;

        public string Agency_WifiCardPath
        {
            get { return agency_WifiCardPath; }
            set
            {
                agency_WifiCardPath = value;
                OnPropertyChanged("Agency_WifiCardPath");
            }
        }

        private int agency_Function;
        public int Agency_Function
        {
            get { return agency_Function; }
            set
            {
                agency_Function = value;
                OnPropertyChanged("Agency_Function");
            }
        }

        private string agency_ViewType;
        public string Agency_ViewType
        {
            get { return agency_ViewType; }
            set
            {
                agency_ViewType = value;
                OnPropertyChanged("Agency_ViewType");
            }
        }

        public AgencySettingViewModel(Agencys agencys)
        {
            Agency_ImagePath = agencys.Agency_ImagePath;
            Agency_WifiCardPath = agencys.Agency_WifiCardPath;
            Agency_ViewType = agencys.Agency_ViewType;
            Agency_Function = agencys.Function_ID;
        }
    }
}
