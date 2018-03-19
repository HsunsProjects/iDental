using iDental.ViewModels.UserControlViewModels;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace iDental.Views.UserControlViews
{
    /// <summary>
    /// AgencySettingTab2.xaml 的互動邏輯
    /// </summary>
    public partial class AgencySettingTab2 : UserControl
    {
        public string PointofixPath { get { return agencySettingTab2ViewModel.PointofixPath; } }
        public string ImageDecodePixel { get { return agencySettingTab2ViewModel.ImageDecodePixel; } }

        private AgencySettingTab2ViewModel agencySettingTab2ViewModel;
        public AgencySettingTab2()
        {
            InitializeComponent();

            agencySettingTab2ViewModel = new AgencySettingTab2ViewModel();
            DataContext = agencySettingTab2ViewModel;
        }

        private void Button_PointofixPath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Multiselect = true,
                DefaultExt = ".exe",
                InitialDirectory = @"C:\",
                Filter = "EXE Files (*.exe)|*.exe|All Files (*.*)|*.*"
            };

            bool? ofdResult = ofd.ShowDialog();
            if (ofdResult.HasValue && ofdResult.Value)
            {
                agencySettingTab2ViewModel.PointofixPath = ofd.FileName;
            }
        }
    }
}
