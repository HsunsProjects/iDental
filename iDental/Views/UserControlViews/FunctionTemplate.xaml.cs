using iDental.Class;
using iDental.DatabaseAccess.QueryEntities;
using iDental.iDentalClass;
using iDental.ViewModels.UserControlViewModels;
using iDental.ViewModels.ViewModelBase;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Threading;
using System.Linq;
using System.Windows.Media.Imaging;

namespace iDental.Views.UserControlViews
{
    /// <summary>
    /// FunctionTemplate.xaml 的互動邏輯
    /// </summary>
    public partial class FunctionTemplate : UserControl
    {
        private Agencys Agencys
        {
            get { return functionTemplateViewModel.Agencys; }
            set { functionTemplateViewModel.Agencys = value; }
        }

        private Patients Patients
        {
            get { return functionTemplateViewModel.Patients; }
            set { functionTemplateViewModel.Patients = value; }
        }

        public MTObservableCollection<ImageInfo> DisplayImageInfo
        {
            get { return functionTemplateViewModel.DisplayImageInfo; }
            set { functionTemplateViewModel.DisplayImageInfo = value; }
        }

        public UserControl TemplateContent
        {
            get { return functionTemplateViewModel.TemplateContent; }
        }

        public Templates SelectedTemplate
        {
            get { return functionTemplateViewModel.SelectedTemplate; }
        }

        private FunctionTemplateViewModel functionTemplateViewModel;
        public FunctionTemplate(Agencys agencys, Patients patients, MTObservableCollection<ImageInfo> displayImageInfo)
        {
            InitializeComponent();

            functionTemplateViewModel = new FunctionTemplateViewModel(agencys, patients);

            DataContext = functionTemplateViewModel;

            DisplayImageInfo = displayImageInfo;
        }
        
        private void Button_Stretch_Click(object sender, RoutedEventArgs e)
        {
            switch (Agencys.Agency_ViewType)
            {
                case "0":
                    GridLength dataGridWidth = functionTemplateViewModel.StretchWidth;
                    if (dataGridWidth.Value.Equals(0))
                    {
                        functionTemplateViewModel.StretchWidth = new GridLength(270, GridUnitType.Pixel);
                        ButtonStretch.Content = ">";
                    }
                    else
                    {
                        functionTemplateViewModel.StretchWidth = new GridLength(0, GridUnitType.Pixel);
                        ButtonStretch.Content = "<";
                    }
                    break;
                case "1":
                    GridLength dataGridHeight = functionTemplateViewModel.StretchHeight;
                    if (dataGridHeight.Value.Equals(0))
                    {
                        functionTemplateViewModel.StretchHeight = new GridLength(205, GridUnitType.Pixel);
                        ButtonStretch.Content = "﹀";
                    }
                    else
                    {
                        functionTemplateViewModel.StretchHeight = new GridLength(0, GridUnitType.Pixel);
                        ButtonStretch.Content = "︿";
                    }
                    break;
            }
        }

        private DateTime RegistrationDate = DateTime.Now;

        // 委派回傳 MainWindows
        // Wifi Auto 匯入後更新
        public delegate void ReturnValueDelegate(DateTime Registration_Date);
        public event ReturnValueDelegate ReturnValueCallback;
        
        bool isStop = false; //ProgressDialogIndeterminate 回傳值 停止
        bool isSkip = true; //ProgressDialogIndeterminate 回傳值 略過

