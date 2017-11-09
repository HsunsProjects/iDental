using iDental.Class;
using iDental.iDentalClass;
using iDental.ViewModels.UserControlViewModels;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace iDental.Views.UserControlViews
{
    /// <summary>
    /// ImageEditorBase.xaml 的互動邏輯
    /// </summary>
    public partial class ImageEditorBase : UserControl
    {
        private ObservableCollection<ImageInfo> ImagesCollection { get; set; }

        private ImageInfo ImageInfo
        {
            get { return imageEditorBaseViewModel.ImageInfo; }
            set { imageEditorBaseViewModel.ImageInfo = value; }
        }

        private BitmapSource BitmapSource
        {
            get { return imageEditorBaseViewModel.BitmapSource; }
            set { imageEditorBaseViewModel.BitmapSource = value; }
        }

        /// <summary>
        /// 滑鼠滾輪的值
        /// </summary>
        private double scaleValue = 1;

        private ImageEditorBaseViewModel imageEditorBaseViewModel;

        public ImageEditorBase(ObservableCollection<ImageInfo> imagesCollection, ImageInfo imageInfo)
        {
            InitializeComponent();

            ImagesCollection = imagesCollection;

            imageEditorBaseViewModel = new ImageEditorBaseViewModel(imageInfo);

            DataContext = imageEditorBaseViewModel;
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //儲存新影像
                ImageHelper.SaveUsingEncoder(BitmapSource, ImageInfo.Image_FullPath, ImageInfo.Image_Extension);
                ImageInfo.BitmapImage = new CreateBitmapImage().SettingBitmapImage(ImageInfo.Image_FullPath, 800);
                MessageBox.Show("儲存成功", "提示", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorMessageOutput(ex.ToString());
                MessageBox.Show("寫入失敗", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_SaveAs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.DefaultExt = ".png";
                sfd.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
                if (sfd.ShowDialog() == true)
                {
                    //儲存的副檔名
                    string extension = sfd.SafeFileName.Substring(sfd.SafeFileName.IndexOf('.'), (sfd.SafeFileName.Length - sfd.SafeFileName.IndexOf('.')));
                    //另存新影像
                    ImageHelper.SaveUsingEncoder(BitmapSource, sfd.FileName, extension);
                    MessageBox.Show("檔案建立成功，存放位置於" + sfd.FileName, "提示", MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorMessageOutput(ex.ToString());
                MessageBox.Show("寫入失敗", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_RotateLeft_Click(object sender, RoutedEventArgs e)
        {
            BitmapSource = new TransformedBitmap(BitmapSource, new RotateTransform(-90));
            GC.Collect();
        }
        private void Button_RotateRight_Click(object sender, RoutedEventArgs e)
        {
            BitmapSource = new TransformedBitmap(BitmapSource, new RotateTransform(90));
            GC.Collect();
        }

        private void Button_MirrorHorizontal_Click(object sender, RoutedEventArgs e)
        {
            BitmapSource = new TransformedBitmap(BitmapSource, new ScaleTransform(-1, 1));
            GC.Collect();
        }

        private void Button_MirrorVertical_Click(object sender, RoutedEventArgs e)
        {
            BitmapSource = new TransformedBitmap(BitmapSource, new ScaleTransform(1, -1));
            GC.Collect();
        }

        private void Button_LastPhoto_Click(object sender, RoutedEventArgs e)
        {
            if (ImagesCollection.IndexOf(ImageInfo) > 0)
            {
                ImageInfo = ImagesCollection[ImagesCollection.IndexOf(ImageInfo) - 1];
            }
        }

        private void Button_NextPhoto_Click(object sender, RoutedEventArgs e)
        {
            if (ImagesCollection.IndexOf(ImageInfo) < ImagesCollection.Count - 1)
            {
                ImageInfo = ImagesCollection[ImagesCollection.IndexOf(ImageInfo) + 1];
            }
        }

        private void Button_Crop_Click(object sender, RoutedEventArgs e)
        {
            Content = new ImageEditorCrop(ImagesCollection, ImageInfo);
            GC.Collect();
        }

        private void Button_Rotate_Click(object sender, RoutedEventArgs e)
        {
            Content = new ImageEditorRotate(ImagesCollection, ImageInfo);
            GC.Collect();
        }

        private void ImageEdi_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                if (scaleValue < 3)
                {
                    scaleValue += 0.1;
                }
            }
            else
            {
                if (scaleValue > 1)
                {
                    scaleValue -= 0.1;
                }
            }
            ImageEdi.RenderTransform = new ScaleTransform(scaleValue, scaleValue, e.GetPosition(ImageEdi).X, e.GetPosition(ImageEdi).Y);
            e.Handled = true;
        }
    }
}
