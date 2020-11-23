using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class VEPServiceTypeModel
    {
        public VEP_ServiceType GetSpecificLocalServiceType(int serviceTypeId)
        {       
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                return dbContext.VEP_ServiceType.SingleOrDefault(a => a.ServiceTypeId == serviceTypeId);
            }
            catch (Exception)
            {
                return null;
            }            
        }

        public IEnumerable<VEP_ServiceType> GetListOfServiceType()
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
                {
                return dbContext.VEP_ServiceType.ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            
        }
    }
}
