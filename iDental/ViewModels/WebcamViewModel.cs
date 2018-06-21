using iDental.Class;
using iDental.DatabaseAccess.QueryEntities;
using iDental.iDentalClass;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace iDental.ViewModels
{
    public class WebcamViewModel :ViewModelBase.PropertyChangedBase
    {
        private ObservableCollection<ImageInfo> displayImageInfoList;

        public ObservableCollection<ImageInfo> DisplayImageInfoList
        {
            get { return displayImageInfoList; }
            set
            {
                displayImageInfoList = value;
                OnPropertyChanged("DisplayImageInfoList");
            }
        }

        private ImageInfo selectedItem;

        public ImageInfo SelectedItem
        {
            get
            {
                if (selectedItem != null)
                {
                    if (selectedItem.BitmapImage == null)
                    {
                        selectedItem.BitmapImage = new CreateBitmapImage().BitmapImageShow(selectedItem.Image_FullPath, 800);
                    }
                }
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        private Agencys agencys;

        private Patients patients;

        private DateTime registrationDate;

        private TableImages tableImages;

        public WebcamViewModel(Agencys agencys, Patients patients, DateTime registrationDate)
        {
            tableImages = new TableImages();

            this.agencys = agencys;

            this.patients = patients;

            this.registrationDate = registrationDate;

            RefreshDisplayImageInfoList();
        }

        public void RefreshDisplayImageInfoList()
        {
            DisplayImageInfoList = tableImages.QueryRegistrationDateImageToImageInfo(agencys, patients, registrationDate);
        }
    }
}
