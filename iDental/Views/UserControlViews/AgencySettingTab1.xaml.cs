using iDental.ViewModels.UserControlViewModels;
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

        public AgencySettingTab1()
        {
            InitializeComponent();

            agencySettingTab1ViewModel = new AgencySettingTab1ViewModel();
            DataContext = agencySettingTab1ViewModel;
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
    }
}
