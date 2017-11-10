using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace iDental.iDentalClass
{
    public static class UIElementExport
    {
        public static void UIElementExportImage(FrameworkElement frameworkElement, string filePath)
        {
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)frameworkElement.ActualWidth, (int)frameworkElement.ActualHeight, 96, 96, PixelFormats.Pbgra32);

            bmp.Render(frameworkElement);

            var encoder = new PngBitmapEncoder();

            encoder.Frames.Add(BitmapFrame.Create(bmp));

            using (Stream stm = File.Create(filePath))
            {
                encoder.Save(stm);
            }
        }
    }
}
