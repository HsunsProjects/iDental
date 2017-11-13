using iDental.DatabaseAccess.QueryEntities;
using iDental.iDentalClass;
using System.Collections.Generic;

namespace iDental.ViewModels
{
    public class PatientCategoryViewModel : ViewModelBase.PropertyChangedBase
    {
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

        private List<PatientCategoryInfo> displayPatientCategoryInfo;
        public List<PatientCategoryInfo> DisplayPatientCategoryInfo
        {
            get { return displayPatientCategoryInfo; }
            set
            {
                displayPatientCategoryInfo = value;
                OnPropertyChanged("DisplayPatientCategoryInfo");
            }
        }

        public PatientCategoryViewModel()
        {
            DisplayPatientCategoryInfo = PatientCategoryInfo = new TablePatientCategorys().QueryAllPatientCategoryInfo();
        }

        public PatientCategoryViewModel(Patients patients)
        {
            DisplayPatientCategoryInfo = PatientCategoryInfo = new TablePatientCategorys().QueryPatientPatientCategoryInfo(patients);
        }
    }
}
