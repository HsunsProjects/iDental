using System.Linq;

namespace iDental.DatabaseAccess.QueryEntities
{
    public class TableAgencys
    {
        /// <summary>
        /// 查詢已認證的機構
        /// </summary>
        /// <returns>Agencys</returns>
        public Agencys QueryVerifyAgencys()
        {
            using (var ide = new iDentalEntities())
            {
                var queryAgencys = from a in ide.Agencys
                                   where a.Agency_IsVerify == true
                                   select a;
                return queryAgencys.Count() > 0 ? queryAgencys.First() : null;
            }
        }
    }
}
