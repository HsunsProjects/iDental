using iDental.Class;
using iDental.DatabaseAccess.QueryEntities;
using iDental.iDentalClass;
using iDental.ViewModels.ViewModelBase;
using iDental.Views;
using iDental.Views.UserControlViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace iDental.ViewModels.UserControlViewModels
{
    public class PatientInformationViewModel : PropertyChangedBase
    {
        private Agencys Agencys { get; set; }
        
        #region 設定病患基本資料
        private Patients patients;
        public Patients Patients
        {
            get { return patients; }
            set
            {
                patients = value;
                OnPropertyChanged("Patients");

                //設定病患資料
                Patient_Number = patients.Patient_Number;
                Patient_Name = patients.Patient_Name;
                Patient_IDNumber = patients.Patient_IDNumber;
                Patient_Gender = patients.Patient_Gender;
                Patient_Birth = patients.Patient_Birth;
                if (PathCheck.IsFileExist(Agencys.Agency_ImagePath + patients.Patient_Photo))
                {
                    Patient_Photo = new CreateBitmapImage().BitmapImageShow(Agencys.Agency_ImagePath + patients.Patient_Photo, 400);
                }

                Patient_FirstRegistrationDate = patients.Patient_FirstRegistrationDate == null ? string.Empty : ((DateTime)patients.Patient_FirstRegistrationDate).ToString("yyyy/MM/dd");

                //設定病患分類
                PatientCategoryInfo = new TablePatientCategorys().QueryPatientCheckedPatientCategoryInfo(patients);
                //設定掛號日
                RegistrationSetting();
                //預設載入最近掛號
                if (RegistrationsList.Where(w => w.DisplayName == Patient_LastRegistrationDate).Count() > 0)
                {
                    ComboBoxItemInfo = RegistrationsList.Where(w => w.DisplayName == Patient_LastRegistrationDate).First();
                }
            }
        }

        private string patient_Number;
        public string Patient_Number
        {
            get { return patient_Number; }
            set
            {
                patient_Number = value;
                OnPropertyChanged("Patient_Number");
            }
        }

        private string patient_Name;
        public string Patient_Name
        {
            get { return patient_Name; }
            set
            {
                patient_Name = value;
                OnPropertyChanged("Patient_Name");
            }
        }

        private string patient_IDNumber;
        public string Patient_IDNumber
        {
            get { return patient_IDNumber; }
            set
            {
                patient_IDNumber = value;
                OnPropertyChanged("Patient_IDNumber");
            }
        }

        private bool patient_Gender = true;
        public bool Patient_Gender
        {
            get { return patient_Gender; }
            set
            {
                patient_Gender = value;
                OnPropertyChanged("Patient_Gender");
            }
        }

        private DateTime patient_Birth = DateTime.Now;
        public DateTime Patient_Birth
        {
            get { return patient_Birth; }
            set
            {
                patient_Birth = value;
                OnPropertyChanged("Patient_Birth");
            }
        }

        private BitmapImage patient_Photo = new BitmapImage(new Uri(@"pack://application:,,,/iDental;component/Resource/Image/avatar.jpg", UriKind.Absolute));
        public BitmapImage Patient_Photo
        {
            get { return patient_Photo; }
            set
            {
                patient_Photo = value;
                OnPropertyChanged("Patient_Photo");
            }
        }

        private string patient_FirstRegistrationDate;
        public string Patient_FirstRegistrationDate
        {
            get { return patient_FirstRegistrationDate; }
            set
            {
                patient_FirstRegistrationDate = value;
                OnPropertyChanged("Patient_FirstRegistrationDate");
            }
        }

        private string patient_LastRegistrationDate;
        public string Patient_LastRegistrationDate
        {
            get { return patient_LastRegistrationDate; }
            set
            {
                patient_LastRegistrationDate = value;
                OnPropertyChanged("Patient_LastRegistrationDate");
            }
        }

        private List<PatientCategoryInfo> patientCategoryInfo;
        public List<PatientCategoryInfo> PatientCategoryInfo
        {
            get { return patientCategoryInfo; }
            set
            {
                patientCategoryInfo = value;
                OnPropertyChanged("PatientCategoryInfo");
            }
        }
        #endregion

        #region 設定是否可見資料(default:true)
        private bool isEnablePatientInformation = true;
        public bool IsEnablePatientInformation
        {
            get { return isEnablePatientInformation; }
            set
            {
                isEnablePatientInformation = value;
                OnPropertyChanged("IsEnablePatientInformation");
            }
        }
        #endregion

        #region 掛號日、匯入日期

        private ObservableCollection<ComboBoxItemInfo> registrationsList;
        public ObservableCollection<ComboBoxItemInfo> RegistrationsList
        {
            get { return registrationsList; }
            set
            {
                registrationsList = value;
                OnPropertyChanged("RegistrationsList");
            }
        }

        private ComboBoxItemInfo comboBoxItemInfo;
        public ComboBoxItemInfo ComboBoxItemInfo
        {
            get { return comboBoxItemInfo; }
            set
            {
                comboBoxItemInfo = value;
                OnPropertyChanged("ComboBoxItemInfo");
                if (comboBoxItemInfo != null)
                {
                    if (!comboBoxItemInfo.SelectedValue.Equals(-1))
                    {
                        //設定匯入日期，並載入那天的影像
                        ImageInfo = new TableImages().QueryRegistrationDateImageToImageInfo(Agencys, Patients, DateTime.Parse(comboBoxItemInfo.DisplayName)); ;
                    }
                    else
                    {
                        //載入全部影像
                        ImageInfo = new TableImages().QueryAllImagesToImageInfo(Agencys, Patients);
                    }
                }
            }
        }

        private DateTime importDate = DateTime.Now;
        public DateTime ImportDate
        {
            get { return importDate; }
            set
            {
                if (importDate != value)
                {
                    importDate = value;
                    OnPropertyChanged("ImportDate");
                }
            }
        }

        #endregion

        #region FinctionTab
        /// <summary>
        /// binding Tab ItemSource來源
        /// </summary>
        private ObservableCollection<TabItem> functionsTabs;
        public ObservableCollection<TabItem> FunctionsTabs
        {
            get { return functionsTabs; }
            set
            {
                functionsTabs = value;
                OnPropertyChanged("FunctionsTabs");
            }
        }

        /// <summary>
        /// Selected Tab頁面(載入圖片)
        /// </summary>
        private TabItem selectedTabItem;
        public TabItem SelectedTabItem
        {
            get { return selectedTabItem; }
            set
            {
                selectedTabItem = value;
                OnPropertyChanged("SelectedTabItem");
                //更新TabControl 分頁   的ImageInfo 來源
                //TabSelectedChanged 重新刷新TAB頁面內的來源
                UpdateImageInfo();
            }
        }
        #endregion

        #region 影像
        private ObservableCollection<ImageInfo> imageInfo;
        public ObservableCollection<ImageInfo> ImageInfo
        {
            get { return imageInfo; }
            set
            {
                imageInfo = value;
                OnPropertyChanged("ImageInfo");
                try
                {
                    DisplayImageInfo = new MTObservableCollection<ImageInfo>();
                    if (imageInfo != null && ImageInfo.Count > 0)
                    {
                        ProgressDialog progressDialog = new ProgressDialog();

                        progressDialog.Dispatcher.Invoke(() =>
                        {
                            progressDialog.PText = "圖片載入中( 0 / " + imageInfo.Count + " )";
                            progressDialog.PMinimum = 0;
                            progressDialog.PValue = 0;
                            progressDialog.PMaximum = imageInfo.Count;
                            progressDialog.Show();
                        });

                        //multi - thread
                        Task t = Task.Factory.StartNew(() =>
                        {
                            Parallel.ForEach(imageInfo, ii =>
                            {
                                ii.BitmapImage = new CreateBitmapImage().BitmapImageShow(ii.Image_FullPath, 800);
                                DisplayImageInfo.Add(ii);

                                progressDialog.Dispatcher.Invoke(() =>
                                {
                                    progressDialog.PValue++;
                                    progressDialog.PText = "圖片載入中( " + progressDialog.PValue + " / " + imageInfo.Count + " )";
                                });

                                UpdateImageInfo();
                            });
                        }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).ContinueWith(cw =>
                        {
                            progressDialog.Dispatcher.Invoke(() =>
                            {
                                progressDialog.PText = "載入完成";
                                progressDialog.Close();
                            });
                            GC.Collect();
                        });
                    }
                    else
                    {
                        UpdateImageInfo();
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.ErrorMessageOutput(ex.ToString());
                    MessageBox.Show("載入圖片發生錯誤", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private MTObservableCollection<ImageInfo> displayImageInfo = new MTObservableCollection<ImageInfo>();
        public MTObservableCollection<ImageInfo> DisplayImageInfo
        {
            get { return displayImageInfo; }
            set
            {
                displayImageInfo = value;
                OnPropertyChanged("DisplayImageInfo");
            }
        }
        #endregion

        private FunctionList functionList;
        private FunctionTemplate functionTemplate;

        public PatientInformationViewModel()
        {
            //無病患帶入
            IsEnablePatientInformation = false;
        }

        public PatientInformationViewModel(Agencys agencys, Patients patients)
        {
            Agencys = agencys;
            Patients = patients;            

            FunctionsSetting();
        }

        public void RegistrationSetting()
        {
            //設定掛號日
            RegistrationsList = new TableRegistrations().QueryRegistrationsList(patients);
            Patient_LastRegistrationDate = RegistrationsList.Count() > 0 ? RegistrationsList[0].DisplayName : Patient_FirstRegistrationDate;
            RegistrationsList.Insert(0, new ComboBoxItemInfo("全部", -1));
        }

        private void FunctionsSetting()
        {
            FunctionsTabs = new ObservableCollection<TabItem>();

            foreach (Functions functions in TableFunctions.QueryFunctions())
            {
                TabItem fTabItem = new TabItem
                {
                    Header = functions.Function_Title,
                    Uid = functions.Function_ID.ToString()
                };

                switch (fTabItem.Uid)
                {
                    case "1":
                        functionList = new FunctionList(Agencys, Patients, DisplayImageInfo);
                        functionList.ReturnValueCallback += new FunctionList.ReturnValueDelegate(RegistrationSetting);
                        functionList.ReturnRenewCallback += new FunctionList.ReturnRenewDelegate(RenewImageSource);
                        break;
                    case "2":
                        functionTemplate = new FunctionTemplate(Agencys, Patients, DisplayImageInfo);
                        functionTemplate.ReturnValueCallback += new FunctionTemplate.ReturnValueDelegate(RenewUsercontrol);
                        break;
                }

                FunctionsTabs.Add(fTabItem);

                if (functions.Function_ID == Agencys.Function_ID)
                {
                    SelectedTabItem = fTabItem;
                }
            }
        }

        /// <summary>
        /// 更新TabControl 分頁的ImageInfo 來源
        /// </summary>
        private void UpdateImageInfo()
        {
            SelectedTabItem.Dispatcher.Invoke(() =>
            {
                switch (SelectedTabItem.Uid)
                {
                    case "1":
                        SelectedTabItem.Content = functionList;
                        functionList.Dispatcher.Invoke(() =>
                        {
                            functionList.DisplayImageInfo = DisplayImageInfo;
                        });
                        break;
                    case "2":
                        SelectedTabItem.Content = functionTemplate;
                        functionTemplate.Dispatcher.Invoke(()=>
                        {
                            functionTemplate.DisplayImageInfo = DisplayImageInfo;
                        });
                        break;
                }
            });
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="registrationDate"></param>
        public void RenewUsercontrol(DateTime registrationDate)
        {
            //wifi auto 載入  取掛號資訊清單 Registration
            RegistrationSetting();
            ComboBoxItemInfo = RegistrationsList.Where(w => w.DisplayName == registrationDate.ToString("yyyy/MM/dd")).First();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="registrationDate"></param>
        public void RenewImageSource()
        {
            ComboBoxItemInfo cbii = ComboBoxItemInfo;
            //取掛號資訊清單 Registration
            RegistrationSetting();

            var querySelectItem = from r in RegistrationsList
                               where r.DisplayName.Equals(cbii.DisplayName)
                               select r;
            if (querySelectItem.Count() > 0)
            {
                ComboBoxItemInfo = querySelectItem.First() as ComboBoxItemInfo;
            }
            else
            {
                ImageInfo = null;
            }
        }
    }
}
