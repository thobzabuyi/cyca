using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
   public class VEPIncidentDetailsModel
    {
        public VEP_IncidentInformation GetSpecificLocalIncidentDetails(int caseid)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                return dbContext.VEP_IncidentInformation.SingleOrDefault(a => a.Caseid == caseid);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
