using iDental.Class;
using iDental.DatabaseAccess.QueryEntities;
using iDental.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace iDental.Views
{
    /// <summary>
    /// PatientCategorySetting.xaml 的互動邏輯
    /// </summary>
    public partial class PatientCategorySetting : Window
    {
        private TablePatientCategorys tablePatientCategorys;

        private PatientCategorySettingViewModel patientCategorySettingViewModel;
        public PatientCategorySetting()
        {
            InitializeComponent();

            patientCategorySettingViewModel = new PatientCategorySettingViewModel();

            DataContext = patientCategorySettingViewModel;

            tablePatientCategorys = new TablePatientCategorys();

        }

        private void Button_CategoryAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PatientCategorys patientCategorys = new PatientCategorys()
                {
                    PatientCategory_Title = textBoxCategoryInput.Text
                };
                tablePatientCategorys.InsertPatientCategorys(patientCategorys);

                DataContext = new PatientCategorySettingViewModel();
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorMessageOutput(ex.ToString());
                MessageBox.Show("新增病患類別錯誤", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DataGrid_Update_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                string editTex = ((TextBox)e.EditingElement).Text;
                PatientCategorys patientCategorys = e.Row.DataContext as PatientCategorys;
                if (!patientCategorys.PatientCategory_Title.Equals(editTex))
                {
                    if (MessageBox.Show("確定將<" + patientCategorys.PatientCategory_Title + ">修改為<" + editTex + ">?\r\n如果是的話，所有擁有此分類的病患分類也會被更動", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {

                        tablePatientCategorys.UpdatePatientCategorysTitle(patientCategorys, editTex);
                    }
                    else
                    {
                        DataContext = new PatientCategorySettingViewModel();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorMessageOutput(ex.ToString());
                MessageBox.Show("資料修改中發生錯誤", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_CategoryDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PatientCategorys patientCategorys = ((FrameworkElement)sender).DataContext as PatientCategorys;
                if (MessageBox.Show("確定刪除<" + patientCategorys.PatientCategory_Title + ">分類?\r\n如果是的話，所有擁有此分類的病患分類也會被取消", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    tablePatientCategorys.DeletePatientCategorys(patientCategorys);
                    DataContext = new PatientCategorySettingViewModel();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorMessageOutput(ex.ToString());
                MessageBox.Show("資料刪除中發生錯誤", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
