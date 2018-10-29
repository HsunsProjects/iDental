using iDental.ViewModels.UserControlViewModels;
using Saraff.Twain;
using System.Windows;
using System.Windows.Controls;

namespace iDental.Views.UserControlViews
{
    /// <summary>
    /// AgencySettingTab1.xaml 的互動邏輯
    /// </summary>
    public partial class AgencySettingTab1 : UserControl
    {
        public Agencys Agencys { get { return agencySettingTab1ViewModel.Agencys; } }

        private AgencySettingTab1ViewModel agencySettingTab1ViewModel;

        private Twain32 twain32;

        public AgencySettingTab1()
        {
            InitializeComponent();

            agencySettingTab1ViewModel = new AgencySettingTab1ViewModel();
            DataContext = agencySettingTab1ViewModel;

            twain32 = new Twain32();
        }

        private void Button_ImagePath_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            using (System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "請選擇影像路徑";
                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    agencySettingTab1ViewModel.Agency_ImagePath = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void Button_WifiCardPath_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            using (System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "請選擇WifiCard路徑";
                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    agencySettingTab1ViewModel.Agency_WifiCardPath = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void Button_TwainDevice_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            twain32.Country = TwCountry.TAIWAN;
            twain32.Language = TwLanguage.CHINESE_TAIWAN;
            twain32.IsTwain2Enable = true;

            twain32.OpenDSM();
            if (twain32.IsTwain2Supported)
            {
                if (twain32.SourcesCount > 0)
                {
                    twain32.CloseDataSource();
                    if (twain32.SelectSource() == true)
                    {
                        agencySettingTab1ViewModel.TwainDeviceName = twain32.GetSourceProductName(twain32.GetDefaultSource());
                    }
                    else
                    {
                        agencySettingTab1ViewModel.TwainDeviceName = string.Empty;
                    }
                }
                else
                {
                    MessageBox.Show("尚未找到掃描裝置(TWAIN)", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            twain32.CloseDSM();
        }
    }
}
