using iDental.Class;
using iDental.DatabaseAccess.DatabaseObject;
using iDental.DatabaseAccess.QueryEntities;
using System;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace iDental.Views
{
    /// <summary>
    /// Login.xaml 的互動邏輯
    /// </summary>
    public partial class Login : Window
    {
        /// <summary>
        /// 載入的機構資訊
        /// </summary>
        public Agencys Agencys { get; private set; }
        /// <summary>
        /// 載入的病患資訊
        /// </summary>
        public Patients Patients { get; private set; }
        /// <summary>
        /// 影像路徑是否存在
        /// </summary>
        public bool IsExistImagePath = false;
        /// <summary>
        /// 試用期，合法使用
        /// </summary>
        private bool IsValidTrialPeriod = false;
        /// <summary>
        /// 訊息視窗提示
        /// </summary>
        private string MessageBoxTips = string.Empty;
        /// <summary>
        /// Login 頁面回傳
        /// </summary>
        private bool ReturnDialogResult = false;

        public Login()
        {
            InitializeComponent();
        }

        private void Window_Login_ContentRendered(object sender, EventArgs e)
        {
            //生命週期在 Window.Loaded 後觸發
            //畫面Rendered完顯示
            Loading();
        }

        /// <summary>
        /// 開始載入判斷
        /// </summary>
        private void Loading()
        {
            try
            {
                //取得本機訊息
                //HostName、IP
                string hostName = Dns.GetHostName();
                string localIP = string.Empty;
                IPHostEntry ipHostEntry = Dns.GetHostEntry(hostName);
                foreach (IPAddress ip in ipHostEntry.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        localIP = ip.ToString();
                    }
                }

                TextBlockStatus.Dispatcher.Invoke(() =>
                {
                    TextBlockStatus.Text = "伺服器位置確認中";
                }, DispatcherPriority.Render);

                //判斷app.config
                if (!ConfigManage.IsHaveValue("Server"))//尚未設置
                {
                    AnswerDialogOne answerDialogOne = new AnswerDialogOne("請輸入伺服器位置:", "IP");
                    if (answerDialogOne.ShowDialog() == true)
                    {
                        //寫入config Server 欄位
                        string serverIP = answerDialogOne.Answer;
                        ConfigManage.AddUpdateAppConfig("Server", serverIP);
                    }
                }

                TextBlockStatus.Dispatcher.Invoke(() =>
                {
                    TextBlockStatus.Text = "嘗試連接伺服器...";
                }, DispatcherPriority.Render);

                //連接Server connection
                if (new ConnectionString().CheckConnection())
                {
                    TextBlockStatus.Dispatcher.Invoke(() =>
                    {
                        TextBlockStatus.Text = "取得本機資訊";
                    }, DispatcherPriority.Render);


                    TableAgencys tableAgencys = new TableAgencys();
                    //取得已經驗證的機構
                    Agencys agencys = tableAgencys.QueryVerifyAgencys();
                    if (agencys != null)
                    {
                        Agencys = agencys;

                        //開始判斷本機與伺服器關係
                        TableClients tableClients = new TableClients();
                        Clients clients = tableClients.QueryClient(hostName);
                        if (clients != null)
                        {
                            if (!string.IsNullOrEmpty(clients.Agency_VerificationCode) && clients.Agency_VerificationCode.Equals(agencys.Agency_VerificationCode))
                            {
                                if (clients.Client_IsVerify)
                                {
                                    TextBlockStatus.Dispatcher.Invoke(() =>
                                    {
                                        TextBlockStatus.Text = "本機已認證";
                                    }, DispatcherPriority.Render);

                                    //本機與伺服器認證通過
                                    //判斷軟體使用期限
                                    CheckServerTrialPeriod(agencys);
                                }
                                else
                                {
                                    MessageBoxTips = "此台電腦已經被停用，請聯絡資訊廠商...";
                                }
                            }
                            else
                            {
                                MessageBoxTips = "此台電腦與伺服器的驗證碼不符，請聯絡資訊廠商";
                            }
                        }
                        else
                        {
                            //第一次使用，輸入驗證碼
                            AnswerDialogOne answerDialogOne = new AnswerDialogOne("此台電腦為第一次登入，請輸入產品驗證碼:", "Verify");
                            if (answerDialogOne.ShowDialog() == true)
                            {
                                string verificationCodeClient = string.Empty;
                                verificationCodeClient = KeyGenerator.SHA384Encode(answerDialogOne.Answer);
                                if (verificationCodeClient.Equals(agencys.Agency_VerificationCode))
                                {
                                    TextBlockStatus.Dispatcher.Invoke(() =>
                                    {
                                        TextBlockStatus.Text = "本機與伺服器驗證成功";
                                    }, DispatcherPriority.Render);

                                    Clients insertClients = new Clients()
                                    {
                                        Client_HostName = hostName,
                                        Client_IP = localIP,
                                        Client_IsVerify = true,
                                        Agency_VerificationCode = verificationCodeClient
                                    };
                                    tableClients.InsertClient(insertClients);

                                    //本機與伺服器認證通過
                                    //判斷軟體使用期限
                                    CheckServerTrialPeriod(agencys);
                                }
                                else
                                {
                                    MessageBoxTips = "與伺服器的驗證碼不符，請聯絡資訊廠商";
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBoxTips = "伺服器尚未被驗證，無法使用，請聯絡資訊廠商";
                    }

                }
                else
                {
                    MessageBoxTips = "伺服器連接失敗";
                    ConfigManage.AddUpdateAppConfig("Server", "");
                }

                //登入成功
                //show MessageBox
                if (ReturnDialogResult)
                {
                    TextBlockStatus.Dispatcher.Invoke(() =>
                    {
                        TextBlockStatus.Text = "載入病患資料";
                    }, DispatcherPriority.Render);
                    LoadPatient();

                    TextBlockStatus.Dispatcher.Invoke(() =>
                    {
                        TextBlockStatus.Text = "影像路徑測試中，請稍候";
                    }, DispatcherPriority.Render);

                    IsExistImagePath = PathCheck.IsPathExist(Agencys.Agency_ImagePath);

                    TextBlockStatus.Dispatcher.Invoke(() =>
                    {
                        TextBlockStatus.Text = "成功登入，歡迎使用iDental";
                    }, DispatcherPriority.Render);

                    if (IsValidTrialPeriod)//還在試用期內可以使用
                    {
                        MessageBox.Show(MessageBoxTips, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    TextBlockStatus.Dispatcher.Invoke(() =>
                    {
                        TextBlockStatus.Text = "登入失敗，原因:" + MessageBoxTips;
                    }, DispatcherPriority.Render);

                    MessageBox.Show(MessageBoxTips, "提示", MessageBoxButton.OK, MessageBoxImage.Stop);
                }

                //寫入ConnectingLog資訊
                TableConnectingLogs tableConnectingLogs = new TableConnectingLogs();
                ConnectingLogs connectingLogs = new ConnectingLogs()
                {
                    ConnectingLog_HostName = hostName,
                    ConnectingLog_IP = localIP,
                    ConnectingLog_Error = MessageBoxTips,
                    ConnectingLog_IsPermit = ReturnDialogResult
                };
                tableConnectingLogs.InsertConnectingLog(connectingLogs);

                Thread.Sleep(2000);
                //回傳結果
                DialogResult = ReturnDialogResult;
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorMessageOutput(ex.ToString());
                DialogResult = false;
            }
            Close();
        }

        /// <summary>
        /// 本機資訊與伺服器資訊都沒問題後的版本判斷
        /// </summary>
        /// <param name="agencys">Agencys</param>
        private void CheckServerTrialPeriod(Agencys agencys)
        {
            //開始判斷伺服器認證碼的使用權限
            TextBlockStatus.Dispatcher.Invoke(() =>
            {
                TextBlockStatus.Text = "取得伺服器認證資訊";
            }, DispatcherPriority.Render);

            if (agencys.Agency_IsVerify)
            {
                if (agencys.Agency_IsTry)
                {
                    if (agencys.Agency_TrialPeriod < DateTime.Now.Date)
                    {
                        MessageBoxTips = "試用期限已到，請聯絡資訊廠商";
                    }
                    else
                    {
                        IsValidTrialPeriod = true;

                        MessageBoxTips = "此為試用版本，試用日期至" + ((DateTime)agencys.Agency_TrialPeriod).ToShortDateString();
                        
                        ReturnDialogResult = true;
                    }
                }
                else
                {
                    ReturnDialogResult = true;
                }
            }
            else
            {
                MessageBoxTips = "此驗證碼已停用，如欲使用請聯絡資訊廠商";
            }
        }

        #region

        private void LoadPatient()
        {
            //Environment.GetCommandLineArgs()
            //args[0] 為程式啟動路徑
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                Patients p = new Patients()
                {
                    Patient_ID = !string.IsNullOrEmpty(args[1].ToString()) ? args[1].ToString() : string.Empty,
                    Patient_Number = !string.IsNullOrEmpty(args[2].ToString()) ? args[2].ToString() : string.Empty,
                    Patient_Name = !string.IsNullOrEmpty(args[3].ToString()) ? args[3].ToString() : string.Empty,
                    Patient_Gender = TransGender(args[4].ToString()),
                    Patient_Birth = DateTime.TryParse(args[5].ToString(), out DateTime patientBirth) ? DateTime.Parse(args[5].ToString()) : default(DateTime),
                    Patient_IDNumber = !string.IsNullOrEmpty(args[6].ToString()) ? args[6].ToString() : string.Empty
                };
                TablePatients tablePatients = new TablePatients();
                Patients = tablePatients.QueryNewOldPatient(p);
            }
            else
            {
                Patients = null;
            }
        }

        private bool TransGender(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                if (input.ToUpper().Equals("M"))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
