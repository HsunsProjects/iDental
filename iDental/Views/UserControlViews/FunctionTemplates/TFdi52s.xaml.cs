using iDental.Class;
using iDental.DatabaseAccess.QueryEntities;
using iDental.iDentalClass;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace iDental.Views.UserControlViews.FunctionTemplates
{
    /// <summary>
    /// TFdi52s.xaml 的互動邏輯
    /// </summary>
    public partial class TFdi52s : UserControl
    {
        private Patients Patients { get; set; }
        private Templates Templates { get; set; }
        private DateTime TemplateImportDate { get; set; }
        private int TemplateImagePixelWidth { get; set; }

        private TableTemplates_Images tableTemplates_Images;
        public TFdi52s(Agencys agencys, Patients patients, Templates templates, DateTime templateImportDate)
        {
            InitializeComponent();

            Patients = patients;

            Templates = templates;

            TemplateImportDate = templateImportDate;

            TemplateImagePixelWidth = (int)templates.Template_DecodePixelWidth;

            tableTemplates_Images = new TableTemplates_Images();

            ObservableCollection<Templates_Images> observableCollection = tableTemplates_Images.QueryTemplatesImagesImportDateAndReturnFullImagePath(agencys, patients, templates, templateImportDate);
            new LoadTemplates_Images().LoadAllTemplatesImages(observableCollection, MainGrid, TemplateImagePixelWidth);
        }

        private void Image_Drop(object sender, DragEventArgs e)
        {
            try
            {
                Image img = e.Source as Image;
                ImageInfo dragImage = new ImageInfo();

                dragImage = ((ImageInfo)e.Data.GetData(DataFormats.Text));

                img.Source = new CreateBitmapImage().SettingBitmapImage(dragImage.Image_FullPath, TemplateImagePixelWidth);

                tableTemplates_Images.InsertOrUpdateTemplatesImages(Patients, Templates, TemplateImportDate, dragImage.Image_ID, dragImage.Image_Path, img.Uid);
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorMessageOutput(ex.ToString());
                MessageBox.Show("移動圖片發生錯誤，聯絡資訊人員", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
