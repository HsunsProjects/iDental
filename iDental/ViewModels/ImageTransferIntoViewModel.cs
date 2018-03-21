using iDental.Class;
using iDental.DatabaseAccess.QueryEntities;
using iDental.iDentalClass;
using iDental.Views;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace iDental.ViewModels
{
    public class ImageTransferIntoViewModel : ViewModelBase.PropertyChangedBase
    {
        public Agencys Agencys { get; set; }

        private Patients patients;

        public Patients Patients
        {
            get { return patients; }
            set
            {
                patients = value;
                OnPropertyChanged("Patients");
            }
        }

        private Patients targetPatients;

        public Patients TargetPatients
        {
            get { return targetPatients; }
            set
            {
                targetPatients = value;
                if (targetPatients == null)
                {
                    TransPatient = "(尚未選取病患)";
                }
                else
                {
                    TransPatient = "(目標病患:" + targetPatients.Patient_Name + " 生日: " + targetPatients.Patient_Birth.ToString("yyyy/MM/dd") + ")";
                }
                OnPropertyChanged("TargetPatients");
            }
        }


        private ObservableCollection<ImageInfo> displayImageInfoList;

        public ObservableCollection<ImageInfo> DisplayImageInfoList
        {
            get { return displayImageInfoList; }
            set
            {
                displayImageInfoList = value;
                DisplayImageInfoListCount = displayImageInfoList.Count;
                OnPropertyChanged("DisplayImageInfoList");
            }
        }

        private int displayImageInfoListCount;

        public int DisplayImageInfoListCount
        {
            get { return displayImageInfoListCount; }
            set
            {
                displayImageInfoListCount = value;
                OnPropertyChanged("DisplayImageInfoListCount");
            }
        }

        private bool rbSelf;

        public bool RbSelf
        {
            get { return rbSelf; }
            set
            {
                rbSelf = value;
                if (rbSelf)
                {
                    TargetPatients = Patients;
                    IsShowPatientTips = Visibility.Hidden;
                }
                else
                {
                    TargetPatients = null;
                    IsShowPatientTips = Visibility.Visible;
                }
                OnPropertyChanged("RbSelf");
            }
        }

        private bool rbNewRegistrationDate;

        public bool RbNewRegistrationDate
        {
            get { return rbNewRegistrationDate; }
            set
            {
                rbNewRegistrationDate = value;
                if (rbNewRegistrationDate)
                {
                    //設定掛號日
                    TransRegistrationDate = NewRegistrationDate.ToString("yyyy/MM/dd");
                }
                OnPropertyChanged("RbNewRegistrationDate");
            }
        }

        private DateTime newRegistrationDate = DateTime.Now;

        public DateTime NewRegistrationDate
        {
            get { return newRegistrationDate; }
            set
            {
                newRegistrationDate = value;
                //設定掛號日
                TransRegistrationDate = newRegistrationDate.ToString("yyyy/MM/dd");
                OnPropertyChanged("NewRegistrationDate");
            }
        }

        private bool rbOldRegistrationDate;

        public bool RbOldRegistrationDate
        {
            get { return rbOldRegistrationDate; }
            set
            {
                rbOldRegistrationDate = value;
                if (rbOldRegistrationDate)
                {
                    //設定掛號日
                    RegistrationsList = new TableRegistrations().QueryRegistrationsList(TargetPatients);
                    if (RegistrationsList.Count > 0)
                    {
                        SelectRegistrationsDate = RegistrationsList[0];
                    }
                    else
                    {
                        SelectRegistrationsDate = null;
                    }
                }
                OnPropertyChanged("RbOldRegistrationDate");
            }
        }

        private ObservableCollection<ComboBoxItemInfo> registrationsList;
        public ObservableCollection<ComboBoxItemInfo> RegistrationsList
        {
            get { return registrationsList; }
            set
            {
                registrationsList = value;
                OnPropertyChanged("RegistrationsList");
            }
        }

        private ComboBoxItemInfo selectRegistrationsDate;

        public ComboBoxItemInfo SelectRegistrationsDate
        {
            get { return selectRegistrationsDate; }
            set
            {
                selectRegistrationsDate = value;
                //設定掛號日
                if (selectRegistrationsDate != null)
                {
                    TransRegistrationDate = selectRegistrationsDate.DisplayName;
                }
                else
                {
                    TransRegistrationDate = string.Empty;
                }
                OnPropertyChanged("SelectRegistrationsDate");
            }
        }

        private Visibility isShowPatientTips;

        public Visibility IsShowPatientTips
        {
            get { return isShowPatientTips; }
            set
            {
                isShowPatientTips = value;
                OnPropertyChanged("IsShowPatientTips");
            }
        }


        private string transPatient;

        public string TransPatient
        {
            get { return transPatient; }
            set
            {
                transPatient = value;
                OnPropertyChanged("TransPatient");
            }
        }


        private string transRegistrationDate;

        public string TransRegistrationDate
        {
            get { return transRegistrationDate; }
            set
            {
                transRegistrationDate = value;
                OnPropertyChanged("TransRegistrationDate");
            }
        }

        public ImageTransferIntoViewModel(Agencys agencys, Patients patients, ObservableCollection<ImageInfo> displayImageInfoList)
        {
            Agencys = agencys;
            Patients = patients;
            RbSelf = true;
            RbNewRegistrationDate = true;
            DisplayImageInfoList = displayImageInfoList;
        }

        public bool CanSave()
        {
            if (Patients != null && TargetPatients != null && !string.IsNullOrEmpty(TransRegistrationDate))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
