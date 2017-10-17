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
                    ide.Patients.Add(patients);
                    ide.SaveChanges();
                    return patients;
                }
            }
        }
    }
}
