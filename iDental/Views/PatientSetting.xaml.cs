using iDental.Class;
using iDental.DatabaseAccess.QueryEntities;
using iDental.iDentalClass;
using iDental.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;

namespace iDental.Views
{
    /// <summary>
    /// PatientSetting.xaml 的互動邏輯
    /// </summary>
    public partial class PatientSetting : Window
    {
        private Agencys Agencys { get; set; }
        public Patients Patients { get; set; }
        private string WindowType { get; set; }

        private PatientSettingViewModel patientSettingViewModel;

        private TablePatients tablePatients;
        private TablePatientCategorys tablePatientCategorys;

        /// <summary>
        /// 新增
        /// </summary>
        public PatientSetting(string windowType, Agencys agencys)
        {
            InitializeComponent();
            Agencys = agencys;
            WindowType = windowType;
        }

        /// <summary>
        /// 編輯
        /// </summary>
        /// <param name="windowTitle"></param>
        /// <param name="agencys"></param>
        /// <param name="patients"></param>
        public PatientSetting(string windowType, Agencys agencys, Patients patients)
        {
            InitializeComponent();
            Agencys = agencys;
            Patients = patients;
            WindowType = windowType;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Patients != null)
            {
                patientSettingViewModel = new PatientSettingViewModel(WindowType, Agencys, Patients);
            }
            else
            {
                patientSettingViewModel = new PatientSettingViewModel(WindowType);
            }

            DataContext = patientSettingViewModel;

            tablePatients = new TablePatients();

            tablePatientCategorys = new TablePatientCategorys();
        }

