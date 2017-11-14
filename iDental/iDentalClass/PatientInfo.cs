using System;
using System.Collections.Generic;

namespace iDental.iDentalClass
{
    public class PatientInfo
    {
        public string Patient_ID { get; set; }
        public string Patient_Number { get; set; }
        public string Patient_Name { get; set; }
        public bool Patient_Gender { get; set; }
        public DateTime Patient_Birth { get; set; }
        public string Patient_IDNumber { get; set; }
        public string Patient_Photo { get; set; }
        public DateTime Patient_FirstRegistrationDate { get; set; }
        public DateTime Patient_LastRegistrationDate { get; set; }
        public List<PatientCategorys> Patient_PatientCategorys { get; set; }
    }
}
