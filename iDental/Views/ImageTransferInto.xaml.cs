using iDental.Class;
using iDental.iDentalClass;
using iDental.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;

namespace iDental.Views
{
    /// <summary>
    /// ImageTransferInto.xaml 的互動邏輯
    /// </summary>
    public partial class ImageTransferInto : Window
    {
        private ImageTransferIntoViewModel imageTransferIntoViewModel;
        public ImageTransferInto(Agencys agencys, Patients patients, ObservableCollection<ImageInfo> displayImageInfoList)
        {
            InitializeComponent();

            imageTransferIntoViewModel = new ImageTransferIntoViewModel(agencys, patients, displayImageInfoList);

            DataContext = imageTransferIntoViewModel;
        }

        private void Button_PatientSearch_Click(object sender, RoutedEventArgs e)
        {
            PatientSearch patientSearch = new PatientSearch();
            if (patientSearch.ShowDialog() == true)
            {
                imageTransferIntoViewModel.TargetPatients = patientSearch.Patients;
            }
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            if (imageTransferIntoViewModel.CanSave())
            {
                try
                {
                    if (imageTransferIntoViewModel.Patients.Patient_ID.Equals(imageTransferIntoViewModel.TargetPatients.Patient_ID))
                    {
                        ////轉至自己
                        TransferImages();
                    }
                    else
                    {
                        //轉至他人
                        if (MessageBox.Show("將影像轉至他人會使樣板模式的照片移除，是否繼續", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                        {
                            TransferImages();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.ErrorMessageOutput(ex.ToString());
                    MessageBox.Show("轉換過程出現問題，請聯絡工程人員", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    DialogResult = false;
                }
            }
            else
            {
                MessageBox.Show("請確認轉至的病患與掛號日是否填寫", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void TransferImages()
        {
            DateTime RegistrationDate = DateTime.Parse(imageTransferIntoViewModel.TransRegistrationDate);
            //設定病患資料夾
            PatientImageFolderInfo patientImageFolderInfo = PatientFolderSetting.PatientImageFolderSetting(imageTransferIntoViewModel.Agencys, imageTransferIntoViewModel.TargetPatients.Patient_ID, RegistrationDate);
            //檢查是否存在，不存在就新增
            PathCheck.CheckPathAndCreate(patientImageFolderInfo.PatientImageFullPath);

            ProgressDialog progressDialog = new ProgressDialog();

            progressDialog.Dispatcher.Invoke(() =>
            {
                progressDialog.PMinimum = 0;
                progressDialog.PValue = 0;
                progressDialog.PMaximum = imageTransferIntoViewModel.DisplayImageInfoListCount;
                progressDialog.PText = "圖片匯入中，請稍後( 0" + " / " + progressDialog.PMaximum + " )";
                progressDialog.Show();
            });

            Registrations registrations = new Registrations();

            Task t = Task.Factory.StartNew(() =>
            {
                using (var ide = new iDentalEntities())
                {
                    var queryRegistrations = from r in ide.Registrations
                                             where r.Patient_ID == imageTransferIntoViewModel.TargetPatients.Patient_ID && r.Registration_Date == RegistrationDate.Date
                                             select r;
                    if (queryRegistrations.Count() > 0)
                    {
                        registrations = queryRegistrations.First();
                    }
                    else
                    {
                        registrations.Patient_ID = imageTransferIntoViewModel.TargetPatients.Patient_ID;
                        registrations.Registration_Date = RegistrationDate.Date;
                        ide.Registrations.Add(registrations);
                        //寫入Registrations
                        ide.SaveChanges();
                    }

                    foreach (ImageInfo ii in imageTransferIntoViewModel.DisplayImageInfoList)
                    {
                        string extension = Path.GetExtension(ii.Image_FullPath).ToUpper();
                        string imageFileName = Path.GetFileName(ii.Image_FullPath);
                        //路徑不同再轉入
                        if (!ii.Image_FullPath.Equals(patientImageFolderInfo.PatientImageFullPath + @"\" + imageFileName))
                        {
                            File.Move(ii.Image_FullPath, patientImageFolderInfo.PatientImageFullPath + @"\" + imageFileName);

                            Images queryImage = (from i in ide.Images
                                                 where i.Image_ID.Equals(ii.Image_ID)
                                                 select i).First();
                            queryImage.Image_Path = patientImageFolderInfo.PatientImagePath + @"\" + imageFileName;
                            queryImage.Image_FileName = imageFileName;
                            queryImage.Image_Extension = extension;
                            queryImage.Registration_ID = registrations.Registration_ID;
                            ide.SaveChanges();
                        }
                        progressDialog.Dispatcher.Invoke(() =>
                        {
                            progressDialog.PValue++;
                            progressDialog.PText = "圖片匯入中，請稍後( " + progressDialog.PValue + " / " + progressDialog.PMaximum + " )";
                        });

                        Thread.Sleep(200);
                    }

                    //判斷那些掛號日底下已經沒有圖片了
                    var queryNoImageInRegistrations = from r in ide.Registrations
                                                      where r.Patient_ID.Equals(imageTransferIntoViewModel.Patients.Patient_ID) &&
                                                      r.Images.Count.Equals(0)
                                                      select r;
                    ide.Registrations.RemoveRange(queryNoImageInRegistrations);

                    if (!imageTransferIntoViewModel.Patients.Patient_ID.Equals(imageTransferIntoViewModel.TargetPatients.Patient_ID))
                    {
                        var queryImageID = from ti in imageTransferIntoViewModel.DisplayImageInfoList
                                           where ti.IsSelected == true
                                           select ti.Image_ID;
                        var deleteItem = from ti in ide.Templates_Images
                                         where queryImageID.Contains((int)ti.Image_ID)
                                         && ti.Patient_ID.Equals(imageTransferIntoViewModel.Patients.Patient_ID)
                                         select ti;
                        ide.Templates_Images.RemoveRange(deleteItem);
                    }
                    ide.SaveChanges();
                }
            }).ContinueWith(cw =>
            {
                progressDialog.Dispatcher.Invoke(() =>
                {
                    progressDialog.PText = "匯入完成";
                    progressDialog.Close();
                });

                MessageBox.Show("轉換成功", "提示", MessageBoxButton.OK);

                GC.Collect();

                DialogResult = true;
                Close();

            }, CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