        private void Button_PatientCategorySetting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PatientCategory patientCategory;
                if (Patients != null)//編輯
                {
                    patientCategory = new PatientCategory(Patients);
                }
                else//新增
                {
                    patientCategory = new PatientCategory();
                }
                if (patientCategory.ShowDialog() == true)
                {
                    patientSettingViewModel.PatientCategoryInfo = patientCategory.PatientCategoryInfo.FindAll(s => s.IsChecked == true);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorMessageOutput(ex.ToString());
                MessageBox.Show("回傳病患分類失敗", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Patients != null)//編輯
                {
                    if (MessageBox.Show("確定修改病患資料<" + patientSettingViewModel.Patient_Number + ">?", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        //新的影像路徑
                        string newPatientPhotoPath = null;
                        if (!IsRemove)
                        {
                            //如果沒觸發移除，要判斷有沒有匯入的路徑
                            if (!string.IsNullOrEmpty(ImportPatientPhotoPath))
                            {
                                //有，蓋掉舊圖
                                if (PathCheck.IsFileExist(ImportPatientPhotoPath))
                                {
                                    //病患大頭照路徑
                                    PatientPhotoFolderInfo patientPhotoFolderInfo = PatientFolderSetting.PatientPhotoFolderSetting(Agencys, Patients.Patient_ID);
                                    //建立大頭照路徑
                                    PathCheck.CheckPathAndCreate(patientPhotoFolderInfo.PatientPhotoFullPath);

                                    string extension = Path.GetExtension(ImportPatientPhotoPath).ToUpper();
                                    string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff");

                                    File.Copy(ImportPatientPhotoPath, patientPhotoFolderInfo.PatientPhotoFullPath + @"\" + newFileName + extension);
                                    Thread.Sleep(500);

                                    newPatientPhotoPath = patientPhotoFolderInfo.PatientPhotoPath + @"\" + newFileName + extension;
                                }
                                else
                                {
                                    MessageBox.Show("圖片路徑出現問題", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                            else
                            {
                                //沒有，保留原圖
                                newPatientPhotoPath = patientSettingViewModel.Patients.Patient_Photo;
                            }
                        }
                        Patients newPatient = new Patients()
                        {
                            Patient_ID = patientSettingViewModel.Patients.Patient_ID,
                            Patient_Number = patientSettingViewModel.Patient_Number,
                            Patient_Name = patientSettingViewModel.Patient_Name,
                            Patient_Gender = patientSettingViewModel.Patient_Gender,
                            Patient_Birth = patientSettingViewModel.Patient_Birth,
                            Patient_IDNumber = patientSettingViewModel.Patient_IDNumber,
                            Patient_Photo = newPatientPhotoPath,
                            Patient_FirstRegistrationDate = patientSettingViewModel.Patient_FirstRegistrationDate,
                            UpdateDate = DateTime.Now
                        };
                        Patients = tablePatients.UpdatePatients(newPatient);

                        //寫入分類
                        List<PatientCategoryInfo> PatientCategoryInfo = patientSettingViewModel.PatientCategoryInfo.FindAll(pci => pci.IsChecked == true);
                        tablePatientCategorys.InsertPatientsPatientCategorys(Patients, PatientCategoryInfo);

                        MessageBox.Show("修改完成", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        DialogResult = true;
                        Close();
                    }
                }
                else//新增
                {
                    if (MessageBox.Show("確定新增病患<" + patientSettingViewModel.Patient_Number + ">?", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        string newPatientID = GetPatientID();

                        string newPatientPhotoPath = null;
                        if (!string.IsNullOrEmpty(ImportPatientPhotoPath))
                        {
                            if (PathCheck.IsFileExist(ImportPatientPhotoPath))
                            {
                                //病患大頭照路徑
                                PatientPhotoFolderInfo patientPhotoFolderInfo = PatientFolderSetting.PatientPhotoFolderSetting(Agencys, newPatientID);
                                //建立大頭照路徑
                                PathCheck.CheckPathAndCreate(patientPhotoFolderInfo.PatientPhotoFullPath);

                                string extension = Path.GetExtension(ImportPatientPhotoPath).ToUpper();
                                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff");

                                File.Copy(ImportPatientPhotoPath, patientPhotoFolderInfo.PatientPhotoFullPath + @"\" + newFileName + extension);
                                Thread.Sleep(500);

                                newPatientPhotoPath = patientPhotoFolderInfo.PatientPhotoPath + @"\" + newFileName + extension;
                            }
                            else
                            {
                                MessageBox.Show("圖片路徑出現問題", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        DateTime nowDateTime = DateTime.Now;
                        //新增病患
                        Patients newPatient = new Patients()
                        {
                            Patient_ID = newPatientID,
                            Patient_Number = patientSettingViewModel.Patient_Number,
                            Patient_Name = patientSettingViewModel.Patient_Name,
                            Patient_Gender = patientSettingViewModel.Patient_Gender,
                            Patient_Birth = patientSettingViewModel.Patient_Birth,
                            Patient_IDNumber = patientSettingViewModel.Patient_IDNumber,
                            Patient_Photo = newPatientPhotoPath,
                            Patient_FirstRegistrationDate = patientSettingViewModel.Patient_FirstRegistrationDate,
                            UpdateDate = nowDateTime,
                            CreateDate = nowDateTime
                        };
                        Patients = tablePatients.InsertPatients(newPatient);
                        //寫入分類
                        //寫入分類
                        List<PatientCategoryInfo> PatientCategoryInfo = patientSettingViewModel.PatientCategoryInfo.FindAll(pci => pci.IsChecked == true);
                        if (PatientCategoryInfo.Count() > 0)
                        {
                            tablePatientCategorys.InsertPatientsPatientCategorys(Patients, PatientCategoryInfo);
                        }

                        MessageBox.Show("新增完成", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        DialogResult = true;
                        Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorMessageOutput(ex.ToString());
                MessageBox.Show("儲存資料發生錯誤", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_PatientCategoryRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //取出選定的分類
                PatientCategoryInfo patientCategoryInfo = ((FrameworkElement)e.Source).DataContext as PatientCategoryInfo;
                patientCategoryInfo.IsChecked = false;
                patientSettingViewModel.PatientCategoryInfo = patientSettingViewModel.PatientCategoryInfo.FindAll(spcs => spcs.IsChecked == true);
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorMessageOutput(ex.ToString());
            }
        }

        private string GetPatientID()
        {
            byte[] newPatientIDByte = Guid.NewGuid().ToByteArray();
            string newPatientID = BitConverter.ToInt64(newPatientIDByte, 0).ToString();
            if (tablePatients.IsUniquePatientID(newPatientID))
            {
                return newPatientID;
            }
            else
            {
                Thread.Sleep(1000);
                return GetPatientID();
            }
        }

        private bool IsRemove = false;
        private string ImportPatientPhotoPath = string.Empty;
        private void Button_Import_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                DefaultExt = ".png",
                Filter = "All Files (*.*)|*.*|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif"
            };
            bool? ofdResult = ofd.ShowDialog();
            if (ofdResult.HasValue && ofdResult.Value)//OpenFileDialog 選確定
            {
                IsRemove = false;
                ImportPatientPhotoPath = ofd.FileName;
                patientSettingViewModel.Patient_Photo = new CreateBitmapImage().SettingBitmapImage(ImportPatientPhotoPath, 400);
            }
        }

        private void Button_Remove_Click(object sender, RoutedEventArgs e)
        {
            IsRemove = true;
            ImportPatientPhotoPath = string.Empty;
            patientSettingViewModel.Patient_Photo = new BitmapImage(new Uri(@"pack://application:,,,/iDental;component/Resource/Image/avatar.jpg", UriKind.Absolute));
        }
    }
}
