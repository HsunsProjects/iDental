using iDental.iDentalClass;
using iDental.ViewModels.UserControlViewModels;
using iDental.ViewModels.ViewModelBase;
using System.Windows.Controls;

namespace iDental.Views.UserControlViews
{
    /// <summary>
    /// FunctionTemplate.xaml 的互動邏輯
    /// </summary>
    public partial class FunctionTemplate : UserControl
    {
        public MTObservableCollection<ImageInfo> DisplayImageInfo
        {
            get { return functionTemplateViewModel.DisplayImageInfo; }
            set { functionTemplateViewModel.DisplayImageInfo = value; }
        }

        private FunctionTemplateViewModel functionTemplateViewModel;
        public FunctionTemplate(Agencys agencys, MTObservableCollection<ImageInfo> displayImageInfo)
        {
            InitializeComponent();

            functionTemplateViewModel = new FunctionTemplateViewModel(agencys);
            DataContext = functionTemplateViewModel;

            DisplayImageInfo = displayImageInfo;
        }
    }
}
