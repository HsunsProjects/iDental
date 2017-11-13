using System;
using System.Linq;

namespace iDental.DatabaseAccess.QueryEntities
{
    public class TablePatients
    {
        /// <summary>
        /// 新增查詢病患並回傳
        /// </summary>
        /// <param name="patients">Patients</param>
        /// <returns></returns>
        public Patients QueryNewOldPatient(Patients patients)
        {
            using (var ide = new iDentalEntities())
            {
                var queryPatient = from p in ide.Patients
                                   where p.Patient_ID == patients.Patient_ID
                                   select p;
                if (queryPatient.Count() > 0)
                {
                    return queryPatient.First();
                }
                else
                {
                    patients.Patient_FirstRegistrationDate = DateTime.Now.Date;
                    ide.Patients.Add(patients);
                    ide.SaveChanges();
                    return patients;
                }
            }
        }

        /// <summary>
        /// 查詢病患並回傳
        /// </summary>
        /// <param name="patients">Patients</param>
        /// <returns></returns>
        public Patients QueryPatient(Patients patients)
        {
            using (var ide = new iDentalEntities())
            {
                var queryPatient = from p in ide.Patients
                                   where p.Patient_ID == patients.Patient_ID
                                   select p;
                return queryPatient.First();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newPatientID"></param>
        /// <returns></returns>
        public bool IsUniquePatientID(string newPatientID)
        {
            using (var ide = new iDentalEntities())
            {
                var checkUnique = from p in ide.Patients
                                  where p.Patient_ID == newPatientID
                                  select p;
                return checkUnique.Count() > 0 ? false : true;
            }
        }

        /// <summary>
        /// 更新病患大頭照
        /// </summary>
        /// <param name="patients"></param>
        /// <param name="photoPath"></param>
        public void UpdatePatientPhoto(Patients patients, string photoPath)
        {
            using (var ide = new iDentalEntities())
            {
                Patients p = (from qp in ide.Patients
                              where qp.Patient_ID == patients.Patient_ID
                              select qp).First();
                p.Patient_Photo = photoPath;
                p.UpdateDate = DateTime.Now;
                ide.SaveChanges();
            }
        }

        /// <summary>
        /// 更新病患大頭照
        /// </summary>
        /// <param name="patients"></param>
        /// <param name="photoPath"></param>
        public Patients UpdatePatients(Patients patients)
        {
            using (var ide = new iDentalEntities())
            {
                Patients p = (from qp in ide.Patients
                              where qp.Patient_ID == patients.Patient_ID
                              select qp).First();
                p.Patient_Number = patients.Patient_Number;
                p.Patient_Name = patients.Patient_Name;
                p.Patient_Gender = patients.Patient_Gender;
                p.Patient_Birth = patients.Patient_Birth;
                p.Patient_IDNumber = patients.Patient_IDNumber;
                p.Patient_Photo = patients.Patient_Photo;
                p.Patient_FirstRegistrationDate = patients.Patient_FirstRegistrationDate;
                p.UpdateDate = patients.UpdateDate;
                ide.SaveChanges();
                return p;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="patients"></param>
        public Patients InsertPatients(Patients patients)
        {
            using (var ide = new iDentalEntities())
            {
                ide.Patients.Add(patients);
                ide.SaveChanges();
                return patients;
            }
        }
    }
}
