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

        private int imageCollectionCount = 0;
        public int ImageCollectionCount
        {
            get { return imageCollectionCount; }
            set
            {
                imageCollectionCount = value;
                OnPropertyChanged("ImageCollectionCount");
            }
        }

        private int imageIndex;

        public int ImageIndex
        {
            get { return imageIndex; }
            set
            {
                imageIndex = value;
                OnPropertyChanged("ImageTips");
            }
        }

        public string ImageTips
        {
            get { return "[ " + (ImageIndex + 1) + " / " + ImageCollectionCount + " ]"; }
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

        public ImageEditorBaseViewModel(ImageInfo imageInfo, int imageCollectionCount, int imageIndex)
        {
            ImageInfo = imageInfo;
            ImageCollectionCount = imageCollectionCount;
            ImageIndex = imageIndex;
        }
    }
}
