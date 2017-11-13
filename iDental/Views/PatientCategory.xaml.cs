using iDental.Class;
using iDental.iDentalClass;
using iDental.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;

namespace iDental.Views
{
    /// <summary>
    /// PatientCategory.xaml 的互動邏輯
    /// </summary>
    public partial class PatientCategory : Window
    {
        public List<PatientCategoryInfo> PatientCategoryInfo { get { return patientCategoryViewModel.PatientCategoryInfo; } }

        private PatientCategoryViewModel patientCategoryViewModel;
        public PatientCategory()
        {
            InitializeComponent();

            patientCategoryViewModel = new PatientCategoryViewModel();

            DataContext = patientCategoryViewModel;
        }

        public PatientCategory(Patients patients)
        {
            InitializeComponent();

            patientCategoryViewModel = new PatientCategoryViewModel(patients);

            DataContext = patientCategoryViewModel;
        }

        private void Button_Search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                patientCategoryViewModel.DisplayPatientCategoryInfo = PatientCategoryInfo.FindAll(w => w.PatientCategory_Title.Contains(textBoxCategoryInput.Text));
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorMessageOutput(ex.ToString());
            }
        }

        private void Button_SearchAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                patientCategoryViewModel.DisplayPatientCategoryInfo = PatientCategoryInfo;
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorMessageOutput(ex.ToString());
            }
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
