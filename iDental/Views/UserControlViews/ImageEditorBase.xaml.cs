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

        private BitmapImage BitmapImage
        {
            get { return imageEditorBaseViewModel.BitmapImage; }
            set { imageEditorBaseViewModel.BitmapImage = value; }
        }

        /// <summary>
        /// 滑鼠滾輪的值
        /// </summary>
        private double scaleValue = 1;

        private ImageEditorBaseViewModel imageEditorBaseViewModel;

        private TransformGroup transformGroup;
        private RotateTransform rotateTransform;
        private ScaleTransform scaleTransform;

        public ImageEditorBase(ObservableCollection<ImageInfo> imagesCollection, ImageInfo imageInfo)
        {
            InitializeComponent();

            ImagesCollection = imagesCollection;

            imageEditorBaseViewModel = new ImageEditorBaseViewModel(imageInfo, imagesCollection.Count, 0);

            DataContext = imageEditorBaseViewModel;

            transformGroup = new TransformGroup();

            rotateTransform = new RotateTransform();

            scaleTransform = new ScaleTransform();

            transformGroup.Children.Add(rotateTransform);
            transformGroup.Children.Add(scaleTransform);
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //儲存新影像
                BitmapSource bitmapSource = BitmapImage;
                bitmapSource = new TransformedBitmap(bitmapSource, transformGroup);
                ImageHelper.SaveUsingEncoder(bitmapSource, ImageInfo.Image_FullPath, ImageInfo.Image_Extension);
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
                    BitmapSource bitmapSource = BitmapImage;
                    bitmapSource = new TransformedBitmap(bitmapSource, transformGroup);
                    ImageHelper.SaveUsingEncoder(bitmapSource, sfd.FileName, extension);
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
            ////直接修改原圖BitmapImage
            //BitmapSource = new TransformedBitmap(BitmapSource, new RotateTransform(-90));

            if (scaleTransform.ScaleY == 1)
            {
                if (scaleTransform.ScaleX == 1)
                {
                    if (rotateTransform.Angle == -360)
                        rotateTransform.Angle = 0;
                    rotateTransform.Angle -= 90;
                }
                else
                {
                    if (rotateTransform.Angle == 360)
                        rotateTransform.Angle = 0;
                    rotateTransform.Angle += 90;
                }
            }
            else
            {
                if (scaleTransform.ScaleX == 1)
                {
                    if (rotateTransform.Angle == 360)
                        rotateTransform.Angle = 0;
                    rotateTransform.Angle += 90;
                }
                else
                {
                    if (rotateTransform.Angle == -360)
                        rotateTransform.Angle = 0;
                    rotateTransform.Angle -= 90;
                }
            }

            ImageEdi.LayoutTransform = transformGroup;

            GC.Collect();
        }
        private void Button_RotateRight_Click(object sender, RoutedEventArgs e)
        {
            ////直接修改原圖BitmapImage
            //BitmapSource = new TransformedBitmap(BitmapSource, new RotateTransform(90));

            if (scaleTransform.ScaleY == 1)
            {
                if (scaleTransform.ScaleX == 1)
                {
                    if (rotateTransform.Angle == 360)
                        rotateTransform.Angle = 0;
                    rotateTransform.Angle += 90;
                }
                else
                {
                    if (rotateTransform.Angle == -360)
                        rotateTransform.Angle = 0;
                    rotateTransform.Angle -= 90;
                }
            }
            else
            {
                if (scaleTransform.ScaleX == 1)
                {
                    if (rotateTransform.Angle == -360)
                        rotateTransform.Angle = 0;
                    rotateTransform.Angle -= 90;
                }
                else
                {
                    if (rotateTransform.Angle == 360)
                        rotateTransform.Angle = 0;
                    rotateTransform.Angle += 90;
                }
            }
            
            ImageEdi.LayoutTransform = transformGroup;

            GC.Collect();
        }

        private void Button_MirrorHorizontal_Click(object sender, RoutedEventArgs e)
        {
            ////直接修改原圖BitmapImage
            //BitmapSource = new TransformedBitmap(BitmapSource, new ScaleTransform(-1, 1));

            if (scaleTransform.ScaleX == 1)
            {
                if (scaleTransform.ScaleY == 1)
                {
                    MirrorSetting("VrH");
                }
                else
                {
                    MirrorSetting("rVrH");
                }
                scaleTransform.ScaleX = -1;
            }
            else
            {
                if (scaleTransform.ScaleY == 1)
                {
                    MirrorSetting("VH");
                }
                else
                {
                    MirrorSetting("rVH");
                }
                scaleTransform.ScaleX = 1;
            }

            ImageEdi.LayoutTransform = transformGroup;

            GC.Collect();
        }

        private void Button_MirrorVertical_Click(object sender, RoutedEventArgs e)
        {
            ////直接修改原圖BitmapImage
            //BitmapSource = new TransformedBitmap(BitmapSource, new ScaleTransform(1, -1));

            if (scaleTransform.ScaleY == 1)
            {
                if (scaleTransform.ScaleX == 1)
                {
                    MirrorSetting("rVH");
                }
                else
                {
                    MirrorSetting("rVrH");
                }
                scaleTransform.ScaleY = -1;
            }
            else
            {
                if (scaleTransform.ScaleX == 1)
                {
                    MirrorSetting("VH");
                }
                else
                {
                    MirrorSetting("VrH");
                }
                scaleTransform.ScaleY = 1;
            }
            
            ImageEdi.LayoutTransform = transformGroup;

            GC.Collect();
        }

        private void Button_LastPhoto_Click(object sender, RoutedEventArgs e)
        {
            if (ImagesCollection.IndexOf(ImageInfo) > 0)
            {
                rotateTransform.Angle = 0;
                scaleTransform.ScaleX = 1;
                scaleTransform.ScaleY = 1;
                ImageEdi.LayoutTransform = transformGroup;
                ImageInfo = ImagesCollection[ImagesCollection.IndexOf(ImageInfo) - 1];
                imageEditorBaseViewModel.ImageIndex = ImagesCollection.IndexOf(ImageInfo);
            }
        }

        private void Button_NextPhoto_Click(object sender, RoutedEventArgs e)
        {
            if (ImagesCollection.IndexOf(ImageInfo) < ImagesCollection.Count - 1)
            {
                rotateTransform.Angle = 0;
                scaleTransform.ScaleX = 1;
                scaleTransform.ScaleY = 1;
                ImageEdi.LayoutTransform = transformGroup;
                ImageInfo = ImagesCollection[ImagesCollection.IndexOf(ImageInfo) + 1];
                imageEditorBaseViewModel.ImageIndex = ImagesCollection.IndexOf(ImageInfo);
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

        private void MirrorSetting(string imageDir)
        {
            switch(imageDir)
            {
                case "VH":
                    //正常
                    scaleTransform.ScaleX = 1;
                    scaleTransform.ScaleY = 1;
                    break;
                case "rVH":
                    //上下顛倒
                    scaleTransform.ScaleX = 1;
                    scaleTransform.ScaleY = -1;
                    break;
                case "VrH":
                    //左右顛倒
                    scaleTransform.ScaleX = -1;
                    scaleTransform.ScaleY = 1;
                    break;
                case "rVrH":
                    //上下左右顛倒
                    scaleTransform.ScaleX = -1;
                    scaleTransform.ScaleY = -1;
                    break;

            }
        }
    }
}
