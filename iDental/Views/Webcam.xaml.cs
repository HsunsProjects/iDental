using AForge.Video;
using AForge.Video.DirectShow;
using iDental.Class;
using iDental.iDentalClass;
using iDental.ViewModels;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;

namespace iDental.Views
{
    /// <summary>
    /// Webcam.xaml 的互動邏輯
    /// </summary>
    public partial class Webcam : Window
    {
        private WebcamViewModel webcamViewModel;

        private Agencys agencys;

        private Patients patients;

        private DateTime registrationDate;

        private PatientImageFolderInfo patientImageFolderInfo;

        ////Device Source 變數(改用Save Image.Source)
        //private Bitmap bitmap;

        private IVideoSource videoSource;

        private bool dialogResul = false;

        public Registrations registrations;

        public Webcam(FilterInfoCollection filterInfoCollection, Agencys agencys, Patients patients, DateTime registrationDate)
        {
            InitializeComponent();

            this.agencys = agencys;

            this.patients = patients;

            this.registrationDate = registrationDate;

            webcamViewModel = new WebcamViewModel(agencys, patients, registrationDate);

            DataContext = webcamViewModel;

            txtRegistratonDate.Text = "匯入日期:" + registrationDate.ToString("yyyy/MM/dd");

            patientImageFolderInfo = PatientFolderSetting.PatientImageFolderSetting(agencys, patients.Patient_ID, registrationDate);

            //檢查是否存在，不存在就新增
            PathCheck.CheckPathAndCreate(patientImageFolderInfo.PatientImageFullPath);

            GetVideoDevices(filterInfoCollection);
        }

        private void GetVideoDevices(FilterInfoCollection filterInfoCollection)
        {
            if (filterInfoCollection.Count > 0)
            {
                foreach (FilterInfo filterInfo in filterInfoCollection)
                {
                    cbDevices.Items.Add(filterInfo);
                }
                cbDevices.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("提示", "尚未取得影像裝置", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                BitmapImage bi;

                //bitmap = (Bitmap)eventArgs.Frame.Clone();

                //bi = bitmap.ToBitmapImage();

                using (var bitmap = (Bitmap)eventArgs.Frame.Clone())
                {
                    bi = bitmap.ToBitmapImage();
                }

                bi.Freeze(); // avoid cross thread operations and prevents leaks
                Dispatcher.BeginInvoke(new ThreadStart(delegate { iVideoPlayer.Source = bi; }));
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorMessageOutput(ex.ToString());
                MessageBox.Show("裝置取得影像發生錯誤", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
                StopCamera();
            }
        }

        private void StartCamera()
        {
            if (((FilterInfo)cbDevices.SelectedItem) != null)
            {
                videoSource = new VideoCaptureDevice(((FilterInfo)cbDevices.SelectedItem).MonikerString);
                videoSource.NewFrame += video_NewFrame;
                videoSource.Start();
            }
        }

        private void StopCamera()
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
                videoSource.NewFrame -= new NewFrameEventHandler(video_NewFrame);
            }
        }

        private void cbDevices_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            StopCamera();
            StartCamera();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StopCamera();
            DialogResult = dialogResul;
        }

        private void Button_Capture_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string imageFileName = string.Empty;
                string extension = string.Empty;
                string imageFullName = string.Empty;

                extension = @".jpg";
                imageFileName = @"device_" + DateTime.Now.ToString("yyyyMMddHHmmssffff");
                imageFullName = imageFileName + extension;

                var encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)iVideoPlayer.Source));
                using (FileStream stream = new FileStream(patientImageFolderInfo.PatientImageFullPath + @"\" + imageFullName, FileMode.Create))
                {
                    encoder.Save(stream);
                }

                //Bitmap current = (Bitmap)bitmap.Clone();
                //bitmap.Save(patientImageFolderInfo.PatientImageFullPath + @"\" + imageFullName);
                //bitmap.Dispose();

                //寫入db
                using (var ide = new iDentalEntities())
                {
                    registrations = new Registrations();
                    var queryRegistrations = from r in ide.Registrations
                                             where r.Patient_ID == patients.Patient_ID && r.Registration_Date == registrationDate.Date
                                             select r;
                    if (queryRegistrations.Count() > 0)
                    {
                        registrations = queryRegistrations.First();
                    }
                    else
                    {
                        registrations.Patient_ID = patients.Patient_ID;
                        registrations.Registration_Date = registrationDate.Date;
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
                webcamViewModel.RefreshDisplayImageInfoList();
                webcamViewModel.SelectedItem = webcamViewModel.DisplayImageInfoList.Last();

                dialogResul = true;
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorMessageOutput(ex.ToString());
                MessageBox.Show("裝置擷取畫面出現問題，請洽資訊廠商", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
