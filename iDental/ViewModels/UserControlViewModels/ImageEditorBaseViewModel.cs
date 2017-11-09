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
                BitmapSource = new CreateBitmapImage().SettingBitmapImage(imageInfo.Image_FullPath, 0);
            }
        }


        private BitmapSource bitmapSource;
        public BitmapSource BitmapSource
        {
            get { return bitmapSource; }
            set
            {
                bitmapSource = value;
                OnPropertyChanged("BitmapSource");
            }
        }

        public ImageEditorBaseViewModel(ImageInfo imageInfo)
        {
            ImageInfo = imageInfo;
        }
    }
}
