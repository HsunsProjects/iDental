using System.Linq;

namespace iDental.DatabaseAccess.QueryEntities
{
    public class TableClients
    {
        /// <summary>
        /// 查詢登入者資訊
        /// </summary>
        /// <param name="hostName">登入者電腦名稱</param>
        /// <returns></returns>
        public Clients QueryClient(string hostName)
        {
            using (var ide = new iDentalEntities())
            {
                var queryClients = from c in ide.Clients
                              where c.Client_HostName == hostName
                              select c;
                return queryClients.Count() > 0 ? queryClients.First() : null;
            }
        }
        /// <summary>
        /// 寫入資訊者資訊
        /// </summary>
        /// <param name="clients">Clients</param>
        public void InsertClient(Clients clients)
        {
            using (var ide = new iDentalEntities())
            {
                ide.Clients.Add(clients);
                ide.SaveChanges();
            }
        }
    }
}
