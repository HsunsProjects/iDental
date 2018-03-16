using iDental.Class;
using iDental.DatabaseAccess.QueryEntities;
using iDental.ViewModels;
using iDental.Views.UserControlViews;
using System;
using System.Windows;

namespace iDental.Views
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 機構設定(更改後重載病患資料)
        /// </summary>
        private Agencys agencys;
        public Agencys Agencys
        {
            get { return agencys; }
            set { agencys = value; }
        }
        /// <summary>
        /// 病患資料
        /// </summary>
        private Patients Patients { get; set; }
        /// <summary>
        /// 影像路徑是否存在(接受login.xaml回傳
        /// </summary>
        private bool IsExistImagePath = false;
        private MainWindowViewModel mainWindowViewModel;
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                Login login = new Login();
                if (login.ShowDialog() == true)
                {
                    //影像路徑是否存在(接受login.xaml回傳
                    IsExistImagePath = login.IsExistImagePath;
                    //載入機構資料
                    Agencys = login.Agencys;
                    //載入病患資料
                    Patients = login.Patients;
                    //先判斷影像路徑是否存在
                    //不存在提醒路徑有問題
                    //MainWindow 設定
                    mainWindowViewModel = new MainWindowViewModel();
                    DataContext = mainWindowViewModel;
                    //MainWindow 的ContentControl區
                    MainContent.Content = new PatientInformation(Agencys, Patients);
                }
                else
                {
                    Application.Current.Shutdown();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorMessageOutput(ex.ToString());
                Application.Current.Shutdown();
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            //畫面呈現後的提示
            if (!IsExistImagePath)
            {
                MessageBox.Show("影像路徑尚未設置或是出現問題，請先至<設定>確認是否已經填入", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        #region menu function event
        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItem_PatientAdd_Click(object sender, RoutedEventArgs e)
        {
            PatientSetting patientSetting = new PatientSetting("ADD", Agencys);
            if (patientSetting.ShowDialog() == true)
            {
                Patients = patientSetting.Patients;
                MainContent.Content = new PatientInformation(Agencys, Patients);
            }

        }

        private void MenuItem_PatientSearch_Click(object sender, RoutedEventArgs e)
        {
            PatientSearch patientSearch = new PatientSearch();
            if (patientSearch.ShowDialog() == true)
            {
                Patients = patientSearch.Patients;
                MainContent.Content = new PatientInformation(Agencys, Patients);
            }
        }

        private void MenuItem_PatientCategorysManage_Click(object sender, RoutedEventArgs e)
        {
            PatientCategorySetting patientCategorySetting = new PatientCategorySetting();
            patientCategorySetting.ShowDialog();
        }

        private void MenuItem_Setting_Click(object sender, RoutedEventArgs e)
        {
            AgencySetting agencySetting = new AgencySetting();
            if (agencySetting.ShowDialog() == true)
            {
                Agencys = new TableAgencys().QueryVerifyAgencys();
                MainContent.Content = new PatientInformation(Agencys, Patients);
            }
        }

        private void MenuItem_About_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }
        #endregion
    }
}
