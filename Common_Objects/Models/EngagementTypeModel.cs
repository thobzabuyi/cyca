using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
   public class EngagementTypeModel
    {
        public List<ACM_EngagementType> GetListOfEngagementTypes()
        {
            List<ACM_EngagementType>engagementtypes;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var engagementTypeList = (from i in dbContext.ACM_EngagementType select i).ToList();
                    engagementtypes = (from i in engagementTypeList select i).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return engagementtypes;
        }

    }
}
