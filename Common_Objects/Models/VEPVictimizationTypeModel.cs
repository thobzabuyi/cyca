using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class VEPVictimizationTypeModel
    {
        public VEP_VictimizationType GetSpecificVictimizationType(int victimizationTypeId)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                return dbContext.VEP_VictimizationType.SingleOrDefault(a => a.Id == victimizationTypeId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<VEP_VictimizationType> GetSelectedVictimizationType(int victimizationTypeId)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var query = (from pt in dbContext.VEP_VictimizationType
                            join ttab in dbContext.VEP_VictimizationTypeDetails on pt.Id equals ttab.VictimizationType
                            where ttab.Case_Id == victimizationTypeId
                            select pt).ToList();

                return query;
                //return dbContext.VEP_VictimizationType.Where(a => a. == victimizationTypeId).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<VEP_VictimizationType> GetListOfVictimizationType()
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                return dbContext.VEP_VictimizationType.ToList();
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
