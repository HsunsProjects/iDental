using iDental.iDentalClass;
using iDental.ViewModels.ViewModelBase;

namespace iDental.ViewModels.UserControlViewModels
{
    public class FunctionListViewModel : PropertyChangedBase
    {
        private MTObservableCollection<ImageInfo> displayImageInfo;
        public MTObservableCollection<ImageInfo> DisplayImageInfo
        {
            get { return displayImageInfo; }
            set
            {
                displayImageInfo = value;
                OnPropertyChanged("DisplayImageInfo");
                CountImages = displayImageInfo.Count;
            }
        }
        #region 頁面設定
        private int imageSelectedCount = 0;
        public int ImageSelectedCount
        {
            get
            {
                return imageSelectedCount;
            }
            set
            {
                imageSelectedCount = value;
                OnPropertyChanged("ImageSelectedCount");
                OnPropertyChanged("TextBlockTips");
                OnPropertyChanged("SelectedStatus");
            }
        }

        private int countImages = 0;
        public int CountImages
        {
            get { return countImages; }
            set
            {
                countImages = value;
                OnPropertyChanged("CountImages");
                OnPropertyChanged("TextBlockTips");
            }
        }

        public string SelectedStatus
        {
            get
            {
                if (ImageSelectedCount > 0 && ImageSelectedCount < CountImages)
                    return "選取" + ImageSelectedCount;
                else
                    return "全部";
            }
        }

        public string TextBlockTips
        {
            get { return "已選取圖片 " + ImageSelectedCount + " 張，共 " + CountImages + " 張"; }
        }

        /// <summary>
        /// 用來 Binding Slider 預設3
        /// </summary>
        private int columnCount = 3;
        public int ColumnCount
        {
            get { return columnCount; }
            set
            {
                columnCount = value;
                OnPropertyChanged("ColumnCount");
            }
        }
        #endregion
    }
}
