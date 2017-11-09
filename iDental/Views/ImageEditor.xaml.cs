using iDental.iDentalClass;
using iDental.Views.UserControlViews;
using System.Collections.ObjectModel;
using System.Windows;

namespace iDental.Views
{
    /// <summary>
    /// ImageEditor.xaml 的互動邏輯
    /// </summary>
    public partial class ImageEditor : Window
    {
        public ImageEditor(ObservableCollection<ImageInfo> imagesCollection)
        {
            InitializeComponent();

            EditorContent.Content = new ImageEditorBase(imagesCollection, imagesCollection[0]);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = true;
        }
    }
}
