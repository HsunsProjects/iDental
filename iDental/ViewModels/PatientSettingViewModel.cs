using iDental.Class;
using iDental.DatabaseAccess.QueryEntities;
using iDental.iDentalClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace iDental.ViewModels
{
    public class PatientSettingViewModel : ViewModelBase.PropertyChangedBase
    {
        private string windowTitle;

        public string WindowTitle
        {
            get { return  windowTitle; }
            set
            {
                windowTitle = value;
                OnPropertyChanged("WindowTitle");
            }
        }
        
        public Patients Patients { get; set; }

        private string patient_Number;
        public string Patient_Number
        {
            get { return patient_Number; }
            set
            {
                patient_Number = value;
                OnPropertyChanged("Patient_Number");
                if (Patients != null && value == Patients.Patient_Number)
                {
                    TipsVisibility = Visibility.Hidden;
                    SaveIsEnable = true;
                }
                else
                {
                    using (var dde = new iDentalEntities())
                    {
                        var queryPatient = from p in dde.Patients
                                           where p.Patient_Number == patient_Number
                                           select p;

                        if (queryPatient.Count() > 0)
                        {
                            TipsVisibility = Visibility.Visible;
                            SaveIsEnable = false;
                        }
                        else
                        {
                            TipsVisibility = Visibility.Hidden;
                            SaveIsEnable = true;
                        }
                    }
                }
            }
        }

        private string patient_Name;
        public string Patient_Name
        {
            get { return patient_Name; }
            set
            {
                patient_Name = value;
                OnPropertyChanged("Patient_Name");
            }
        }

        private string patient_IDNumber;
        public string Patient_IDNumber
        {
            get { return patient_IDNumber; }
            set
            {
                patient_IDNumber = value;
                OnPropertyChanged("Patient_IDNumber");
            }
        }

        private bool patient_Gender = true;
        public bool Patient_Gender
        {
            get { return patient_Gender; }
            set
            {
                patient_Gender = value;
                OnPropertyChanged("Patient_Gender");
            }
        }

        private DateTime patient_Birth = DateTime.Now;
        public DateTime Patient_Birth
        {
            get { return patient_Birth; }
            set
            {
                patient_Birth = value;
                OnPropertyChanged("Patient_Birth");
            }
        }

        private BitmapImage patient_Photo = new BitmapImage(new Uri(@"pack://application:,,,/iDental;component/Resource/Image/avatar.jpg", UriKind.Absolute));
        public BitmapImage Patient_Photo
        {
            get { return patient_Photo; }
            set
            {
                patient_Photo = value;
                OnPropertyChanged("Patient_Photo");
            }
        }

        private DateTime patient_FirstRegistrationDate = DateTime.Now;
        public DateTime Patient_FirstRegistrationDate
        {
            get { return patient_FirstRegistrationDate; }
            set
            {
                patient_FirstRegistrationDate = value;
                OnPropertyChanged("Patient_FirstRegistrationDate");
            }
        }

        private string patient_LastRegistrationDate = DateTime.Now.ToString("yyyy/MM/dd");
        public string Patient_LastRegistrationDate
        {
            get { return patient_LastRegistrationDate; }
            set
            {
                patient_LastRegistrationDate = value;
                OnPropertyChanged("Patient_LastRegistrationDate");
            }
        }

        private List<PatientCategoryInfo> patientCategoryInfo;
        public List<PatientCategoryInfo> PatientCategoryInfo
        {
            get { return patientCategoryInfo; }
            set
            {
                patientCategoryInfo = value;
                OnPropertyChanged("PatientCategoryInfo");
            }
        }

        private Visibility tipsVisibility = Visibility.Hidden;
        public Visibility TipsVisibility
        {
            get { return tipsVisibility; }
            set
            {
                tipsVisibility = value;
                OnPropertyChanged("TipsVisibility");
            }
        }

        private bool saveIsEnable = true;
        public bool SaveIsEnable
        {
            get { return saveIsEnable; }
            set
            {
                saveIsEnable = value;
                OnPropertyChanged("SaveIsEnable");
            }
        }

        //新增
        public PatientSettingViewModel(string windowTitle)
        {
            WindowTitle = windowTitle;
        }
        
        //編輯
        public PatientSettingViewModel(string windowTitle, Agencys agencys, Patients patients)
        {
            WindowTitle = windowTitle;
            Patients = patients;

            //設定病患資料
            //載入設定就好，避免多設定
            Patient_Number = patients.Patient_Number;
            Patient_Name = patients.Patient_Name;
            Patient_IDNumber = patients.Patient_IDNumber;
            Patient_Gender = patients.Patient_Gender;
            Patient_Birth = patients.Patient_Birth;
            if (PathCheck.IsFileExist(agencys.Agency_ImagePath + patients.Patient_Photo))
            {
                Patient_Photo = new CreateBitmapImage().SettingBitmapImage(agencys.Agency_ImagePath + patients.Patient_Photo, 400);
            }
            Patient_FirstRegistrationDate = patients.Patient_FirstRegistrationDate == null ? DateTime.Now : (DateTime)patients.Patient_FirstRegistrationDate;
            DateTime lastRegistrationDate = new TableRegistrations().QueryLastRegistrationDate(patients);
            Patient_LastRegistrationDate = lastRegistrationDate == null ? Patient_FirstRegistrationDate.ToString("yyyy/MM/dd") : lastRegistrationDate.ToString("yyyy/MM/dd");

            PatientCategoryInfo = new TablePatientCategorys().QueryPatientPatientCategoryInfo(Patients).ToList().FindAll(s => s.IsChecked == true);
        }
    }
}
