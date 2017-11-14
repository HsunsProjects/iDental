using iDental.iDentalClass;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace iDental.DatabaseAccess.QueryEntities
{
    public class TablePatientCategorys
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<PatientCategorys> QueryAllPatientCategory()
        {
            using (var ide = new iDentalEntities())
            {
                return new ObservableCollection<PatientCategorys>(ide.PatientCategorys.OrderByDescending(o => o.PatientCategory_ID));
            }
        }

        /// <summary>
        /// 所有病患分類並轉換成PatientCategoryInfo
        /// </summary>
        /// <returns></returns>
        public List<PatientCategoryInfo> QueryAllPatientCategoryInfo()
        {
            using (var ide = new iDentalEntities())
            {
                var qpc = from pc in ide.PatientCategorys
                          select new PatientCategoryInfo()
                          {
                              PatientCategory_ID = pc.PatientCategory_ID,
                              PatientCategory_Title = pc.PatientCategory_Title,
                              IsChecked = false
                          };
                return qpc.ToList();
            }
        }

        /// <summary>
        /// 病患有無勾選的分類
        /// </summary>
        /// <param name="patients"></param>
        /// <returns></returns>
        public List<PatientCategoryInfo> QueryPatientPatientCategoryInfo(Patients patients)
        {
            using (var ide = new iDentalEntities())
            {
                var qpc = from pc in ide.PatientCategorys
                          select new PatientCategoryInfo()
                          {
                              PatientCategory_ID = pc.PatientCategory_ID,
                              PatientCategory_Title = pc.PatientCategory_Title,
                              IsChecked = pc.Patients.Where(p => p.Patient_ID == patients.Patient_ID).Count() > 0 ? true : false
                          };
                return qpc.ToList();
            }
        }

        /// <summary>
        /// 病患分類
        /// </summary>
        /// <param name="patients"></param>
        /// <returns></returns>
        public List<PatientCategoryInfo> QueryPatientCheckedPatientCategoryInfo(Patients patients)
        {
            using (var ide = new iDentalEntities())
            {
                var qpc = from pc in ide.PatientCategorys
                          where pc.Patients.Any(p => p.Patient_ID == patients.Patient_ID)
                          select new PatientCategoryInfo()
                          {
                              PatientCategory_ID = pc.PatientCategory_ID,
                              PatientCategory_Title = pc.PatientCategory_Title,
                              IsChecked = true
                          };
                return qpc.ToList();
            }
        }
        public List<PatientCategorys> QueryPatientCategoryItem(string keyword)
        {
            using (var ide = new iDentalEntities())
            {
                return ide.PatientCategorys.Where(pc => pc.PatientCategory_Title.Contains(keyword)).ToList();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="patientCategorys"></param>
        public void InsertPatientCategorys(PatientCategorys patientCategorys)
        {
            using (var ide = new iDentalEntities())
            {
                ide.PatientCategorys.Add(patientCategorys);
                ide.SaveChanges();
            }
        }

        public void InsertPatientsPatientCategorys(Patients patients, List<PatientCategoryInfo> list)
        {
            using (var ide = new iDentalEntities())
            {
                Patients p = (from ps in ide.Patients
                              where ps.Patient_ID == patients.Patient_ID
                              select ps).First();

                foreach (PatientCategorys pc in ide.PatientCategorys)
                {
                    var queryCheck = list.FindAll(pci => pci.PatientCategory_ID == pc.PatientCategory_ID);
                    if (queryCheck.Count() > 0)
                    {
                        p.PatientCategorys.Remove(pc);
                        p.PatientCategorys.Add(pc);
                    }
                    else
                    {
                        p.PatientCategorys.Remove(pc);
                    }
                }
                ide.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="patientCategorys"></param>
        /// <param name="newTitle"></param>
        public void UpdatePatientCategorysTitle(PatientCategorys patientCategorys, string newTitle)
        {
            using (var ide = new iDentalEntities())
            {
                PatientCategorys updateItem = (from pc in ide.PatientCategorys
                                               where pc.PatientCategory_ID == patientCategorys.PatientCategory_ID
                                               select pc).First();
                updateItem.PatientCategory_Title = newTitle;
                ide.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="patientCategorys"></param>
        /// <param name="newTitle"></param>
        public void DeletePatientCategorys(PatientCategorys patientCategorys)
        {
            using (var ide = new iDentalEntities())
            {
                var deleteItem = ide.PatientCategorys.Where(w => w.PatientCategory_ID == patientCategorys.PatientCategory_ID).First();
                deleteItem.Patients.Remove(new Patients());
                ide.PatientCategorys.Remove(deleteItem);
                ide.SaveChanges();

            }
        }
    }
}
