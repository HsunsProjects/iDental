using AForge.Video.DirectShow;
using iDental.Class;
using iDental.DatabaseAccess.QueryEntities;
using iDental.iDentalClass;
using iDental.ViewModels.UserControlViewModels;
using Microsoft.Win32;
using Saraff.Twain;
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

        private Twain32 twain32;

        private PatientInformationViewModel patientInformationViewModel;

        public PatientInformation(Agencys agencys, Patients patients)
        {
            InitializeComponent();

            Agencys = agencys;
            Patients = patients;

            twain32 = new Twain32();
            twain32.AcquireCompleted += twain_AcquireCompleted;
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
        
        private void Button_PatientEdit_Click(object sender, RoutedEventArgs e)
        {
            PatientSetting patientSetting = new PatientSetting("EDIT", Agencys, Patients);
            if (patientSetting.ShowDialog() == true)
            {
                //更新畫面(ViewModel) 和.cs的Patients
                Patients = patientSetting.Patients;
                patientInformationViewModel.Patients = Patients;
            }
        }

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

        private void Button_WebcamImport_Click(object sender, RoutedEventArgs e)
        {
            if (PathCheck.IsPathExist(Agencys.Agency_ImagePath))
            {
                FilterInfoCollection filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                if (filterInfoCollection.Count > 0)
                {
                    DateTime RegistrationDate = patientInformationViewModel.ImportDate;

                    Webcam webcam = new Webcam(filterInfoCollection, Agencys, Patients, RegistrationDate);

                    if (webcam.ShowDialog() == true)
                    {
                        Registrations registrations = webcam.registrations;
                        ReloadOrAddImage(registrations, RegistrationDate);
                    }
                }
                else
                {
                    MessageBox.Show("尚未偵測到影像裝置", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("影像路徑有問題，請至<設定>檢查是否有誤", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnTwainImport_Click(object sender, RoutedEventArgs e)
        {
            if (PathCheck.IsPathExist(Agencys.Agency_ImagePath))
            {
                //判斷有沒有設定裝置
                //有 : 掃描
                //無 : 跳視窗選擇 (Twain)
                string TwainDevice = ConfigManage.ReadAppConfig("TwainDevice");

                twain32.Country = TwCountry.TAIWAN;
                twain32.Language = TwLanguage.CHINESE_TAIWAN;
                twain32.IsTwain2Enable = true;

                twain32.OpenDSM();
                if (twain32.IsTwain2Supported)
                {
                    if (twain32.SourcesCount > 0)
                    {
                        twain32.CloseDataSource();
                        Twain32.Identity identity;
                        bool IsDefault = false;
                        if (!string.IsNullOrEmpty(TwainDevice))
                        {
                            for (var i = 0; i < twain32.SourcesCount; i++)
                            {
                                if (TwainDevice.Equals(twain32.GetSourceIdentity(i).Name))
                                {
                                    identity = twain32.GetSourceIdentity(i);
                                    twain32.SetDefaultSource(i);
                                    IsDefault = true;
                                }
                            }
                        }
                        if (!IsDefault)
                        {
                            if (twain32.SelectSource() == true)
                            {
                                ErrorLog.ErrorMessageOutput("STARTING SET CONFIG");
                                ConfigManage.AddUpdateAppConfig("TwainDevice", twain32.GetSourceProductName(twain32.GetDefaultSource()));
                                ErrorLog.ErrorMessageOutput("END SET CONFIG");
                                ErrorLog.ErrorMessageOutput("IsDefault = false STARTING Acquire");
                                twain32.Acquire();
                                ErrorLog.ErrorMessageOutput("twain32.Acquire() IsDefault = false LINE 149");
                            }
                        }
                        else
                        {
                            ErrorLog.ErrorMessageOutput("IsDefault = true STARTING Acquire");
                            twain32.Acquire();
                            ErrorLog.ErrorMessageOutput("twain32.Acquire() IsDefault = true LINE 155");
                        }
                    }
                    else
                    {
                        MessageBox.Show("尚未找到掃描裝置(TWAIN)", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                twain32.CloseDSM();
            }
            else
            {
                MessageBox.Show("影像路徑有問題，請至<設定>檢查是否有誤", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void twain_AcquireCompleted(object sender, EventArgs e)
        {
            try
            {
                DateTime RegistrationDate = patientInformationViewModel.ImportDate;
                //設定病患資料夾
                PatientImageFolderInfo patientImageFolderInfo = PatientFolderSetting.PatientImageFolderSetting(Agencys, Patients.Patient_ID, RegistrationDate);
                //檢查是否存在，不存在就新增
                PathCheck.CheckPathAndCreate(patientImageFolderInfo.PatientImageFullPath);

                string imageFileName = string.Empty;
                string extension = string.Empty;
                string imageFullName = string.Empty;

                imageFileName = @"twain" + DateTime.Now.ToString("yyyyMMddHHmmssffff");
                extension = @".jpg";
                imageFullName = imageFileName + extension;
                
                //儲存影像
                twain32.GetImage(0).Save(patientImageFolderInfo.PatientImageFullPath + @"\" + imageFullName);
                //寫入DB
                Registrations registrations = new Registrations();
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

                    Images images = new Images()
                    {
                        Image_Path = patientImageFolderInfo.PatientImagePath + @"\" + imageFullName,
                        Image_FileName = imageFileName,
                        Image_Extension = extension
                    };
                    registrations.Images.Add(images);
                    //Images
                    ide.SaveChanges();
                }
                ReloadOrAddImage(registrations, RegistrationDate);
                ErrorLog.ErrorMessageOutput("twain_AcquireCompleted And Saved LINE 171");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
                twain32.CloseDSM();
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
                    DateTime RegistrationDate = patientInformationViewModel.ImportDate;
                    //設定病患資料夾
                    PatientImageFolderInfo patientImageFolderInfo = PatientFolderSetting.PatientImageFolderSetting(Agencys, Patients.Patient_ID, RegistrationDate);
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
                                    Image_Path = patientImageFolderInfo.PatientImagePath + @"\" + imageFileName,
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

                        ReloadOrAddImage(registrations, RegistrationDate);
                        //patientInformationViewModel.RegistrationSetting();
                        //patientInformationViewModel.ComboBoxItemInfo = patientInformationViewModel.RegistrationsList.Where(w => w.SelectedValue.Equals(registrations.Registration_ID)).First();
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
                    DateTime RegistrationDate = patientInformationViewModel.ImportDate;

                    ProgressDialogIndeterminate progressDialogIndeterminate = new ProgressDialogIndeterminate();

                    progressDialogIndeterminate.Dispatcher.Invoke(() =>
                    {
                        progressDialogIndeterminate.PText = "準備匯入至(" + RegistrationDate.ToString("yyyy/MM/dd") + ")，圖片偵測中";
                        progressDialogIndeterminate.PIsIndeterminate = true;
                        progressDialogIndeterminate.ButtonContentVisibility = Visibility.Hidden;
                        progressDialogIndeterminate.ReturnValueCallback += new ProgressDialogIndeterminate.ReturnValueDelegate(SetReturnValueCallbackFun);

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
                                    PatientImageFolderInfo patientImageFolderInfo = PatientFolderSetting.PatientImageFolderSetting(Agencys, Patients.Patient_ID, RegistrationDate);
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

                                    //複製完如果刪除發生問題
                                    //嘗試五次每次間隔3秒
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
                                        Image_Path = patientImageFolderInfo.PatientImagePath + @"\" + imageFileName,
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
                            ReloadOrAddImage(registrations, RegistrationDate);
                            //patientInformationViewModel.RegistrationSetting();
                            //patientInformationViewModel.ComboBoxItemInfo = patientInformationViewModel.RegistrationsList.Where(w => w.SelectedValue.Equals(registrations.Registration_ID)).First();
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

        /// <summary>
        /// 新增同一天日期的影像，或是新增其他天再重載
        /// </summary>
        /// <param name="registrations">掛號資訊</param>
        /// <param name="RegistrationDate">匯入的日期</param>
        private void ReloadOrAddImage(Registrations registrations, DateTime RegistrationDate)
        {
            //要載入裝置的圖片
            if (patientInformationViewModel.ComboBoxItemInfo == null)
            {
                patientInformationViewModel.UpdateDisplayImageInfo();
            }
            else
            {
                DateTime displayDate;
                if (DateTime.TryParse(patientInformationViewModel.ComboBoxItemInfo.DisplayName, out displayDate))
                {
                    if (displayDate.Date.Equals(RegistrationDate.Date))
                    {
                        patientInformationViewModel.UpdateDisplayImageInfo();
                    }
                    else
                    {
                        patientInformationViewModel.RegistrationSetting();
                        patientInformationViewModel.ComboBoxItemInfo = patientInformationViewModel.RegistrationsList.Where(w => w.SelectedValue.Equals(registrations.Registration_ID)).First();
                    }
                }
                else
                {
                    patientInformationViewModel.RegistrationSetting();
                    patientInformationViewModel.ComboBoxItemInfo = patientInformationViewModel.RegistrationsList.Where(w => w.SelectedValue.Equals(registrations.Registration_ID)).First();
                }
            }
        }

        private void Image_Drop(object sender, DragEventArgs e)
        {
            try
            {
                Image img = e.Source as Image;

                ImageInfo dragImage = new ImageInfo();
                dragImage = ((ImageInfo)e.Data.GetData(DataFormats.Text));

                //病患大頭照路徑
                PatientPhotoFolderInfo patientPhotoFolderInfo = PatientFolderSetting.PatientPhotoFolderSetting(Agencys, Patients.Patient_ID);
                //建立大頭照路徑
                PathCheck.CheckPathAndCreate(patientPhotoFolderInfo.PatientPhotoFullPath);

                string newPatientPhotoFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + dragImage.Image_Extension;
                string newPatientPhotoFile = patientPhotoFolderInfo.PatientPhotoFullPath + @"\" + newPatientPhotoFileName;
                File.Copy(dragImage.Image_FullPath, newPatientPhotoFile);
                Thread.Sleep(500);

                patientInformationViewModel.Patient_Photo = new CreateBitmapImage().BitmapImageShow(newPatientPhotoFile, 400);

                new TablePatients().UpdatePatientPhoto(Patients, patientPhotoFolderInfo.PatientPhotoPath + @"\" + newPatientPhotoFileName);
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorMessageOutput(ex.ToString());
                MessageBox.Show("移動圖片發生錯誤，聯絡資訊人員", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
    }
}
