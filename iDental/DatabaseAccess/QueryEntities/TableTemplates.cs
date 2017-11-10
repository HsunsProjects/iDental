using System.Collections.ObjectModel;
using System.Linq;

namespace iDental.DatabaseAccess.QueryEntities
{
    public class TableTemplates
    {
        public ObservableCollection<Templates> QueryAllTemplates()
        {
            using (var ide = new iDentalEntities())
            {
                ObservableCollection<Templates> observableCollection;

                var temp = from t in ide.Templates
                           where t.Template_IsEnable == true
                           select t;
                observableCollection = new ObservableCollection<Templates>(temp);

                return observableCollection;
            }
        }
    }
}
