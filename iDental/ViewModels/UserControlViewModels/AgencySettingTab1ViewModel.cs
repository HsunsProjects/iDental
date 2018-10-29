using iDental.Class;
using iDental.DatabaseAccess.QueryEntities;

namespace iDental.ViewModels.UserControlViewModels
{
    public class AgencySettingTab1ViewModel : ViewModelBase.PropertyChangedBase
    {
        public Agencys Agencys;

        private string agency_ImagePath;
        public string Agency_ImagePath
        {
            get { return agency_ImagePath; }
            set
            {
                agency_ImagePath = value;
                Agencys.Agency_ImagePath = agency_ImagePath;
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
                Agencys.Agency_WifiCardPath = agency_WifiCardPath;
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
                Agencys.Function_ID = agency_Function;
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
                Agencys.Agency_ViewType = agency_ViewType;
                OnPropertyChanged("Agency_ViewType");
            }
        }

        private string twainDeviceName;

        public string TwainDeviceName
        {
            get
            {
                return string.IsNullOrEmpty(ConfigManage.ReadAppConfig("TwainDevice")) ? "尚未設定" : ConfigManage.ReadAppConfig("TwainDevice");
            }
            set
            {
                twainDeviceName = value;
                ConfigManage.AddUpdateAppConfig("TwainDevice", twainDeviceName);
                OnPropertyChanged("TwainDeviceName");
            }
        }


        public AgencySettingTab1ViewModel()
        {
            Agencys = new TableAgencys().QueryVerifyAgencys();
            Agency_ImagePath = Agencys.Agency_ImagePath;
            Agency_WifiCardPath = Agencys.Agency_WifiCardPath;
            Agency_ViewType = Agencys.Agency_ViewType;
            Agency_Function = Agencys.Function_ID;

            //如果沒有TwainDevice,建立config,key = TwainDevice
            ConfigManage.CreateConfig("TwainDevice");
        }
    }
}
