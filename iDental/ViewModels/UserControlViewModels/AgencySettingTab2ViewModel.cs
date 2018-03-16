using iDental.Class;

namespace iDental.ViewModels.UserControlViewModels
{
    public class AgencySettingTab2ViewModel : ViewModelBase.PropertyChangedBase
    {
        private string pointofixPath;

        public string PointofixPath
        {
            get { return pointofixPath; }
            set
            {
                pointofixPath = value;
                OnPropertyChanged("PointofixPath");
            }
        }

        public AgencySettingTab2ViewModel()
        {
            PointofixPath = ConfigManage.ReadAppConfig("PointofixPath");
        }
    }
}
