using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class RiskIndicatorModel
    {
        public Risk_Indicator GetSpecificRiskIndicator(int riskIndicatorId)
        {
            Risk_Indicator riskIndicator;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var riskIndicatorList = (from r in dbContext.Risk_Indicators
                                         where r.Risk_Indicator_Id.Equals(riskIndicatorId)
                                         select r).ToList();

                riskIndicator = (from r in riskIndicatorList
                                 select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return riskIndicator;
        }

        public List<Risk_Indicator> GetListOfRiskIndicators()
        {
            List<Risk_Indicator> riskIndicators;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var riskIndicatorList = (from r in dbContext.Risk_Indicators
                                             select r).ToList();

                    riskIndicators = (from r in riskIndicatorList
                                      select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return riskIndicators;
        }
    }
}
