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
        /// <summary>
        /// 更新機構資訊
        /// </summary>
        /// <param name="agencys"></param>
        /// <param name="imagePath"></param>
        /// <param name="wifiCardPath"></param>
        /// <param name="viewType"></param>
        /// <param name="functionID"></param>
        /// <returns></returns>
        public void UpdateAgency(Agencys agencys, string imagePath, string wifiCardPath, string viewType, int functionID)
        {
            using (var ide = new iDentalEntities())
            {
                Agencys newAgencys = (from a in ide.Agencys
                                      where a.Agency_Code == agencys.Agency_Code
                                      select a).First();
                newAgencys.Agency_ImagePath = imagePath;
                newAgencys.Agency_WifiCardPath = wifiCardPath;
                newAgencys.Agency_ViewType = viewType;
                newAgencys.Function_ID = functionID;
                ide.SaveChanges();
            }
        }
    }
}
