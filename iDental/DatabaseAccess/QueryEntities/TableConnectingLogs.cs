using System.Linq;

namespace iDental.DatabaseAccess.QueryEntities
{
    public class TableConnectingLogs
    {
        /// <summary>
        /// 寫入連線資訊
        /// </summary>
        /// <param name="connectingLogs">ConnectingLogs</param>
        public void InsertConnectingLog(ConnectingLogs connectingLogs)
        {
            using (var ide = new iDentalEntities())
            {
                ide.ConnectingLogs.Add(connectingLogs);
                ide.SaveChanges();
            }
        }
    }
}
