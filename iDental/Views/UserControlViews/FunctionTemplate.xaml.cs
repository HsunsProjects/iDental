using iDental.Class;
using iDental.DatabaseAccess.QueryEntities;
using iDental.iDentalClass;
using iDental.ViewModels.UserControlViewModels;
using iDental.ViewModels.ViewModelBase;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

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
                    UIElementExport.UIElementExportImage(functionTemplateViewModel.TemplateContent, sfd.FileName);

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
