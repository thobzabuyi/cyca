using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class ChronicIllnessModel
    {
        public Chronic_Illness GetSpecificChronicIllness(int chronicIllnessId)
        {
            Chronic_Illness chronicIllness;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var chronicIllnessList = (from r in dbContext.Chronic_Illnesses
                                          where r.Chronic_Illness_Id.Equals(chronicIllnessId)
                                          select r).ToList();

                chronicIllness = (from r in chronicIllnessList
                                  select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return chronicIllness;
        }

        public List<Chronic_Illness> GetListOfChronicIllnesses()
        {
            List<Chronic_Illness> chronicIllnesses;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var chronicIllnessList = (from r in dbContext.Chronic_Illnesses
                                              select r).ToList();

                    chronicIllnesses = (from r in chronicIllnessList
                                        select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return chronicIllnesses;
        }
    }
}
