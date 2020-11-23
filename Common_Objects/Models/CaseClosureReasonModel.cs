using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class CaseClosureReasonModel
    {
        
        public Case_Closure_Reason GetSpecificCaseClosureReading(int abuseIndicatorId)
        {
            Case_Closure_Reason abuseIndicator;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var abuseIndicatorList = (from r in dbContext.Case_Closure_Reasons
                                          where r.Case_Closure_Reason_Id.Equals(abuseIndicatorId)
                                          select r).ToList();

                abuseIndicator = (from r in abuseIndicatorList
                                  select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return abuseIndicator;
        }

        public List<Case_Closure_Reason> GetListOfCaseClosureReadings()
        {
            List<Case_Closure_Reason> abuseIndicators;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var abuseIndicatorList = (from r in dbContext.Case_Closure_Reasons
                                              select r).ToList();

                    abuseIndicators = (from r in abuseIndicatorList
                                       select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return abuseIndicators;
        }

        public List<Case_Closure_Reason> GetListOfClosure_Reasonses()
        {
            List<Case_Closure_Reason> closure_Reasonsses;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var closure_ReasonsList = (from r in dbContext.Case_Closure_Reasons
                                               select r).ToList();

                    closure_Reasonsses = (from r in closure_ReasonsList
                                          select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return closure_Reasonsses;
        }
    }
}
