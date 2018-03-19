using iDental.Class;
using iDental.DatabaseAccess.QueryEntities;
using iDental.ViewModels;
using System;
using System.Windows;

namespace iDental.Views
{
    /// <summary>
    /// AgencySetting.xaml 的互動邏輯
    /// </summary>
    public partial class AgencySetting : Window
    {
        /// <summary>
        /// 視窗結果
        /// </summary>
        private bool ReturnDialogResult = false;

        private AgencySettingViewModel agencySettingViewModel;

        public AgencySetting()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            agencySettingViewModel = new AgencySettingViewModel();
            DataContext = agencySettingViewModel;
        }

        private void ToggleButton_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //診所設定判斷
                Agencys agencys = agencySettingViewModel.Agencys;
                if (PathCheck.IsPathExist(agencys.Agency_ImagePath))
                {
                    new TableAgencys().UpdateAgency(agencys, agencys.Agency_ImagePath, agencys.Agency_WifiCardPath, agencys.Agency_ViewType, agencys.Function_ID);
                    ReturnDialogResult = true;
                }
                else
                {
                    if (MessageBox.Show("您所設定的影像路徑無法使用，可能會導致影像無法存取，是否繼續?", "提示", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        new TableAgencys().UpdateAgency(agencys, agencys.Agency_ImagePath, agencys.Agency_WifiCardPath, agencys.Agency_ViewType, agencys.Function_ID);
                        ReturnDialogResult = true;
                    }
                }
                //其他
                string PointofixPath = agencySettingViewModel.Pointofix;
                if (PathCheck.IsFileExist(PointofixPath) || string.IsNullOrEmpty(PointofixPath))
                {
                    ConfigManage.AddUpdateAppConfig("PointofixPath", PointofixPath);
                    ReturnDialogResult = true;
                }
                else
                {
                    if (MessageBox.Show("您所設定的Pointofix的位置不存在，可能會導致該軟體無法使用，是否繼續?", "提示", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        ConfigManage.AddUpdateAppConfig("PointofixPath", PointofixPath);
                        ReturnDialogResult = true;
                    }
                }
                string ImageDecodePixel = agencySettingViewModel.ImageDecodePixel;
                ConfigManage.AddUpdateAppConfig("ImageDecodePixel", ImageDecodePixel);
                MessageBox.Show("設定已更改");
            }
            catch (Exception ex)
            {
                MessageBox.Show("儲存設定時發生異常", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                ErrorLog.ErrorMessageOutput(ex.ToString());
            }
            DialogResult = ReturnDialogResult;
            Close();
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
