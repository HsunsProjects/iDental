using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace iDental.Class
{
    public class CreateBitmapImage
    {
        private BitmapImage bitmapImage;
        /// <summary>
        /// 建立圖片BitmapImage
        /// </summary>
        /// <param name="filePath">影像路徑</param>
        /// <param name="decodePixelWidth">影像解析</param>
        public BitmapImage SettingBitmapImage(string filePath, int decodePixelWidth)
        {
            try
            {
                bitmapImage = new BitmapImage();
                if (File.Exists(filePath))
                {
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                    {
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = fileStream;
                        bitmapImage.DecodePixelWidth = decodePixelWidth;
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.EndInit();
                        bitmapImage.Freeze();
                        fileStream.Close();
                    }
                }
                return bitmapImage;
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorMessageOutput(ex.ToString());
                return bitmapImage;
            }
        }
    }
}
