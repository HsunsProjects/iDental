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
                if (imageSelectedCount > 0)
                {
                    SelectedAll = false;
                    SelectedList = true;
                }
                else
                {
                    SelectedAll = true;
                    SelectedList = false;
                }
                return imageSelectedCount;
            }
            set
            {
                imageSelectedCount = value;
                OnPropertyChanged("ImageSelectedCount");
                OnPropertyChanged("TextBlockTips");
            }
        }

        private int comboPicCount = 0;
        public int ComboPicCount
        {
            get
            {
                return comboPicCount;
            }
            set
            {
                comboPicCount = value;
                OnPropertyChanged("ComboPicCount");
                OnPropertyChanged("TextBlockTips");
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

        private bool selectedAll;
        public bool SelectedAll
        {
            get { return selectedAll; }
            set
            {
                selectedAll = value;
                OnPropertyChanged("SelectedAll");
            }
        }

        private bool selectedList;
        public bool SelectedList
        {
            get { return selectedList; }
            set
            {
                selectedList = value;
                OnPropertyChanged("SelectedList");
            }
        }

        public string TextBlockTips
        {
            get { return "已選取圖片 " + ImageSelectedCount + " 張，共 " + CountImages + "張。" + "組圖選取" + ComboPicCount + "張。"; }
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
