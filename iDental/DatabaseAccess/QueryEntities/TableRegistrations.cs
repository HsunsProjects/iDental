using iDental.iDentalClass;
using System;
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
                ObservableCollection<ComboBoxItemInfo> observableCollection = new ObservableCollection<ComboBoxItemInfo>();

                var queryRegistrations = (from r in ide.Registrations
                                         where r.Patient_ID == patients.Patient_ID
                                         select new
                                         {
                                             Registration_ID = r.Registration_ID,
                                             Registration_Date = r.Registration_Date
                                         }).ToList().Select(s=> new ComboBoxItemInfo
                                         {
                                             DisplayName = s.Registration_Date.ToString("yyyy/MM/dd"),
                                             SelectedValue = s.Registration_ID
                                         });
                if (queryRegistrations.Count() > 0)
                {
                    observableCollection = new ObservableCollection<ComboBoxItemInfo>(queryRegistrations.ToList().OrderByDescending(o => o.DisplayName));
                }
                return observableCollection;
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
                                            where r.Patient_ID == patients.Patient_ID
                                            orderby r.Registration_Date descending
                                            select r;
                return queryRegistrationDate.Count() > 0 ? queryRegistrationDate.First().Registration_Date : (DateTime)patients.Patient_FirstRegistrationDate;
            }
        }
    }
}
