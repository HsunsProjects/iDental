using iDental.Class;
using iDental.iDentalClass;
using iDental.ViewModels.UserControlViewModels;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace iDental.Views.UserControlViews
{
    /// <summary>
    /// PatientInformation.xaml 的互動邏輯
    /// </summary>
    public partial class PatientInformation : UserControl
    {
        private Agencys Agencys { get; set; }
        private Patients Patients { get; set; }

        private PatientInformationViewModel patientInformationViewModel;

        public PatientInformation(Agencys agencys, Patients patients)
        {
            InitializeComponent();

            Agencys = agencys;
            Patients = patients;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Patients != null)
                {
                    patientInformationViewModel = new PatientInformationViewModel(Agencys, Patients);
                }
                else
                {
                    patientInformationViewModel = new PatientInformationViewModel();
                }

                DataContext = patientInformationViewModel;
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorMessageOutput(ex.ToString());
            }
        }

        #region event

        private void Button_Stretch_Click(object sender, RoutedEventArgs e)
        {
            GridLength dataGridLength = PatientInformationBlock.Width;
            if (dataGridLength.Value.Equals(0))
            {
                PatientInformationBlock.Width = new GridLength(200, GridUnitType.Pixel);
                ButtonStretch.Content = "<";
            }
            else
            {
                PatientInformationBlock.Width = new GridLength(0, GridUnitType.Pixel);
                ButtonStretch.Content = ">";
            }
        }

        private void Button_Import_Click(object sender, RoutedEventArgs e)
        {
            btnImport.Dispatcher.Invoke(() =>
            {
                btnImport.IsEnabled = false;
            });

            OpenFileDialog ofd = new OpenFileDialog()
            {
                Multiselect = true,
                DefaultExt = ".png",
                Filter = "All Files (*.*)|*.*|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif"
            };

            bool? ofdResult = ofd.ShowDialog();
            if (ofdResult.HasValue && ofdResult.Value)
            {
                if (PathCheck.IsPathExist(Agencys.Agency_ImagePath))
                {
                    //讀寫Registrations
                    //確認掛號資料
                    DateTime RegistrationDate = patientInformationViewModel.ImportDate;                

                    //設定病患資料夾
                    PatientImageFolderInfo patientImageFolderInfo = PatientFolderSetting.PatientImageFolderSetting(Agencys, Patients, RegistrationDate);
                    //檢查是否存在，不存在就新增
                    PathCheck.CheckPathAndCreate(patientImageFolderInfo.PatientImageFullPath);

                    ProgressDialog progressDialog = new ProgressDialog();

                    progressDialog.Dispatcher.Invoke(() =>
                    {
                        progressDialog.PMinimum = 0;
                        progressDialog.PValue = 0;
                        progressDialog.PMaximum = ofd.FileNames.Count();
                        progressDialog.PText = "圖片匯入中，請稍後( 0" + " / " + progressDialog.PMaximum + " )";
                        progressDialog.Show();
                    });

                    Registrations registrations = new Registrations();

                    Task t = Task.Factory.StartNew(() =>
                    {
                        using (var ide = new iDentalEntities())
                        {
                            var queryRegistrations = from r in ide.Registrations
                                                     where r.Patient_ID == Patients.Patient_ID && r.Registration_Date == RegistrationDate.Date
                                                     select r;
                            if (queryRegistrations.Count() > 0)
                            {
                                registrations = queryRegistrations.First();
                            }
                            else
                            {
                                registrations.Patient_ID = Patients.Patient_ID;
                                registrations.Registration_Date = RegistrationDate.Date;
                                ide.Registrations.Add(registrations);
                                //寫入Registrations
                                ide.SaveChanges();
                            }

                            foreach (string fileName in ofd.FileNames)
                            {
                                string extension = Path.GetExtension(fileName).ToUpper();
                                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                                string imageFileName = newFileName + extension;

                                //判斷圖片是否為正
                                //不是，旋轉後儲存
                                //是，直接File.Copy
                                ImageHelper.RotateImageByExifOrientationData(fileName, patientImageFolderInfo.PatientImageFullPath + @"\" + newFileName + extension, extension, true);

                                Images images = new Images()
                                {
                                    Image_Path = @"\" + patientImageFolderInfo.PatientImagePath + @"\" + imageFileName,
                                    Image_FileName = imageFileName,
                                    Image_Extension = extension
                                };
                                registrations.Images.Add(images);

                                progressDialog.Dispatcher.Invoke(() =>
                                {
                                    progressDialog.PValue++;
                                    progressDialog.PText = "圖片匯入中，請稍後( " + progressDialog.PValue + " / " + progressDialog.PMaximum + " )";
                                });

                                Thread.Sleep(200);
                            }

                            //Images
                            ide.SaveChanges();
                        }
                    }).ContinueWith(cw =>
                    {
                        progressDialog.Dispatcher.Invoke(() =>
                        {
                            progressDialog.PText = "匯入完成";
                            progressDialog.Close();
                        });

                        patientInformationViewModel.RegistrationSetting();
                        patientInformationViewModel.ComboBoxItemInfo = patientInformationViewModel.RegistrationsList.Where(w => w.SelectedValue.Equals(registrations.Registration_ID)).First();
                        GC.Collect();

                    }, CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
                }
                else
                {
                    MessageBox.Show("影像路徑有問題，請至<設定>檢查是否有誤", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            btnImport.Dispatcher.Invoke(() =>
            {
                btnImport.IsEnabled = true;
            });
        }

        bool isStop = false; //接ProcessingDialog 回傳值 停止
        private void ToggleButton_AutoImport_Click(object sender, RoutedEventArgs e)
        {
            if (btnAutoImport.IsChecked == true)
            {
                if (PathCheck.IsPathExist(Agencys.Agency_ImagePath) && PathCheck.IsPathExist(Agencys.Agency_WifiCardPath))
                {
                    //讀寫Registrations
                    //確認掛號資料
                    DateTime RegistrationDate = patientInformationViewModel.ImportDate;

                    ProgressDialogIndeterminate progressDialogIndeterminate = new ProgressDialogIndeterminate();

                    progressDialogIndeterminate.Dispatcher.Invoke(() =>
                    {
                        progressDialogIndeterminate.PText = "準備匯入至(" + RegistrationDate.ToString("yyyy/MM/dd") + ")，圖片偵測中";
                        progressDialogIndeterminate.PIsIndeterminate = true;
                        progressDialogIndeterminate.ButtonContentVisibility = Visibility.Hidden;
                        progressDialogIndeterminate.ReturnValueCallback += new ProgressDialogIndeterminate.ReturnValueDelegate(this.SetReturnValueCallbackFun);

                        progressDialogIndeterminate.Show();
                    });

                    Registrations registrations = new Registrations();
                    //匯入的圖片數
                    int imageCount = 0;

                    Task task = Task.Factory.StartNew(() =>
                    {
                        using (var ide = new iDentalEntities())
                        {
                            while (true)
                            {
                                //偵測資料夾
                                foreach (string f in Directory.GetFiles(Agencys.Agency_WifiCardPath))
                                {
                                    Thread.Sleep(1000);

                                    //設定病患資料夾
                                    PatientImageFolderInfo patientImageFolderInfo = PatientFolderSetting.PatientImageFolderSetting(Agencys, Patients, RegistrationDate);
                                    //檢查是否存在，不存在就新增
                                    PathCheck.CheckPathAndCreate(patientImageFolderInfo.PatientImageFullPath);

                                    string extension = Path.GetExtension(f).ToUpper();
                                    string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                                    string imageFileName = newFileName + extension;

                                    //判斷圖片是否為正
                                    //不是，旋轉後儲存
                                    //是，直接File.Copy
                                    ImageHelper.RotateImageByExifOrientationData(f, patientImageFolderInfo.PatientImageFullPath + @"\" + newFileName + extension, extension, true);

                                    var queryRegistrations = from r in ide.Registrations
                                                             where r.Patient_ID == Patients.Patient_ID && r.Registration_Date == RegistrationDate.Date
                                                             select r;
                                    if (queryRegistrations.Count() > 0)
                                    {
                                        registrations = queryRegistrations.First();
                                    }
                                    else
                                    {
                                        registrations.Patient_ID = Patients.Patient_ID;
                                        registrations.Registration_Date = RegistrationDate.Date;
                                        ide.Registrations.Add(registrations);
                                        //寫入Registrations
                                        ide.SaveChanges();
                                    }

                                    int deleteTime = 0;
                                    while(deleteTime < 5)
                                    {
                                        Thread.Sleep(3000);
                                        try
                                        {
                                            File.Delete(f);
                                            deleteTime = 5;
                                        }
                                        catch (Exception ex)
                                        {
                                            deleteTime++;
                                            if (deleteTime == 5)
                                            {
                                                if (MessageBox.Show("影像搬移中出現問題", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                                                {
                                                    isStop = true;
                                                    ErrorLog.ErrorMessageOutput(ex.ToString());
                                                }
                                            }
                                        }
                                    }

                                    Images images = new Images()
                                    {
                                        Image_Path = @"\" + patientImageFolderInfo.PatientImagePath + @"\" + imageFileName,
                                        Image_FileName = imageFileName,
                                        Image_Extension = extension
                                    };
                                    registrations.Images.Add(images);
                                    ide.SaveChanges();

                                    //已匯入
                                    imageCount++;
                                    progressDialogIndeterminate.Dispatcher.Invoke(() =>
                                    {
                                        progressDialogIndeterminate.PText = "圖片匯入中，已匯入" + imageCount + "張";
                                    });

                                    Thread.Sleep(200);
                                }
                                //按停止
                                if (isStop)
                                {
                                    isStop = false;
                                    return;
                                }
                            }
                        }
                    }).ContinueWith(cw =>
                    {
                        progressDialogIndeterminate.Dispatcher.Invoke(()=>
                        {
                            progressDialogIndeterminate.PText = "處理完畢";
                            progressDialogIndeterminate.Close();
                        });

                        btnAutoImport.IsChecked = false;

                        if (imageCount > 0)
                        {
                            patientInformationViewModel.RegistrationSetting();
                            patientInformationViewModel.ComboBoxItemInfo = patientInformationViewModel.RegistrationsList.Where(w => w.SelectedValue.Equals(registrations.Registration_ID)).First();
                        }

                        GC.Collect();

                    }, CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
                }
                else
                {
                    MessageBox.Show("影像路徑或Wifi Card路徑有問題，請至<設定>檢查是否有誤", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        private void SetReturnValueCallbackFun(bool isDetecting)
        {
            if (isDetecting)
            {
                isStop = isDetecting;
            }
        }
        #endregion
    }
}
