using iDental.Class;
using iDental.iDentalClass;
using System.Windows.Media.Imaging;

namespace iDental.ViewModels.UserControlViewModels
{
    public class ImageEditorBaseViewModel : ViewModelBase.PropertyChangedBase
    {
        private ImageInfo imageInfo;

        public ImageInfo ImageInfo
        {
            get { return imageInfo; }
            set
            {
                imageInfo = value;
                OnPropertyChanged("ImageInfo");
                BitmapImage = new CreateBitmapImage().SettingBitmapImage(imageInfo.Image_FullPath, 0);
            }
        }


        private BitmapImage bitmapImage;
        public BitmapImage BitmapImage
        {
            get { return bitmapImage; }
            set
            {
                bitmapImage = value;
                OnPropertyChanged("BitmapImage");
            }
        }

        public ImageEditorBaseViewModel(ImageInfo imageInfo)
        {
            ImageInfo = imageInfo;
        }
    }
}
