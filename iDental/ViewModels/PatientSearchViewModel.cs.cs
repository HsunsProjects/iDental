using iDental.DatabaseAccess.QueryEntities;
using iDental.iDentalClass;
using System;
using System.Collections.Generic;
using System.Linq;

namespace iDental.ViewModels
{
    public class PatientSearchViewModel : ViewModelBase.PropertyChangedBase
    {
        private DateTime selectStartDate = DateTime.Now;
        public DateTime SelectStartDate
        {
            get { return selectStartDate; }
            set
            {
                selectStartDate = value;
                OnPropertyChanged("SelectStartDate");
            }
        }

        private DateTime selectEndDate = DateTime.Now;
        public DateTime SelectEndDate
        {
            get { return selectEndDate; }
            set
            {
                selectEndDate = value;
                OnPropertyChanged("SelectEndDate");
            }
        }

        private List<PatientCategorys> listPatientCategorys;
        public List<PatientCategorys> ListPatientCategorys
        {
            get { return listPatientCategorys; }
            set
            {
                listPatientCategorys = value;
                OnPropertyChanged("ListPatientCategorys");
            }
        }

        private List<PatientInfo> listPatientInfo;

        public List<PatientInfo> ListPatientInfo
        {
            get { return listPatientInfo; }
            set
            {
                listPatientInfo = value;
                OnPropertyChanged("ListPatientInfo");
            }
        }

        private List<PatientInfo> displayPatientInfo;

        public List<PatientInfo> DisplayPatientInfo
        {
            get { return displayPatientInfo; }
            set
            {
                displayPatientInfo = value;
                OnPropertyChanged("DisplayPatientInfo");
            }
        }



        public PatientSearchViewModel()
        {
            using (var ide = new iDentalEntities())
            {
                ListPatientCategorys = ide.PatientCategorys.ToList();
                DisplayPatientInfo = ListPatientInfo = new TablePatients().QueryPatientAllToPatientInfo();
            }
        }
    }
}
