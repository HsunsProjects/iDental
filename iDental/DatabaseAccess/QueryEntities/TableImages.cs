using iDental.iDentalClass;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace iDental.DatabaseAccess.QueryEntities
{
    public class TableImages
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="agencys"></param>
        /// <param name="patients"></param>
        /// <returns></returns>
        public ObservableCollection<ImageInfo> QueryAllImagesToImageInfo(Agencys agencys, Patients patients)
        {
            using (var ide = new iDentalEntities())
            {
                var queryImages = from r in ide.Registrations
                                  where r.Patient_ID == patients.Patient_ID
                                  join i in ide.Images
                                  on r.Registration_ID equals i.Registration_ID into ri
                                  from qri in ri.DefaultIfEmpty()
                                  where qri.Image_IsEnable == true
                                  select qri;
                ObservableCollection<ImageInfo> observableCollection = new ObservableCollection<ImageInfo>(queryImages.ToList().Select(s => new ImageInfo
                {
                    Registration_Date = s.Registrations.Registration_Date,
                    Image_ID = s.Image_ID,
                    Image_Path = s.Image_Path,
                    Image_FullPath = agencys.Agency_ImagePath + s.Image_Path,
                    Image_FileName = s.Image_FileName,
                    Image_Extension = s.Image_Extension,
                    Registration_ID = s.Registration_ID,
                    CreateDate = s.CreateDate,
                    IsSelected = false
                }));
                return observableCollection;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="agencys"></param>
        /// <param name="patients"></param>
        /// <param name="registrationDate"></param>
        /// <returns></returns>
        public ObservableCollection<ImageInfo> QueryRegistrationDateImageToImageInfo(Agencys agencys, Patients patients, DateTime registrationDate)
        {
            using (var ide = new iDentalEntities())
            {
                var queryImages = from r in ide.Registrations
                                  where r.Patient_ID == patients.Patient_ID && r.Registration_Date == registrationDate.Date
                                  join i in ide.Images
                                  on r.Registration_ID equals i.Registration_ID into ri
                                  from qri in ri.DefaultIfEmpty()
                                  where qri.Image_IsEnable == true
                                  select qri;
                ObservableCollection<ImageInfo> observableCollection = new ObservableCollection<ImageInfo>(queryImages.ToList().Select(s => new ImageInfo
                {
                    Registration_Date = s.Registrations.Registration_Date,
                    Image_ID = s.Image_ID,
                    Image_Path = s.Image_Path,
                    Image_FullPath = agencys.Agency_ImagePath + s.Image_Path,
                    Image_FileName = s.Image_FileName,
                    Image_Extension = s.Image_Extension,
                    Registration_ID = s.Registration_ID,
                    CreateDate = s.CreateDate,
                    IsSelected = false
                }));
                return observableCollection;
            }
        }
    }
}
