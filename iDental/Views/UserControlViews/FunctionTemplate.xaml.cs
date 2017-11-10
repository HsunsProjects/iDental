using iDental.Class;
using iDental.iDentalClass;
using iDental.ViewModels.UserControlViewModels;
using iDental.ViewModels.ViewModelBase;
using System;
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

        /// <summary>
        /// 紀錄是否展開
        /// </summary>
        private bool IsStretch = true;
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
