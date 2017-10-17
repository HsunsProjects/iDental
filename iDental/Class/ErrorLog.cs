using System;
using System.IO;

namespace iDental.Class
{
    public class ErrorLog
    {
        /// <summary>
        /// ErrorLog 的資料夾路徑
        /// </summary>
        private static string errorLogDir = AppDomain.CurrentDomain.BaseDirectory + @"\ErrorLog";

        /// <summary>
        /// 匯出 ErrorLog
        /// </summary>
        /// <param name="ErrMsg">錯誤訊息字串</param>
        public static void ErrorMessageOutput(string ErrMsg)
        {
            if (!Directory.Exists(errorLogDir))
            {
                Directory.CreateDirectory(errorLogDir);
            }
            string fileName = DateTime.Now.ToString("yyyyMMdd") + @".txt";
            string OutputMsg = "*****" + DateTime.Now + "*****\r\n" + ErrMsg + "\r\n";
            File.AppendAllText(errorLogDir + @"\" + fileName, OutputMsg);
        }
    }
}
