using System.Collections.Generic;
using System.Linq;

namespace iDental.DatabaseAccess.QueryEntities
{
    public static class TableFunctions
    {
        /// <summary>
        /// 載入功能頁面
        /// </summary>
        /// <returns></returns>
        public static List<Functions> QueryFunctions()
        {
            using (var ide = new iDentalEntities())
            {
                List<Functions> list = new List<Functions>();
                var queryFunctions = from f in ide.Functions
                                     where f.Function_IsEnable == true
                                     select f;
                if (queryFunctions.Count() > 0)
                {
                    list = queryFunctions.ToList();
                }

                return list;
            }
        }
    }
}
