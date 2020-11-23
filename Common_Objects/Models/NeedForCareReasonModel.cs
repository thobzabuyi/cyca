using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class NeedForCareReasonModel
    {
        public Need_for_Care_Reason_Item GetSpecificNeedForCareReasonItem(int needForCareReasonItemId)
        {
            Need_for_Care_Reason_Item needForCareReasonItem;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var needForCareReasonItemList = (from r in dbContext.Need_for_Care_Reason_Items
                                                 where r.Need_for_Care_Reason_Id.Equals(needForCareReasonItemId)
                                                 select r).ToList();

                needForCareReasonItem = (from r in needForCareReasonItemList
                                         select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return needForCareReasonItem;
        }

        public List<Need_for_Care_Reason_Item> GetListOfNeedForCareReasonItems()
        {
            List<Need_for_Care_Reason_Item> needForCareReasonItems;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var needForCareReasonItemList = (from r in dbContext.Need_for_Care_Reason_Items
                                                     select r).ToList();

                    needForCareReasonItems = (from r in needForCareReasonItemList
                                              select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return needForCareReasonItems;
        }
    }
}
