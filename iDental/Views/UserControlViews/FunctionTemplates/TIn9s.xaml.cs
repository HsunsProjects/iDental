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
    /// TIn9s.xaml 的互動邏輯
    /// </summary>
    public partial class TIn9s : UserControl
    {
        private Patients Patients { get; set; }
        private Templates Templates { get; set; }
        private DateTime TemplateImportDate { get; set; }
        private int TemplateImagePixelWidth { get; set; }

        private TableTemplates_Images tableTemplates_Images;
        public TIn9s(Agencys agencys, Patients patients, Templates templates, DateTime templateImportDate)
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

                //取得圖片位置代號寫入圖片用
                string Template_Image_Number = img.Name.Replace("Image", "");
                string ImageUID = string.Empty;
                //寫入資料庫再帶回畫面
                ImageUID = tableTemplates_Images.InsertOrUpdateTemplatesImages(Patients, Templates, TemplateImportDate, dragImage.Image_ID, dragImage.Image_Path, Template_Image_Number);
                img.Uid = ImageUID;
                img.Source = new CreateBitmapImage().BitmapImageShow(dragImage.Image_FullPath, TemplateImagePixelWidth);
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorMessageOutput(ex.ToString());
                MessageBox.Show("移動圖片發生錯誤，聯絡資訊人員", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
