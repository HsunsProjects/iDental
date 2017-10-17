using iDental.Class;
using System;
using System.Windows;

namespace iDental.Views
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                Login login = new Login();
                if (login.ShowDialog() == true)
                {
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
    }
}
