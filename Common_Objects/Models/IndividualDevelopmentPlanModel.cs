using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class IndividualDevelopmentPlanModel
    {
        public List<ACM_IndividualDevelopmentPlan> GetListOfIndividualDevelopmentPlans()
        {
            List<ACM_IndividualDevelopmentPlan> listOfIndividualDevelopmentPlans;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var IndividualDevelopmentPlans = (from e in dbContext.ACM_IndividualDevelopmentPlan
                                                select e).ToList();

                listOfIndividualDevelopmentPlans = (from e in IndividualDevelopmentPlans
                                                 select e).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return listOfIndividualDevelopmentPlans;

        }
    }
}
