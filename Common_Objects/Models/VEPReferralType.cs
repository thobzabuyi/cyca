using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class VEPReferralType
    {
        public apl_Department GetSpecificLocalServiceType(int referralTypeId)
        {       
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                return dbContext.apl_Department.SingleOrDefault(a => a.Department_Id == referralTypeId);
            }
            catch (Exception)
            {
                return null;
            }            
        }

        public IEnumerable<apl_Department> GetListOfReferralType()
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
                {
                return dbContext.apl_Department.ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            
        }


    }
}
