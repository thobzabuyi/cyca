using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class SpecialNeedModel
    {
        public apl_Special_Need GetSpecificSpecialNeed(int specialNeedId)
        {
            apl_Special_Need specialNeed;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var specialNeedList = (from r in dbContext.apl_Special_Need
                                      where r.Special_Needs_Id.Equals(specialNeedId)
                                      select r).ToList();

                specialNeed = (from r in specialNeedList
                               select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return specialNeed;
        }

        public List<apl_Special_Need> GetSelectedListOfSpecialNeeds(int specialNeedId)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var query = (from pt in dbContext.apl_Special_Need
                             join ttab in dbContext.Int_Person_SpecialNeed on pt.Special_Needs_Id equals ttab.SpecialNeed_Id
                             where ttab.Person_Id == specialNeedId
                             select pt).ToList();

                return query;
                //return dbContext.VEP_VictimizationType.Where(a => a. == victimizationTypeId).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }


        public List<apl_Special_Need> GetListOfSpecialNeeds()
        {
            List<apl_Special_Need> specialNeeds;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var specialNeedList = (from r in dbContext.apl_Special_Need
                                          select r).ToList();

                    specialNeeds = (from r in specialNeedList
                                    select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return specialNeeds;
        }
    }
}
