using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace iDental.DatabaseAccess.QueryEntities
{
    public class TableTemplates_Images
    {
        /// <summary>
        /// 尋找病患模板的匯入日期
        /// </summary>
        /// <param name="patients"></param>
        /// <param name="templates"></param>
        /// <returns></returns>
        public ObservableCollection<string> QueryAllTemplatesImagesImportDate(Patients patients, Templates templates)
        {
            using (var ide = new iDentalEntities())
            {
                ObservableCollection<string> observableCollection;
                var queryImportDate = (from ti in ide.Templates_Images
                                       where ti.Patient_ID == patients.Patient_ID && ti.Template_ID == templates.Template_ID
                                       group ti by ti.Template_Image_ImportDate into tii
                                       select new
                                       {
                                           ImportDate = tii.Key
                                       }).ToList().Select(s => s.ImportDate.ToString("yyyy/MM/dd"));
                observableCollection = new ObservableCollection<string>(queryImportDate);

                return observableCollection;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="patients"></param>
        /// <param name="templates"></param>
        /// <returns></returns>
        public ObservableCollection<Templates_Images> QueryTemplatesImagesImportDateAndReturnFullImagePath(Agencys agencys, Patients patients, Templates templates, DateTime templateImportDate)
        {
            using (var ide = new iDentalEntities())
            {
                ObservableCollection<Templates_Images> observableCollection;
                var queryTemplatesImages = (from iie in ide.Templates_Images
                                    where iie.Template_ID == templates.Template_ID &&
                                    iie.Patient_ID == patients.Patient_ID &&
                                    iie.Template_Image_ImportDate == templateImportDate.Date
                                    select new
                                    {
                                        Template_Image_ID = iie.Template_Image_ID,
                                        Template_Image_Number = iie.Template_Image_Number,
                                        Template_Image_ImportDate = iie.Template_Image_ImportDate,
                                        Template_ID = iie.Template_ID,
                                        Image_ID = iie.Image_ID,
                                        Image_FullPath = agencys.Agency_ImagePath + iie.Image_Path,
                                        Patient_ID = iie.Patient_ID
                                    }).ToList().Select(s => new Templates_Images
                                    {
                                        Template_Image_ID = s.Template_Image_ID,
                                        Template_Image_Number = s.Template_Image_Number,
                                        Template_Image_ImportDate = s.Template_Image_ImportDate,
                                        Template_ID = s.Template_ID,
                                        Image_ID = s.Image_ID,
                                        Image_Path = s.Image_FullPath,
                                        Patient_ID = s.Patient_ID
                                    });
                observableCollection = new ObservableCollection<Templates_Images>(queryTemplatesImages);
                return observableCollection;
            }
        }

        /// <summary>
        /// 新增或更新Templates_Images
        /// </summary>
        /// <param name="patients"></param>
        /// <param name="templates"></param>
        /// <param name="templateImportDate"></param>
        /// <param name="imageID"></param>
        /// <param name="imagePath"></param>
        /// <param name="tiNumber"></param>
        public string InsertOrUpdateTemplatesImages(Patients patients, Templates templates, DateTime templateImportDate, int imageID, string imagePath, string tiNumber)
        {
            using (var ide = new iDentalEntities())
            {
                Templates_Images templates_Images = new Templates_Images();
                var IsImageExist = from iie in ide.Templates_Images
                                   where iie.Template_ID == templates.Template_ID &&
                                   iie.Template_Image_Number == tiNumber &&
                                   iie.Template_Image_ImportDate == templateImportDate.Date &&
                                   iie.Patient_ID == patients.Patient_ID
                                   select iie;
                if (IsImageExist.Count() > 0)
                {
                    templates_Images = IsImageExist.First();
                    templates_Images.Image_ID = imageID;
                    templates_Images.Image_Path = imagePath;
                    ide.SaveChanges();
                }
                else
                {
                    templates_Images.Template_Image_Number = tiNumber;
                    templates_Images.Template_ID = templates.Template_ID;
                    templates_Images.Template_Image_ImportDate = templateImportDate.Date;
                    templates_Images.Image_ID = imageID;
                    templates_Images.Image_Path = imagePath;
                    templates_Images.Patient_ID = patients.Patient_ID;

                    ide.Templates_Images.Add(templates_Images);
                    ide.SaveChanges();
                }
                return templates_Images.Template_Image_ID.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template_image_id"></param>
        public string RemoveAndReturnUidTemplatesImages(int template_image_id)
        {
            using (var ide = new iDentalEntities())
            {
                var qti = from ti in ide.Templates_Images
                         where ti.Template_Image_ID == template_image_id
                         select ti;
                if (qti.Count() > 0)
                {
                    Templates_Images templates_Images = new Templates_Images();
                    templates_Images = qti.First();
                    templates_Images.Image_ID = null;
                    templates_Images.Image_Path = string.Empty;
                    ide.SaveChanges();
                    var qt = ide.Templates.Where(w => w.Template_ID == templates_Images.Template_ID).First();
                    return qt.Template_DefaultImage;
                }
                return null;
            }
        }
    }
}
