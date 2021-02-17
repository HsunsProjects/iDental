using iDental.Class;
using iDental.iDentalClass;
using System.Windows;

namespace iDental.Views.ComboPics
{
    /// <summary>
    /// ComboPic2.xaml 的互動邏輯
    /// </summary>
    public partial class ComboPic2 : Window
    {
        public ComboPic2(ImageInfo imageInfo1, ImageInfo imageInfo2)
        {
            InitializeComponent();
            ComboPicImage1.Source = new CreateBitmapImage().BitmapImageOriginal(imageInfo1.Image_FullPath);
            ComboPicImage2.Source = new CreateBitmapImage().BitmapImageOriginal(imageInfo2.Image_FullPath);
        }
    }
}
