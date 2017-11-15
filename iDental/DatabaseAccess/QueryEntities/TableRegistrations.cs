using iDental.iDentalClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace iDental.DatabaseAccess.QueryEntities
{
    public class TableRegistrations
    {
        /// <summary>
        /// 取得所有掛號清單
        /// </summary>
        /// <param name="patients"></param>
        /// <returns></returns>
        public ObservableCollection<ComboBoxItemInfo> QueryRegistrationsList(Patients patients)
        {
            using (var ide = new iDentalEntities())
            {
                var queryRegistrations = (from r in ide.Registrations
                                          where r.Patient_ID == patients.Patient_ID &&
                                          r.Images.Where(s => s.Image_IsEnable == true).Count() > 0
                                          select new
                                          {
                                              Registration_ID = r.Registration_ID,
                                              Registration_Date = r.Registration_Date
                                          }).ToList().Select(s => new ComboBoxItemInfo
                                          {
                                              DisplayName = s.Registration_Date.ToString("yyyy/MM/dd"),
                                              SelectedValue = s.Registration_ID
                                          });
                return new ObservableCollection<ComboBoxItemInfo>(queryRegistrations.ToList().OrderByDescending(o => o.DisplayName));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="patients"></param>
        /// <returns></returns>
        public DateTime QueryLastRegistrationDate(Patients patients)
        {
            using (var ide = new iDentalEntities())
            {
                var queryRegistrationDate = from r in ide.Registrations
                                         where r.Patient_ID == patients.Patient_ID &&
                                         r.Images.Where(s => s.Image_IsEnable == true).Count() > 0
                                         orderby r.Registration_Date descending
                                         select r;
                return queryRegistrationDate.Count() > 0 ? queryRegistrationDate.First().Registration_Date : (DateTime)patients.Patient_FirstRegistrationDate;
            }
        }
    }
}
