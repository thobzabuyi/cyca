using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class VEPSettlementTypeModel
    {
        public VEP_SettlementType GetSpecificSettlementType(int settlementTypeId)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                return dbContext.VEP_SettlementType.SingleOrDefault(a => a.Id == settlementTypeId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<VEP_SettlementType> GetListOfSettlementType()
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                return dbContext.VEP_SettlementType.ToList();
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
