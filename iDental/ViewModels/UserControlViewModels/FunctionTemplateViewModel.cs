using iDental.DatabaseAccess.QueryEntities;
using iDental.iDentalClass;
using iDental.ViewModels.ViewModelBase;
using iDental.Views.UserControlViews.FunctionTemplates;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace iDental.ViewModels.UserControlViewModels
{
    public class FunctionTemplateViewModel : PropertyChangedBase
    {
        public Agencys Agencys { get; set; }
        public Patients Patients { get; set; }

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

        #region 頁面配置設定

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

        private GridLength stretchWidth = new GridLength(270, GridUnitType.Pixel);
        public GridLength StretchWidth
        {
            get { return stretchWidth; }
            set
            {
                stretchWidth = value;
                OnPropertyChanged("StretchWidth");
            }
        }

        private GridLength stretchHeight = new GridLength(205, GridUnitType.Pixel);
        public GridLength StretchHeight
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
        #region 頁面Control設定
        /// <summary>
        /// 是否可以使用自動拍攝功能
        /// </summary>
        private bool autoImportEnable = false;
        public bool AutoImportEnable
        {
            get { return autoImportEnable; }
            set
            {
                autoImportEnable = value;
                OnPropertyChanged("AutoImportEnable");
            }
        }
        /// <summary>
        /// 統計圖片總和
        /// </summary>
        private int countImages = 0;
        public int CountImages
        {
            get { return countImages; }
            set
            {
                countImages = value;
                OnPropertyChanged("CountImages");
            }
        }

        #endregion

        #region TemplateConent 設定
        
        private UserControl templateContent;
        public UserControl TemplateContent
        {
            get { return templateContent; }
            set
            {
                templateContent = value;
                OnPropertyChanged("TemplateContent");
            }
        }

        #endregion
        #region 載入ComboBox Templates 清單
        /// <summary>
        /// Binding 選擇樣板日期
        /// </summary>
        private DateTime selectedDate = DateTime.Now.Date;
        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set
            {
                if (selectedDate != value)
                {
                    selectedDate = value;
                    OnPropertyChanged("SelectedDate");
                    if (SelectedTemplate != null)
                    {
                        SetTemplateContent(SelectedTemplate);
                    }
                }
            }
        }
        /// <summary>
        /// Binding ComboBox 樣板清單
        /// </summary>
        private ObservableCollection<Templates> templates;
        public ObservableCollection<Templates> Templates
        {
            get { return templates; }
            set
            {
                templates = value;
                OnPropertyChanged("Templates");

            }
        }
        /// <summary>
        /// Binding ComboBox Selected
        /// </summary>
        private Templates selectedTemplate;
        public Templates SelectedTemplate
        {
            get { return selectedTemplate; }
            set
            {
                if (selectedTemplate != value)
                {
                    selectedTemplate = value;
                    OnPropertyChanged("SelectedTemplate");
                    SetTemplateContent(selectedTemplate);
                    if (selectedTemplate != null)
                    {
                        AutoImportEnable = true;
                    }
                    else
                    {
                        AutoImportEnable = false;
                    }
                }
            }
        }

        /// <summary>
        /// Binding 瀏覽ComboBox
        /// </summary>
        private ObservableCollection<string> importDateCollect = new ObservableCollection<string>();

        public ObservableCollection<string> ImportDateCollect
        {
            get { return importDateCollect; }
            set
            {
                importDateCollect = value;
                OnPropertyChanged("ImportDateCollect");
            }
        }
        /// <summary>
        /// Binding Selected 瀏覽ComboBox
        /// </summary>
        private string selectedImportDate;
        public string SelectedImportDate
        {
            get { return selectedImportDate; }
            set
            {
                if (selectedImportDate != value)
                {
                    selectedImportDate = value;
                    OnPropertyChanged("SelectedImportDate");
                    if (selectedImportDate != null)
                        SelectedDate = DateTime.Parse(selectedImportDate);
                    else
                        SelectedDate = SelectedDate;
                }
            }
        }

        #endregion
        public FunctionTemplateViewModel(Agencys agencys, Patients patients)
        {
            Agencys = agencys;
            Patients = patients;

            SetTemplateLayout(agencys);
            //載入樣板
            Templates = new TableTemplates().QueryAllTemplates();
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

        private void SetTemplateContent(Templates templateItem)
        {
            switch (templateItem.Template_UserControlName)
            {
                case "TBeforeAfter":
                    TBeforeAfter tBeforeAfter = new TBeforeAfter(Agencys, Patients, templateItem, SelectedDate);
                    TemplateContent = tBeforeAfter;
                    break;
                case "TIn5s":
                    TIn5s tIn5s = new TIn5s(Agencys, Patients, templateItem, SelectedDate);
                    TemplateContent = tIn5s;
                    break;
                case "TIn6s":
                    TIn6s tIn6s = new TIn6s(Agencys, Patients, templateItem, SelectedDate);
                    TemplateContent = tIn6s;
                    break;
                case "TIn9s":
                    TIn9s tIn9s = new TIn9s(Agencys, Patients, templateItem, SelectedDate);
                    TemplateContent = tIn9s;
                    break;
                case "TInOut9s":
                    TInOut9s tInOut9s = new TInOut9s(Agencys, Patients, templateItem, SelectedDate);
                    TemplateContent = tInOut9s;
                    break;
                case "TInOut10s":
                    TInOut10s tInOut10s = new TInOut10s(Agencys, Patients, templateItem, SelectedDate);
                    TemplateContent = tInOut10s;
                    break;
                case "TInOut11s":
                    TInOut11s tInOut11s = new TInOut11s(Agencys, Patients, templateItem, SelectedDate);
                    TemplateContent = tInOut11s;
                    break;
                case "TXRay6s":
                    TXRay6s tXRay6s = new TXRay6s(Agencys, Patients, templateItem, SelectedDate);
                    TemplateContent = tXRay6s;
                    break;
                case "TXRay19s":
                    TXRay19s tXRay19s = new TXRay19s(Agencys, Patients, templateItem, SelectedDate);
                    TemplateContent = tXRay19s;
                    break;
                case "TPlasterModel5s":
                    TPlasterModel5s tPlasterModel5s = new TPlasterModel5s(Agencys, Patients, templateItem, SelectedDate);
                    TemplateContent = tPlasterModel5s;
                    break;
                case "TFdi52s":
                    TFdi52s tFdi52s = new TFdi52s(Agencys, Patients, templateItem, SelectedDate);
                    TemplateContent = tFdi52s;
                    break;
                case "TOthers1s":
                    TOthers1s tOthers1s = new TOthers1s(Agencys, Patients, templateItem, SelectedDate);
                    TemplateContent = tOthers1s;
                    break;
            }

            ImportDateCollect = new TableTemplates_Images().QueryAllTemplatesImagesImportDate(Patients, templateItem);
            SelectedImportDate = (from idc in ImportDateCollect
                                  where idc == selectedDate.ToString("yyyy/MM/dd")
                                  select idc).ToList().Count() > 0 ? selectedDate.ToString("yyyy/MM/dd") : null;
        }
    }
}
