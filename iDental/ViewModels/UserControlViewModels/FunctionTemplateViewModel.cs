using iDental.iDentalClass;
using iDental.ViewModels.ViewModelBase;
using System.Windows.Controls;

namespace iDental.ViewModels.UserControlViewModels
{
    public class FunctionTemplateViewModel : PropertyChangedBase
    {
        private MTObservableCollection<ImageInfo> displayImageInfo;
        public MTObservableCollection<ImageInfo> DisplayImageInfo
        {
            get { return displayImageInfo; }
            set
            {
                displayImageInfo = value;
                OnPropertyChanged("DisplayImageInfo");
            }
        }

        #region 頁面設定

        private int columnSpan;
        public int ColumnSpan
        {
            get { return columnSpan; }
            set
            {
                columnSpan = value;
                OnPropertyChanged("ColumnSpan");
            }
        }

        private int rowSpan;
        public int RowSpan
        {
            get { return rowSpan; }
            set
            {
                rowSpan = value;
                OnPropertyChanged("RowSpan");
            }
        }

        private int stretchWidth = 270;
        public int StretchWidth
        {
            get { return stretchWidth; }
            set
            {
                stretchWidth = value;
                OnPropertyChanged("StretchWidth");
            }
        }

        private int stretchHeight = 205;
        public int StretchHeight
        {
            get { return stretchHeight; }
            set
            {
                stretchHeight = value;
                OnPropertyChanged("StretchHeight");
            }
        }

        private string buttonStretchContent;
        public string ButtonStretchContent
        {
            get { return buttonStretchContent; }
            set
            {
                buttonStretchContent = value;
                OnPropertyChanged("ButtonStretchContent");
            }
        }

        private int buttonStretchColumn;
        public int ButtonStretchColumn
        {
            get { return buttonStretchColumn; }
            set
            {
                buttonStretchColumn = value;
                OnPropertyChanged("ButtonStretchColumn");
            }
        }

        private int buttonStretchRow;
        public int ButtonStretchRow
        {
            get { return buttonStretchRow; }
            set
            {
                buttonStretchRow = value;
                OnPropertyChanged("ButtonStretchRow");
            }
        }

        private int buttonStretchWidth;
        public int ButtonStretchWidth
        {
            get { return buttonStretchWidth; }
            set
            {
                buttonStretchWidth = value;
                OnPropertyChanged("ButtonStretchWidth");
            }
        }

        private int buttonStretchHeight;
        public int ButtonStretchHeight
        {
            get { return buttonStretchHeight; }
            set
            {
                buttonStretchHeight = value;
                OnPropertyChanged("ButtonStretchHeight");
            }
        }

        private int listColumn;
        public int ListColumn
        {
            get { return listColumn; }
            set
            {
                listColumn = value;
                OnPropertyChanged("ListColumn");
            }
        }

        private int listRow;
        public int ListRow
        {
            get { return listRow; }
            set
            {
                listRow = value;
                OnPropertyChanged("ListRow");
            }
        }

        private int listItemColumn;
        public int ListItemColumn
        {
            get { return listItemColumn; }
            set
            {
                listItemColumn = value;
                OnPropertyChanged("ListItemColumn");
            }
        }

        private int listItemRow;
        public int ListItemRow
        {
            get { return listItemRow; }
            set
            {
                listItemRow = value;
                OnPropertyChanged("ListItemRow");
            }
        }

        private ScrollBarVisibility listHSBV;

        public ScrollBarVisibility ListHSBV
        {
            get { return listHSBV; }
            set
            {
                listHSBV = value;
                OnPropertyChanged("ListHSBV");
            }
        }

        private ScrollBarVisibility listVSBV;

        public ScrollBarVisibility ListVSBV
        {
            get { return listVSBV; }
            set
            {
                listVSBV = value;
                OnPropertyChanged("ListVSBV");
            }
        }

        private Orientation wrapOrientation;
        public Orientation WrapOrientation
        {
            get { return wrapOrientation; }
            set
            {
                wrapOrientation = value;
                OnPropertyChanged("WrapOrientation");
            }
        }
        #endregion

        public FunctionTemplateViewModel(Agencys agencys)
        {
            SetTemplateLayout(agencys);
        }

        private void SetTemplateLayout(Agencys agencys)
        {
            //設定Grid 橫向或直向
            //0:橫幅 1:直幅
            switch (agencys.Agency_ViewType)
            {
                case "0":
                    ColumnSpan = 1;
                    RowSpan = 3;
                    ButtonStretchContent = ">";
                    ButtonStretchColumn = 1;
                    ButtonStretchRow = 0;
                    ButtonStretchWidth = 15;
                    ButtonStretchHeight = 60;
                    ListColumn = 2;
                    ListRow = 0;
                    ListItemColumn = 1;
                    ListHSBV = ScrollBarVisibility.Hidden;
                    ListVSBV = ScrollBarVisibility.Visible;
                    WrapOrientation = Orientation.Vertical;
                    break;
                case "1":
                    ColumnSpan = 3;
                    RowSpan = 1;
                    ButtonStretchContent = "﹀";
                    ButtonStretchColumn = 0;
                    ButtonStretchRow = 1;
                    ButtonStretchWidth = 60;
                    ButtonStretchHeight = 15;
                    ListColumn = 0;
                    ListRow = 2;
                    ListItemRow = 1;
                    ListHSBV = ScrollBarVisibility.Visible;
                    ListVSBV = ScrollBarVisibility.Hidden;
                    WrapOrientation = Orientation.Horizontal;
                    break;
            }
        }
    }
}
