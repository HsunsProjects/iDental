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
    }
}
