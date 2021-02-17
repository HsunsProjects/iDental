using iDental.Class;
using iDental.iDentalClass;
using System.Windows;

namespace iDental.Views.ComboPics
{
    /// <summary>
    /// ComboPic1.xaml 的互動邏輯
    /// </summary>
    public partial class ComboPic1 : Window
    {
        public ComboPic1(ImageInfo imageInfo)
        {
            InitializeComponent();
            ComboPicImage.Source = new CreateBitmapImage().BitmapImageOriginal(imageInfo.Image_FullPath);
        }
    }
}
