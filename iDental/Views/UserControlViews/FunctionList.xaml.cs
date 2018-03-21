using iDental.Class;
using iDental.iDentalClass;
using iDental.ViewModels.UserControlViewModels;
using iDental.ViewModels.ViewModelBase;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace iDental.Views.UserControlViews
{
    /// <summary>
    /// FunctionList.xaml 的互動邏輯
    /// </summary>
    public partial class FunctionList : UserControl
    {
        private Agencys Agencys { get; set; }
        private Patients Patients { get; set; }
        public MTObservableCollection<ImageInfo> DisplayImageInfo
        {
            get { return functionListViewModel.DisplayImageInfo; }
            set { functionListViewModel.DisplayImageInfo = value; }
        }

        private FunctionListViewModel functionListViewModel;
        public FunctionList(Agencys agencys, Patients patients, MTObservableCollection<ImageInfo> displayImageInfo)
        {
            InitializeComponent();

            Agencys = agencys;

            Patients = patients;

            functionListViewModel = new FunctionListViewModel();

            DataContext = functionListViewModel;

            DisplayImageInfo = displayImageInfo;
        }

        private void Button_ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            if (functionListViewModel.ColumnCount > 1)
                functionListViewModel.ColumnCount--;
        }

        private void Button_ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (functionListViewModel.ColumnCount < 5)
                functionListViewModel.ColumnCount++;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (ImageInfo item in e.RemovedItems)
            {
                item.IsSelected = false;
            }
            foreach (ImageInfo item in e.AddedItems)
            {
                item.IsSelected = true;
            }
            functionListViewModel.ImageSelectedCount = functionListViewModel.DisplayImageInfo.Where(i => i.IsSelected).Count();

        }

        private void Button_ImageEditor_Click(object sender, RoutedEventArgs e)
        {
            if (functionListViewModel.DisplayImageInfo.Count() > 0)
            {
                ObservableCollection<ImageInfo> DisplayImageInfoList = new ObservableCollection<ImageInfo>(functionListViewModel.DisplayImageInfo.Where(i => i.IsSelected));
                if (DisplayImageInfoList.Count() > 0)
                {
                    DisplayImageInfoList = new ObservableCollection<ImageInfo>(DisplayImageInfoList.OrderBy(o => o.Image_ID).OrderByDescending(o2 => o2.Registration_Date));
                }
                else
                {
                    DisplayImageInfoList = new ObservableCollection<ImageInfo>(functionListViewModel.DisplayImageInfo.OrderBy(o => o.Image_ID).OrderByDescending(o2 => o2.Registration_Date));
                }

                ImageEditor imageEditor = new ImageEditor(DisplayImageInfoList);
                if (imageEditor.ShowDialog() == true)
                {

                    GC.Collect();

                    GC.WaitForPendingFinalizers();

                    GC.Collect();
                }

                lbImages.UnselectAll();
            }
            else
            {
                MessageBox.Show("尚未載入圖片", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Button_ImageExport_Click(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "請選擇圖片匯出的位置";
                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var listIsSelected = DisplayImageInfo.Where(w => w.IsSelected == true);
                    foreach (ImageInfo ii in listIsSelected)
                    {
                        //判斷圖片是否為正
                        //不是，旋轉後儲存
                        //是，直接File.Copy
                        ImageHelper.RotateImageByExifOrientationData(ii.Image_FullPath, folderBrowserDialog.SelectedPath + @"\" + ii.Image_FileName + ii.Image_Extension, ii.Image_Extension, true);
                        Thread.Sleep(200);
                    }
                    MessageBox.Show("匯出完成", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        // 委派回傳 MainWindows
        // 刪圖片完後更新所有匯入的紀錄
        public delegate void ReturnValueDelegate();
        public event ReturnValueDelegate ReturnValueCallback;
        // 委派回傳 MainWindows
        // 更新UserControl資料來源
        public delegate void ReturnRenewDelegate();
        public event ReturnRenewDelegate ReturnRenewCallback;

        private void Button_ImageDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("確定刪除已選定的" + functionListViewModel.ImageSelectedCount + "個項目?", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                using (var ide = new iDentalEntities())
                {
                    try
                    {
                        var selectedItemID = from si in DisplayImageInfo
                                             where si.IsSelected == true
                                             select si.Image_ID;
                        var deleteItem = (from i in ide.Images
                                          where selectedItemID.Contains(i.Image_ID)
                                          select i).ToList();
                        deleteItem.ForEach(i => i.Image_IsEnable = false);
                        ide.SaveChanges();

                        foreach (ImageInfo ii in DisplayImageInfo.ToArray())
                        {
                            if (ii.IsSelected)
                                DisplayImageInfo.Remove(ii);
                        }
                        functionListViewModel.CountImages = DisplayImageInfo.Count();
                        if (DisplayImageInfo.Count() == 0)
                        {
                            ReturnValueCallback();
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.ErrorMessageOutput(ex.ToString());
                        MessageBox.Show("刪除圖片發生錯誤", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
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
                MessageBox.Show("移動圖片發生錯誤，聯絡資訊人員", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_ImageTransferInto_Click(object sender, RoutedEventArgs e)
        {
            if (functionListViewModel.DisplayImageInfo.Count() > 0)
            {
                ObservableCollection<ImageInfo> DisplayImageInfoList = new ObservableCollection<ImageInfo>(functionListViewModel.DisplayImageInfo.Where(i => i.IsSelected));
                if (DisplayImageInfoList.Count() > 0)
                {
                    DisplayImageInfoList = new ObservableCollection<ImageInfo>(DisplayImageInfoList.OrderBy(o => o.Image_ID).OrderByDescending(o2 => o2.Registration_Date));
                }
                else
                {
                    DisplayImageInfoList = new ObservableCollection<ImageInfo>(functionListViewModel.DisplayImageInfo.OrderBy(o => o.Image_ID).OrderByDescending(o2 => o2.Registration_Date));
                }
                ImageTransferInto imageTransferInto = new ImageTransferInto(Agencys, Patients, DisplayImageInfoList);
                if (imageTransferInto.ShowDialog() == true)
                {
                    //要重新刷新FunctionList 頁面的圖片
                    ReturnRenewCallback();
                }
            }
            else
            {
                MessageBox.Show("尚未載入圖片", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
