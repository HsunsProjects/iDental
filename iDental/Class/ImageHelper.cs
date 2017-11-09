using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace iDental.Class
{
    public static class ImageHelper
    {
        /// <summary>
        /// Rotate the given image file according to Exif Orientation data
        /// </summary>
        /// <param name="sourceFilePath">path of source file</param>
        /// <param name="targetFilePath">path of target file</param>
        /// <param name="targetFormat">target format</param>
        /// <param name="updateExifData">set it to TRUE to update image Exif data after rotation (default is TRUE)</param>
        /// <returns>The RotateFlipType value corresponding to the applied rotation. If no rotation occurred, RotateFlipType.RotateNoneFlipNone will be returned.</returns>
        public static void RotateImageByExifOrientationData(string sourceFilePath, string targetFilePath, string imageExtension, bool updateExifData = true)
        {
            // Rotate the image according to EXIF data
            var bmp = new Bitmap(sourceFilePath);
            RotateFlipType fType = RotateImageByExifOrientationData(bmp, updateExifData);
            if (fType != RotateFlipType.RotateNoneFlipNone)
            {
                bmp.Save(targetFilePath, GetImageFormat(imageExtension));
            }
            else
            {
                File.Copy(sourceFilePath, targetFilePath);
            }
        }

        /// <summary>
        /// Rotate the given bitmap according to Exif Orientation data
        /// </summary>
        /// <param name="img">source image</param>
        /// <param name="updateExifData">set it to TRUE to update image Exif data after rotation (default is TRUE)</param>
        /// <returns>The RotateFlipType value corresponding to the applied rotation. If no rotation occurred, RotateFlipType.RotateNoneFlipNone will be returned.</returns>
        public static RotateFlipType RotateImageByExifOrientationData(Image img, bool updateExifData = true)
        {
            int orientationId = 0x0112;
            var fType = RotateFlipType.RotateNoneFlipNone;
            if (img.PropertyIdList.Contains(orientationId))
            {
                var pItem = img.GetPropertyItem(orientationId);
                fType = GetRotateFlipTypeByExifOrientationData(pItem.Value[0]);
                if (fType != RotateFlipType.RotateNoneFlipNone)
                {
                    img.RotateFlip(fType);
                    if (updateExifData) img.RemovePropertyItem(orientationId); // Remove Exif orientation tag
                }
            }
            return fType;
        }

        /// <summary>
        /// Return the proper System.Drawing.RotateFlipType according to given orientation EXIF metadata
        /// </summary>
        /// <param name="orientation">Exif "Orientation"</param>
        /// <returns>the corresponding System.Drawing.RotateFlipType enum value</returns>
        public static RotateFlipType GetRotateFlipTypeByExifOrientationData(int orientation)
        {
            switch (orientation)
            {
                case 1:
                default:
                    return RotateFlipType.RotateNoneFlipNone;
                case 2:
                    return RotateFlipType.RotateNoneFlipX;
                case 3:
                    return RotateFlipType.Rotate180FlipNone;
                case 4:
                    return RotateFlipType.Rotate180FlipX;
                case 5:
                    return RotateFlipType.Rotate90FlipX;
                case 6:
                    return RotateFlipType.Rotate90FlipNone;
                case 7:
                    return RotateFlipType.Rotate270FlipX;
                case 8:
                    return RotateFlipType.Rotate270FlipNone;
            }
        }

        public static ImageFormat GetImageFormat(string imageExtension)
        {
            ImageFormat imageFormat = new ImageFormat(Guid.NewGuid());
            switch (imageExtension.ToUpper())
            {
                case ".JPG":
                case ".JPEG":
                    imageFormat = ImageFormat.Jpeg;
                    break;
                case ".PNG":
                    imageFormat = ImageFormat.Png;
                    break;
                case ".GIF":
                    imageFormat = ImageFormat.Gif;
                    break;
            }
            return imageFormat;
        }

        /// <summary>
        /// 儲存 BitmapIimage 影像
        /// </summary>
        /// <param name="bitmapSource">BitmapSource資料來源</param>
        /// <param name="fileName">儲存路徑</param>
        /// <param name="encoder">編碼</param>
        public static void SaveUsingEncoder(BitmapSource bitmapSource, string fileName, string extension)
        {
            using (var stream = File.Create(fileName))
            {
                BitmapEncoder encoder;
                switch (extension.ToUpper())
                {
                    case ".JPG":
                    case ".JPEG":
                        encoder = new JpegBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                        encoder.Save(stream);
                        break;
                    case ".PNG":
                        encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                        encoder.Save(stream);
                        break;
                    case ".GIF":
                        encoder = new GifBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                        encoder.Save(stream);
                        break;
                }
            }
        }

        /// <summary>
        /// 切圖
        /// </summary>
        /// <param name="bitmapSource">圖源</param>
        /// <param name="cut">裁切區</param>
        /// <returns></returns>
        public static BitmapSource CutImage(BitmapSource bitmapSource, Int32Rect cut)
        {
            //計算Stride
            var stride = bitmapSource.Format.BitsPerPixel * cut.Width;
            //
            byte[] data = new byte[cut.Height * stride];
            //CopyPixels
            bitmapSource.CopyPixels(cut, data, stride, 0);

            return BitmapSource.Create(cut.Width, cut.Height, 0, 0, PixelFormats.Bgr32, null, data, stride);
        }

        // ImageSource --> Bitmap
        public static Bitmap ImageSourceToBitmap(ImageSource imageSource)
        {
            BitmapSource m = (BitmapSource)imageSource;

            Bitmap bmp = new Bitmap(m.PixelWidth, m.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            BitmapData data = bmp.LockBits(
            new Rectangle(System.Drawing.Point.Empty, bmp.Size), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            m.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride); bmp.UnlockBits(data);

            return bmp;
        }

        // Bitmap --> BitmapImage
        public static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Bmp);

                stream.Position = 0;
                BitmapImage result = new BitmapImage();
                result.BeginInit();
                // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
                // Force the bitmap to load right now so we can dispose the stream.
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();

                return result;
            }
        }
    }
}
