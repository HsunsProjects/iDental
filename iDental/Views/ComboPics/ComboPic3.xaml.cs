using iDental.Class;
using iDental.iDentalClass;
using System.Windows;

namespace iDental.Views.ComboPics
{
    /// <summary>
    /// ComboPic3.xaml 的互動邏輯
    /// </summary>
    public partial class ComboPic3 : Window
    {
        public ComboPic3(ImageInfo imageInfo1, ImageInfo imageInfo2, ImageInfo imageInfo3)
        {
            InitializeComponent();
            ComboPicImage1.Source = new CreateBitmapImage().BitmapImageOriginal(imageInfo1.Image_FullPath);
            ComboPicImage2.Source = new CreateBitmapImage().BitmapImageOriginal(imageInfo2.Image_FullPath);
            ComboPicImage3.Source = new CreateBitmapImage().BitmapImageOriginal(imageInfo3.Image_FullPath);
        }
    }
}
