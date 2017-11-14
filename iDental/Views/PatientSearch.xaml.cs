using iDental.ViewModels;
using System.Windows;
using System.Linq;
using System;
using iDental.iDentalClass;
using iDental.DatabaseAccess.QueryEntities;

namespace iDental.Views
{
    /// <summary>
    /// PatientSearch.xaml 的互動邏輯
    /// </summary>
    public partial class PatientSearch : Window
    {
        public Patients Patients { get; private set; }
        private PatientSearchViewModel patientSearchViewModel;

        private TablePatients tablePatients;
        private TablePatientCategorys tablePatientCategorys;
        public PatientSearch()
        {
            InitializeComponent();

            patientSearchViewModel = new PatientSearchViewModel();

            DataContext = patientSearchViewModel;

            tablePatients = new TablePatients();

            tablePatientCategorys = new TablePatientCategorys();
        }

        private void Button_PatientSearch_Click(object sender, RoutedEventArgs e)
        {
            using (var ide = new iDentalEntities())
            {
                patientSearchViewModel.DisplayPatientInfo = (from lp in patientSearchViewModel.ListPatientInfo
                                                             where lp.Patient_Number.ToUpper().Contains(textPatientKeyword.Text.ToUpper()) ||
                                                             lp.Patient_Name.ToUpper().Contains(textPatientKeyword.Text.ToUpper()) ||
                                                             lp.Patient_IDNumber.ToUpper().Contains(textPatientKeyword.Text.ToUpper()) ||
                                                             lp.Patient_Birth.ToString("yyyyMMdd").Contains(textPatientKeyword.Text) ||
                                                             lp.Patient_Birth.ToString("yyyy-MM-dd").Contains(textPatientKeyword.Text) ||
                                                             lp.Patient_Birth.ToString("yyyy/MM/dd").Contains(textPatientKeyword.Text)
                                                             select lp).ToList();
                textPatientKeyword.Text = string.Empty;
            }
        }

        private void Button_PatientCategorySearch_Click(object sender, RoutedEventArgs e)
        {
            patientSearchViewModel.ListPatientCategorys = tablePatientCategorys.QueryPatientCategoryItem(textPatientCategoryKeyword.Text);
        }

        private void Button_PatientCategorySearchAll_Click(object sender, RoutedEventArgs e)
        {
            using (var ide = new iDentalEntities())
            {
                patientSearchViewModel.ListPatientCategorys = ide.PatientCategorys.ToList();
                textPatientCategoryKeyword.Text = string.Empty;
            }
        }

        private void Button_PatientCategory_Click(object sender, RoutedEventArgs e)
        {
            PatientCategorys patientCategorys = ((FrameworkElement)sender).DataContext as PatientCategorys;
            patientSearchViewModel.DisplayPatientInfo = (from pc in patientSearchViewModel.ListPatientInfo
                                                         where pc.Patient_PatientCategorys.Any(w => w.PatientCategory_ID == patientCategorys.PatientCategory_ID)
                                                         select pc).ToList();
        }

        private void DataGrid_PatientSelected_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dgPatients.SelectedItem != null)
            {
                PatientInfo patientInfo = dgPatients.SelectedItem as PatientInfo;
                Patients = tablePatients.QueryPatient(patientInfo.Patient_ID);
                DialogResult = true;
                Close();
            }
        }

        private void Button_LastRegistrationDateSearch_Click(object sender, RoutedEventArgs e)
        {
            if (rb1Day.IsChecked == true)
            {
                patientSearchViewModel.DisplayPatientInfo = (from pc in patientSearchViewModel.ListPatientInfo
                                                             where pc.Patient_LastRegistrationDate >= DateTime.Now.Date.AddDays(-1)
                                                             select pc).ToList();
            }
            else if (rb3Day.IsChecked == true)
            {
                patientSearchViewModel.DisplayPatientInfo = (from pc in patientSearchViewModel.ListPatientInfo
                                                             where pc.Patient_LastRegistrationDate >= DateTime.Now.Date.AddDays(-3)
                                                             select pc).ToList();
            }
            else if (rb5Day.IsChecked == true)
            {
                patientSearchViewModel.DisplayPatientInfo = (from pc in patientSearchViewModel.ListPatientInfo
                                                             where pc.Patient_LastRegistrationDate >= DateTime.Now.Date.AddDays(-5)
                                                             select pc).ToList();
            }
            else if (rb7Day.IsChecked == true)
            {
                patientSearchViewModel.DisplayPatientInfo = (from pc in patientSearchViewModel.ListPatientInfo
                                                             where pc.Patient_LastRegistrationDate >= DateTime.Now.Date.AddDays(-7)
                                                             select pc).ToList();
            }
            else if (rb14Day.IsChecked == true)
            {
                patientSearchViewModel.DisplayPatientInfo = (from pc in patientSearchViewModel.ListPatientInfo
                                                             where pc.Patient_LastRegistrationDate >= DateTime.Now.Date.AddDays(-14)
                                                             select pc).ToList();
            }
            else if (rb30Day.IsChecked == true)
            {
                patientSearchViewModel.DisplayPatientInfo = (from pc in patientSearchViewModel.ListPatientInfo
                                                             where pc.Patient_LastRegistrationDate >= DateTime.Now.Date.AddDays(-30)
                                                             select pc).ToList();
            }
            else if (rbRange.IsChecked == true)
            {
                patientSearchViewModel.DisplayPatientInfo = (from pc in patientSearchViewModel.ListPatientInfo
                                                             where pc.Patient_LastRegistrationDate.Date >= patientSearchViewModel.SelectStartDate.Date &&
                                                             pc.Patient_LastRegistrationDate.Date <= patientSearchViewModel.SelectEndDate.Date
                                                             select pc).ToList();
            }
        }

        private void Button_PatientAll_Click(object sender, RoutedEventArgs e)
        {
            patientSearchViewModel.DisplayPatientInfo = patientSearchViewModel.ListPatientInfo;
        }
    }
}
