using iDental.Class;
using System;
using System.Windows;

namespace iDental
{
    /// <summary>
    /// App.xaml 的互動邏輯
    /// </summary>
    public partial class App : Application
    {
        public Patients Patients = new Patients();
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                if (e.Args.Length > 0)
                {
                    Patients.Patient_ID = !string.IsNullOrEmpty(e.Args[0].ToString()) ? e.Args[0].ToString() : string.Empty;
                    Patients.Patient_Number = !string.IsNullOrEmpty(e.Args[1].ToString()) ? e.Args[1].ToString() : string.Empty;
                    Patients.Patient_Name = !string.IsNullOrEmpty(e.Args[2].ToString()) ? e.Args[2].ToString() : string.Empty;
                    Patients.Patient_Gender = TransGender(e.Args[3].ToString());
                    Patients.Patient_Birth = DateTime.TryParse(e.Args[4].ToString(), out DateTime patientBirth) ? DateTime.Parse(e.Args[4].ToString()) : default(DateTime);
                    Patients.Patient_IDNumber = !string.IsNullOrEmpty(e.Args[5].ToString()) ? e.Args[5].ToString() : string.Empty;
                }
                else
                {
                    //測試資料
                    //1. Patient = null
                    Patients = new Patients();
                    //2.Patient Testing
                    //Patients = new Patients()
                    //{
                    //    Patient_ID = "0001",
                    //    Patient_Number = "E0001",
                    //    Patient_Name = "Eason",
                    //    Patient_Gender = true,
                    //    Patient_Birth = DateTime.Parse("1986-08-11"),
                    //    Patient_IDNumber = "W100399932"
                    //};
                }
                base.OnStartup(e);
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorMessageOutput(ex.ToString());
                MessageBox.Show("帶入的參數有誤，DigiDental無法啟動", "警告", MessageBoxButton.OK, MessageBoxImage.Error);
                Current.Shutdown();
            }
        }
        private bool TransGender(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                if (input.ToUpper().Equals("M"))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
