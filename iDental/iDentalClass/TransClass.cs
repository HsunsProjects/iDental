using iDental.Class;
using System;

namespace iDental.iDentalClass
{
    public class TransClass
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="images"></param>
        /// <returns></returns>
        public ImageInfo TransImagesToImageInfo(Agencys agencys, Images images, DateTime registrationDate, int decodePixelWidth)
        {
            ImageInfo imageInfo = new ImageInfo()
            {
                Registration_Date = registrationDate,
                Image_ID = images.Image_ID,
                Image_Path = images.Image_Path,
                Image_FullPath = agencys.Agency_ImagePath + images.Image_Path,
                Image_FileName = images.Image_FileName,
                Image_Extension = images.Image_Extension,
                CreateDate = images.CreateDate,
                Registration_ID = images.Registration_ID,
                BitmapImage = new CreateBitmapImage().BitmapImageShow(agencys.Agency_ImagePath + images.Image_Path, decodePixelWidth),
                IsSelected = false
            };
            return imageInfo;
        }
    }
}
