using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace iDental.Class
{
    public class CreateBitmapImage
    {
        private BitmapImage bitmapImage;
        /// <summary>
        /// 建立圖片BitmapImage(載入時看到時使用)
        /// </summary>
        /// <param name="filePath">影像路徑</param>
        /// <param name="decodePixelWidth">影像解析</param>
        public BitmapImage BitmapImageShow(string filePath, int decodePixel)
        {
            try
            {
                bitmapImage = new BitmapImage();
                if (File.Exists(filePath))
                {
                    if (decodePixel.Equals(0))
                    {
                        int configDecodePixe;
                        if (int.TryParse(ConfigManage.ReadAppConfig("ImageDecodePixel"), out configDecodePixe))
                        {
                            decodePixel = configDecodePixe;
                        }
                        else
                        {
                            decodePixel = 0;
                        }
                    }
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                    {
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = fileStream;
                        bitmapImage.DecodePixelWidth = decodePixel;
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

        /// <summary>
        /// 建立圖片BitmapImage(異動後用原圖變更)
        /// </summary>
        /// <param name="filePath">影像路徑</param>
        public BitmapImage BitmapImageOriginal(string filePath)
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
