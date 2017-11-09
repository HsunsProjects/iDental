using System;

namespace iDental.iDentalClass
{
    public static class PatientFolderSetting
    {
        /// <summary>
        /// 病患大頭照路徑設定
        /// </summary>
        /// <param name="agencys">機構資料</param>
        /// <param name="patients">病患資料</param>
        /// <returns></returns>
        public static PatientPhotoFolderInfo PatientPhotoFolderSetting(Agencys agencys, Patients patients)
        {
            PatientPhotoFolderInfo patientPhotoInfo = new PatientPhotoFolderInfo()
            {
                PatientPhotoPath = @"\" + patients.Patient_ID + @"\PatientPhoto",
                PatientPhotoFullPath = agencys.Agency_ImagePath + @"\" + patients.Patient_ID + @"\PatientPhoto"
            };
            return patientPhotoInfo;
        }

        /// <summary>
        /// 病患影像路徑設定
        /// </summary>
        /// <param name="agencys">機構資料</param>
        /// <param name="patients">病患資料</param>
        /// <param name="registrationDate">掛號日</param>
        public static PatientImageFolderInfo PatientImageFolderSetting(Agencys agencys, Patients patients, DateTime registrationDate)
        {
            PatientImageFolderInfo patientImageFolderInfo = new PatientImageFolderInfo()
            {
                PatientImagePath = @"\" + patients.Patient_ID + @"\" + registrationDate.ToString("yyyyMMdd"),
                PatientImageFullPath = agencys.Agency_ImagePath + @"\" + patients.Patient_ID + @"\" + registrationDate.ToString("yyyyMMdd")
            };
            return patientImageFolderInfo;
        }
    }
}