        private void Button_AutoImport_Click(object sender, RoutedEventArgs e)
        {
            if (btnAutoImport.IsChecked == true)
            {
                if (PathCheck.IsPathExist(Agencys.Agency_ImagePath) && PathCheck.IsPathExist(Agencys.Agency_WifiCardPath))
                {
                    bool isEverChanged = false;

                    ProgressDialogIndeterminate progressDialogIndeterminate = new ProgressDialogIndeterminate();

                    progressDialogIndeterminate.Dispatcher.Invoke(() =>
                    {
                        progressDialogIndeterminate.PText = "圖片偵測中";
                        progressDialogIndeterminate.PIsIndeterminate = true;
                        progressDialogIndeterminate.ButtonContent = "跳過";
                        progressDialogIndeterminate.ReturnValueCallback += new ProgressDialogIndeterminate.ReturnValueDelegate(SetReturnValueCallbackFun);
                        progressDialogIndeterminate.Show();
                    });

                    Registrations registrations = new Registrations();

                    Task t = Task.Factory.StartNew(() =>
                    {
                        using (var ide = new iDentalEntities())
                        {
                            CreateBitmapImage createBitmapImage = new CreateBitmapImage();

                            TableTemplates_Images tableTemplates_Images = new TableTemplates_Images();

                            ObservableCollection<Templates_Images> Templates_ImagesCollect = tableTemplates_Images.QueryTemplatesImagesImportDateAndReturnFullImagePath(Agencys, Patients, SelectedTemplate, functionTemplateViewModel.SelectedDate);

                            //default Image[i] in UserControl Templates
                            int Imagei = 0;
                            //載入樣板設定
                            int ImageCount = (int)SelectedTemplate.Template_ImageCount;
                            int DecodePixelWidth = (int)SelectedTemplate.Template_DecodePixelWidth;
                            while (Imagei < ImageCount)
                            {
                                progressDialogIndeterminate.Dispatcher.Invoke(() =>
                                {
                                    progressDialogIndeterminate.PText = "圖片 " + (Imagei + 1) + " 偵測中";
                                });

                                //目前處理的Image[i]
                                Image iTarget;

                                TemplateContent.Dispatcher.Invoke(() =>
                                {
                                    iTarget = new Image();
                                    iTarget = (Image)TemplateContent.FindName("Image" + Imagei);

                                    //更換目標圖
                                    //iTarget.Source = new BitmapImage(new Uri(@"/DigiDental;component/Resource/yes.png", UriKind.RelativeOrAbsolute));
                                });

                                //set the paramater default
                                bool isChanged = false;
                                bool detecting = true;
                                while (true)
                                {
                                    //開始偵測wifi card路徑
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

                                        Images images = new Images()
                                        {
                                            Image_Path = @"\" + patientImageFolderInfo.PatientImagePath + @"\" + imageFileName,
                                            Image_FileName = imageFileName,
                                            Image_Extension = extension
                                        };

                                        registrations.Images.Add(images);
                                        ide.SaveChanges();
                                        
                                        //複製完如果刪除發生問題
                                        //嘗試五次每次間隔3秒
                                        int deleteTime = 0;
                                        while (deleteTime < 5)
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

                                        TemplateContent.Dispatcher.Invoke(() =>
                                        {
                                            iTarget = new Image();
                                            iTarget = (Image)TemplateContent.FindName("Image" + Imagei);

                                            //INSERT TemplateImages
                                            tableTemplates_Images.InsertOrUpdateTemplatesImages(Patients, functionTemplateViewModel.SelectedTemplate, functionTemplateViewModel.SelectedDate, images.Image_ID, images.Image_Path, iTarget.Uid);
                                            iTarget.Source = createBitmapImage.SettingBitmapImage(patientImageFolderInfo.PatientImageFullPath + @"\" + imageFileName, DecodePixelWidth);
                                            isChanged = true;
                                        });

                                        isEverChanged = true;
                                        //代表以處理完結束這回合的偵測
                                        detecting = false;
                                    }
                                    //ProcessingDialog STOP
                                    //停止
                                    if (isStop)
                                    {
                                        isStop = false;
                                        TemplateContent.Dispatcher.Invoke(() =>
                                        {
                                            iTarget = new Image();
                                            iTarget = (Image)TemplateContent.FindName("Image" + Imagei);
                                            var findOriImage = from tc in Templates_ImagesCollect
                                                               where tc.Template_Image_Number == Imagei.ToString()
                                                               select tc;
                                            if (findOriImage.Count() > 0)
                                            {
                                                iTarget.Source = createBitmapImage.SettingBitmapImage(findOriImage.First().Image_Path, DecodePixelWidth);
                                            }
                                            else
                                            {
                                                //iTarget.Source = new BitmapImage(new Uri(@"/DigiDental;component/Resource/no.png", UriKind.RelativeOrAbsolute));
                                            }
                                        });
                                        return;
                                    }
                                    else
                                    {
                                        // import pic OR skip import (NEXT)
                                        // 匯入之後接下一張，或是按了跳過
                                        if (!detecting || !isSkip)
                                        {
                                            if (!isChanged)
                                            {
                                                TemplateContent.Dispatcher.Invoke(() =>
                                                {
                                                    iTarget = new Image();
                                                    iTarget = (Image)TemplateContent.FindName("Image" + Imagei);
                                                    var findOriImage = from tc in Templates_ImagesCollect
                                                                       where tc.Template_Image_Number == Imagei.ToString()
                                                                       select tc;
                                                    if (findOriImage.Count() > 0)
                                                    {
                                                        iTarget.Source = createBitmapImage.SettingBitmapImage(findOriImage.First().Image_Path, DecodePixelWidth);
                                                    }
                                                    else
                                                    {
                                                        iTarget.Source = new BitmapImage(new Uri(@"/DigiDental;component/Resource/no.png", UriKind.RelativeOrAbsolute));
                                                    }
                                                });
                                            }
                                            Imagei++;
                                            isSkip = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }).ContinueWith(cw =>
                    {
                        //結束
                        progressDialogIndeterminate.Dispatcher.Invoke(() =>
                        {
                            progressDialogIndeterminate.PText = "處理完畢";
                            progressDialogIndeterminate.Close();
                        });

                        //委派回傳MainWindow
                        //刷新Registrations 資料
                        //刷新Images 資料
                        if (isEverChanged)
                        {
                            ReturnValueCallback(registrations.Registration_Date);
                        }

                        GC.Collect();

                        btnAutoImport.IsChecked = false;

                    }, TaskScheduler.FromCurrentSynchronizationContext());
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
            else
            {
                isSkip = isDetecting;
            }
        }

        private void Button_ExportTemplateImage_Click(object sender, RoutedEventArgs e)
        {
            if (functionTemplateViewModel.SelectedTemplate != null)
            {
                SaveFileDialog sfd = new SaveFileDialog()
                {
                    FileName = Patients.Patient_ID + "-" + Patients.Patient_Name + "-" + functionTemplateViewModel.SelectedDate.ToString("yyyyMMdd") + functionTemplateViewModel.SelectedTemplate.Template_Title,
                    DefaultExt = ".png",
                    Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif"
                };
                if (sfd.ShowDialog() == true)
                {
                    //匯出圖片
                    UIElementExport.UIElementExportImage(TemplateContent, sfd.FileName);

                    MessageBox.Show("檔案建立成功，存放位置於" + sfd.FileName, "提示", MessageBoxButton.OK);
                }
            }
            else
            {
                MessageBox.Show("尚未選擇想要匯出的樣板", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Button_ExportPPT_Click(object sender, RoutedEventArgs e)
        {
            if (functionTemplateViewModel.SelectedTemplate != null)
            {
                SaveFileDialog sfd = new SaveFileDialog()
                {
                    FileName = Patients.Patient_ID + "-" + Patients.Patient_Name + "-" + functionTemplateViewModel.SelectedDate.ToString("yyyyMMdd") + functionTemplateViewModel.SelectedTemplate.Template_Title,
                    DefaultExt = ".pptx",
                    Filter = "PowerPoint 簡報 (*.pptx)|*.pptx"
                };
                if (sfd.ShowDialog() == true)
                {
                    ObservableCollection<Templates_Images> observableCollection = new TableTemplates_Images().QueryTemplatesImagesImportDateAndReturnFullImagePath(Agencys, Patients, functionTemplateViewModel.SelectedTemplate, functionTemplateViewModel.SelectedDate);
                    if (new PPTPresentation().CreatePPTExport(observableCollection, sfd.FileName, functionTemplateViewModel.SelectedTemplate.Template_Title))
                    {
                        MessageBox.Show("檔案建立成功，存放位置於" + sfd.FileName, "提示", MessageBoxButton.OK);
                    }
                    else
                    {
                        MessageBox.Show("檔案匯出發生問題", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                GC.Collect();
            }
            else
            {
                MessageBox.Show("尚未選擇想要匯出的樣板", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Image_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                ImageInfo dragImage = (ImageInfo)((Image)e.Source).DataContext;
                DataObject data = new DataObject(DataFormats.Text, dragImage);

                DragDrop.DoDragDrop((DependencyObject)e.Source, data, DragDropEffects.Copy);
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorMessageOutput(ex.ToString());
                MessageBox.Show("移動圖片發生錯誤，聯絡資訊人員", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
