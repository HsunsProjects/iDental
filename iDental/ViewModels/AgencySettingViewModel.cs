using iDental.Views.UserControlViews;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace iDental.ViewModels
{
    public class AgencySettingViewModel : ViewModelBase.PropertyChangedBase
    {
        /// <summary>
        /// 取得AgencySettingTab1值
        /// </summary>
        public Agencys Agencys { get { return agencySettingTab1.Agencys; } }
        /// <summary>
        /// 取得AgencySettingTab2值
        /// </summary>
        public string Pointofix { get { return agencySettingTab2.PointofixPath; } }
        /// <summary>
        /// 取得AgencySettingTab2值
        /// </summary>
        public string ImageDecodePixel { get { return agencySettingTab2.ImageDecodePixel; } }

        public AgencySettingViewModel()
        {
            TabsSetting();
        }

        #region FinctionTab
        /// <summary>
        /// binding Tab ItemSource來源
        /// </summary>
        private ObservableCollection<TabItem> agencySettingTabs;
        public ObservableCollection<TabItem> AgencySettingTabs
        {
            get { return agencySettingTabs; }
            set
            {
                agencySettingTabs = value;
                OnPropertyChanged("AgencySettingTabs");
            }
        }

        /// <summary>
        /// Selected Tab頁面(載入圖片)
        /// </summary>
        private TabItem selectedTabItem;
        public TabItem SelectedTabItem
        {
            get { return selectedTabItem; }
            set
            {
                selectedTabItem = value;
                OnPropertyChanged("SelectedTabItem");
            }
        }
        #endregion

        /// <summary>
        /// Main Tab
        /// </summary>
        private AgencySettingTab1 agencySettingTab1 = new AgencySettingTab1();
        /// <summary>
        /// Other Tab
        /// </summary>
        private AgencySettingTab2 agencySettingTab2 = new AgencySettingTab2();

        private void TabsSetting()
        {
            AgencySettingTabs = new ObservableCollection<TabItem>();
            //AgencySettingTab1
            AgencySettingTabs.Add(new TabItem()
            {
                Header = "主要設定",
                Content = agencySettingTab1
            });
            //AgencySettingTab2
            AgencySettingTabs.Add(new TabItem()
            {
                Header = "其他",
                Content = agencySettingTab2
            });
            SelectedTabItem = AgencySettingTabs[0];
        }
    }
}
