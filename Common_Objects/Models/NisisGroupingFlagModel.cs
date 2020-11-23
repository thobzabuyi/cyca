using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class NisisGroupingFlagModel
    {
        public NISIS_Grouping_Flag GetSpecificGroupingFlags(int groupingFlagId)
        {
            NISIS_Grouping_Flag groupingFlag;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var groupingFlagList = (from r in dbContext.NISIS_Grouping_Flag_Items
                                        where r.NISIS_Grouping_Flag_Id.Equals(groupingFlagId)
                                        select r).ToList();

                groupingFlag = (from r in groupingFlagList
                                select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return groupingFlag;
        }

        public List<NISIS_Grouping_Flag> GetListOfGroupingFlags()
        {
            List<NISIS_Grouping_Flag> groupingFlags;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var groupingFlagList = (from x in dbContext.NISIS_Grouping_Flag_Items
                                        select x).ToList();

                groupingFlags = (from x in groupingFlagList
                                 select x).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return groupingFlags;
        }

    }
}
