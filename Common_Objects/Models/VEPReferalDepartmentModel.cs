using Common_Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class VEPReferalDepartmentModel
    {
        public apl_Department GetSpecificLocalDepartment(int referralTypeId)
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

        public IEnumerable<apl_Department> GetListOfServiceType()
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
