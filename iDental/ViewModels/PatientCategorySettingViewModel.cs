using iDental.DatabaseAccess.QueryEntities;
using System.Collections.ObjectModel;

namespace iDental.ViewModels
{
    public class PatientCategorySettingViewModel : ViewModelBase.PropertyChangedBase
    {
        private ObservableCollection<PatientCategorys> patientCategorys;

        public ObservableCollection<PatientCategorys> PatientCategorys
        {
            get { return patientCategorys; }
            set
            {
                patientCategorys = value;
                OnPropertyChanged("PatientCategorys");
            }
        }

        public PatientCategorySettingViewModel()
        {
            PatientCategorys = new TablePatientCategorys().QueryAllPatientCategory();
        }
    }
}
