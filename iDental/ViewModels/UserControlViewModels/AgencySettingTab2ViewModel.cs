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

        private string imageDecodePixel;
        public string ImageDecodePixel
        {
            get { return imageDecodePixel; }
            set
            {
                imageDecodePixel = value;
                OnPropertyChanged("ImageDecodePixel");
            }
        }

        public AgencySettingTab2ViewModel()
        {
            //先建立config設定
            CheckAppConfigSetting();

            PointofixPath = ConfigManage.ReadAppConfig("PointofixPath");
            ImageDecodePixel = ConfigManage.ReadAppConfig("ImageDecodePixel");
        }

        /// <summary>
        /// 判斷config 設定是否存在 並預設
        /// </summary>
        private void CheckAppConfigSetting()
        {
            //如果沒有PointofixPath,建立config,key = PointofixPath
            ConfigManage.CreateConfig("PointofixPath");
            //如果沒有ImageDecodePixel,建立config,key = ImageDecodePixel, value = 0
            ConfigManage.CreateConfig("ImageDecodePixel", "0");
        }
    }
}
