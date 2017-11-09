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
        public Agencys Agencys { get; set; }

        private AgencySettingViewModel agencySettingViewModel;
        public AgencySetting(Agencys agencys)
        {
            InitializeComponent();

            Agencys = agencys;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            agencySettingViewModel = new AgencySettingViewModel(Agencys);
            DataContext = agencySettingViewModel;
        }

        private void ToggleButton_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PathCheck.IsPathExist(agencySettingViewModel.Agency_ImagePath))
                {
                    Agencys = new TableAgencys().UpdateAgency(Agencys, agencySettingViewModel.Agency_ImagePath, agencySettingViewModel.Agency_WifiCardPath, agencySettingViewModel.Agency_ViewType, agencySettingViewModel.Agency_Function);
                    DialogResult = true;
                }
                else
                {
                    if (MessageBox.Show("您所設定的影像路徑無法使用，可能會導致影像無法存取，是否繼續?", "提示", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        Agencys = new TableAgencys().UpdateAgency(Agencys, agencySettingViewModel.Agency_ImagePath, agencySettingViewModel.Agency_WifiCardPath, agencySettingViewModel.Agency_ViewType, agencySettingViewModel.Agency_Function);
                        DialogResult = true;
                    }
                    else
                    {
                        DialogResult = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("儲存設定時發生異常", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                ErrorLog.ErrorMessageOutput(ex.ToString());
                DialogResult = false;
            }
            Close();
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
